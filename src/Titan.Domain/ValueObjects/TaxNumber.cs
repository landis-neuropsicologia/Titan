namespace Titan.Domain.ValueObjects;

public sealed class TaxNumber : ValueObject
{
    private static readonly string key = "TaxNumber_";

    #region Properties

    public string Value { get; }

    #endregion

    #region C'tor

    private TaxNumber(string value)
    {
        Value = value;
    }

    #endregion

    #region Factory

    public static TaxNumber Create(string taxNumber)
    {
        if (string.IsNullOrWhiteSpace(taxNumber))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(taxNumber));

        taxNumber = taxNumber.Trim();

        return new TaxNumber(taxNumber);
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(TaxNumber taxNumber) => taxNumber?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (TaxNumber)obj;

        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Value.ToLowerInvariant().GetHashCode();
    }

    #endregion
}
