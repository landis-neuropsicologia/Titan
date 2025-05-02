namespace Titan.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    private static readonly string key = "Address_";

    #region Properties

    public string Street { get; }

    public string Number { get; }

    public string Complement { get; }

    public string Neighborhood { get; }

    public string City { get; }

    public string State { get; }

    public string Country { get; }

    public string ZipCode { get; }

    #endregion

    #region C'tor

    private Address(string street, string number, string complement, string neighborhood, string city, string state, string country, string zipCode)
    {
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    #endregion

    #region Factory

    public static Address Create(string street, string number, string city, string state, string country, string zipCode, string neighborhood = null, string complement = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException(GetResourceString($"{key}StreetEmpty", CultureInfo.CurrentCulture), nameof(street));

        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException(GetResourceString($"{key}NumberEmpty", CultureInfo.CurrentCulture), nameof(number));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException(GetResourceString($"{key}CityEmpty", CultureInfo.CurrentCulture), nameof(city));

        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException(GetResourceString($"{key}StateEmpty", CultureInfo.CurrentCulture), nameof(state));

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException(GetResourceString($"{key}CountryEmpty", CultureInfo.CurrentCulture), nameof(country));

        if (string.IsNullOrWhiteSpace(zipCode))
            throw new ArgumentException(GetResourceString($"{key}ZipEmpty", CultureInfo.CurrentCulture), nameof(zipCode));

        return new Address(street.Trim(), number.Trim(), complement?.Trim(), neighborhood?.Trim(), city.Trim(), state.Trim(), country.Trim(), NormalizeZipCode(zipCode));
    }

    private static string NormalizeZipCode(string zipCode)
    {
        // Remove any non-alphanumeric character
        return Regex.Replace(zipCode.Trim(), @"[^a-zA-Z0-9]", "");
    }

    #endregion

    #region Methods

    public string ToFormattedString()
    {
        var sb = new StringBuilder();

        sb.Append($"{Street}, {Number}");

        if (!string.IsNullOrWhiteSpace(Complement))
            sb.Append($" - {Complement}");

        if (!string.IsNullOrWhiteSpace(Neighborhood))
            sb.Append($", {Neighborhood}");

        sb.Append($", {City}");
        sb.Append($", {State}");
        sb.Append($", {Country}");
        sb.Append($" - {FormatZipCode(ZipCode, Country)}");

        return sb.ToString();
    }

    public string ToSingleLineString()
    {
        return ToFormattedString();
    }

    public string ToMultiLineString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{Street}, {Number}");

        if (!string.IsNullOrWhiteSpace(Complement))
            sb.AppendLine(Complement);

        if (!string.IsNullOrWhiteSpace(Neighborhood))
            sb.AppendLine(Neighborhood);

        sb.AppendLine($"{City}, {State}");
        sb.AppendLine(Country);
        sb.Append(FormatZipCode(ZipCode, Country));

        return sb.ToString();
    }

    private string FormatZipCode(string zipCode, string country)
    {
        // Add country-specific zip code formatting
        // This is a simple implementation that could be expanded for specific countries
        switch (country.ToUpperInvariant())
        {
            case "USA":
            case "UNITED STATES":
            case "UNITED STATES OF AMERICA":
                // Format as "12345" or "12345-6789"
                if (zipCode.Length == 5)
                    return zipCode;
                else if (zipCode.Length == 9)
                    return $"{zipCode.Substring(0, 5)}-{zipCode.Substring(5, 4)}";
                break;

            case "BRAZIL":
            case "BRASIL":
                // Format as "12345-678"
                if (zipCode.Length == 8)
                    return $"{zipCode.Substring(0, 5)}-{zipCode.Substring(5, 3)}";
                break;
        }

        // Default: return as is
        return zipCode;
    }

    #endregion

    #region Equals & HashCode

    public override string ToString() => ToFormattedString();

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (Address)obj;

        return string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Number, other.Number, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Complement, other.Complement, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Neighborhood, other.Neighborhood, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(State, other.State, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(ZipCode, other.ZipCode, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Street?.ToUpperInvariant());
        hashCode.Add(Number?.ToUpperInvariant());
        hashCode.Add(Complement?.ToUpperInvariant());
        hashCode.Add(Neighborhood?.ToUpperInvariant());
        hashCode.Add(City?.ToUpperInvariant());
        hashCode.Add(State?.ToUpperInvariant());
        hashCode.Add(Country?.ToUpperInvariant());
        hashCode.Add(ZipCode?.ToUpperInvariant());
        return hashCode.ToHashCode();
    }

    #endregion
}