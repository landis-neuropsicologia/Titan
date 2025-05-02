namespace Titan.Domain.ValueObjects;

public sealed class Currency : ValueObject
{
    private static readonly string key = "Currency_";

    #region Common Currencies

    public static readonly string USD = "USD";
    public static readonly string EUR = "EUR";
    public static readonly string GBP = "GBP";
    public static readonly string BRL = "BRL";
    public static readonly string JPY = "JPY";

    #endregion

    #region Properties

    public decimal Amount { get; }

    public string CurrencyCode { get; }

    public int DecimalPlaces { get; }

    #endregion

    #region C'tor

    private Currency(decimal amount, string currencyCode, int decimalPlaces)
    {
        Amount = amount;
        CurrencyCode = currencyCode;
        DecimalPlaces = decimalPlaces;
    }

    #endregion

    #region Factory

    public static Currency Create(decimal amount, string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(currencyCode));

        // Standardize currency code to uppercase
        currencyCode = currencyCode.Trim().ToUpperInvariant();

        // Determine decimal places based on currency
        int decimalPlaces = GetDecimalPlacesForCurrency(currencyCode);

        // Round to appropriate decimal places
        decimal roundedAmount = Math.Round(amount, decimalPlaces, MidpointRounding.AwayFromZero);

        return new Currency(roundedAmount, currencyCode, decimalPlaces);
    }

    public static Currency FromString(string value, string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(GetResourceString($"{key}ValueEmpty", CultureInfo.CurrentCulture), nameof(value));

        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(currencyCode));

        if (!decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal amount))
            throw new ArgumentException(GetResourceString($"{key}InvalidValue", CultureInfo.CurrentCulture), nameof(value));

        return Create(amount, currencyCode);
    }

    private static int GetDecimalPlacesForCurrency(string currencyCode)
    {
        return currencyCode switch
        {
            "JPY" => 0,      // Japanese Yen has no decimal places
            "KRW" => 0,      // Korean Won has no decimal places
            "HUF" => 0,      // Hungarian Forint has no decimal places
            "CLP" => 0,      // Chilean Peso has no decimal places
            _ => 2           // Default for most currencies (USD, EUR, GBP, etc.)
        };
    }

    #endregion

    #region Operations

    public Currency Add(Currency other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (other.CurrencyCode != CurrencyCode)
        {
            var message = GetResourceString($"{key}Add", CultureInfo.CurrentCulture);
            var and = GetResourceString($"{key}And", CultureInfo.CurrentCulture);

            throw new InvalidOperationException($"{message} {CurrencyCode} {and} {other.CurrencyCode}");
        }
            
        return new Currency(Amount + other.Amount, CurrencyCode, DecimalPlaces);
    }

    public Currency Subtract(Currency other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (other.CurrencyCode != CurrencyCode)
        {
            var message = GetResourceString($"{key}Subtract", CultureInfo.CurrentCulture);
            var and = GetResourceString($"{key}And", CultureInfo.CurrentCulture);

            throw new InvalidOperationException($"{message} {CurrencyCode} {and} {other.CurrencyCode}");
        }

        return new Currency(Amount - other.Amount, CurrencyCode, DecimalPlaces);
    }

    public Currency Multiply(decimal factor)
    {
        var result = Amount * factor;

        return new Currency(Math.Round(result, DecimalPlaces, MidpointRounding.AwayFromZero), CurrencyCode, DecimalPlaces);
    }

    public Currency Divide(decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException(GetResourceString($"{key}Divide", CultureInfo.CurrentCulture));

        var result = Amount / divisor;

        return new Currency(Math.Round(result, DecimalPlaces, MidpointRounding.AwayFromZero), CurrencyCode, DecimalPlaces);
    }

    public string Format(bool includeSymbol = true)
    {
        if (!includeSymbol)
            return Amount.ToString($"N{DecimalPlaces}", CultureInfo.InvariantCulture);

        return GetCultureInfoForCurrency(CurrencyCode) is CultureInfo cultureInfo ? string.Format(cultureInfo, "{0:C}", Amount) : $"{GetCurrencySymbol(CurrencyCode)}{Amount.ToString($"N{DecimalPlaces}", CultureInfo.InvariantCulture)}";
    }

    private static string GetCurrencySymbol(string currencyCode)
    {
        return currencyCode switch
        {
            "USD" => "$",
            "EUR" => "€",
            "GBP" => "£",
            "BRL" => "R$",
            "JPY" => "¥",
            _ => currencyCode + " "
        };
    }

    private static CultureInfo GetCultureInfoForCurrency(string currencyCode)
    {
        try
        {
            return currencyCode switch
            {
                "USD" => new CultureInfo("en-US"),
                "EUR" => new CultureInfo("fr-FR"),
                "GBP" => new CultureInfo("en-GB"),
                "BRL" => new CultureInfo("pt-BR"),
                "JPY" => new CultureInfo("ja-JP"),
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region Equals & HashCode

    public override string ToString() => Format();

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (Currency)obj;

        // Compare amounts with appropriate precision
        var amountDifference = Math.Abs(Amount - other.Amount);
        var epsilon = (decimal)Math.Pow(0.1, DecimalPlaces) / 2;

        return string.Equals(CurrencyCode, other.CurrencyCode, StringComparison.OrdinalIgnoreCase) &&
               amountDifference < epsilon;
    }

    public override int GetHashCode()
    {
        // Round to appropriate precision to ensure consistent hash codes
        return HashCode.Combine(
            Math.Round(Amount, DecimalPlaces, MidpointRounding.AwayFromZero),
            CurrencyCode?.ToUpperInvariant());
    }

    #endregion
}