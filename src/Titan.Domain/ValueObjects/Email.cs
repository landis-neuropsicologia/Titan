namespace Titan.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    private static readonly string key = "Email_";
    private static readonly Regex EmailRegex = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"); //new(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled);

    #region Properties

    public string Value { get; }

    #endregion

    #region C'tor

    private Email(string value)
    {
        Value = value;
    }

    #endregion

    #region Factory

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(email));

        email = email.Trim();

        if (email.Length > 256)
            throw new ArgumentException(GetResourceString($"{key}TooLong", CultureInfo.CurrentCulture), nameof(email));

        if (!EmailRegex.IsMatch(email))
            throw new ArgumentException(GetResourceString($"{key}Invalid", CultureInfo.CurrentCulture), nameof(email));

        return new Email(email);
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(Email email) => email?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (Email)obj;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Value.ToLowerInvariant().GetHashCode();
    }

    #endregion
}
