namespace Titan.Domain.ValueObjects;

public sealed class Name : ValueObject
{
    private static readonly string key = "Name_";

    #region Properties

    public string Value { get; }

    #endregion

    #region C'tor

    private Name(string value)
    {
        Value = value;
    }

    #endregion

    #region Factory

    public static Name Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(name));

        name = name.Trim();

        if (name.Length > 256)
            throw new ArgumentException(GetResourceString($"{key}TooLong", CultureInfo.CurrentCulture), nameof(name));

        return new Name(name);
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(Name name) => name?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (Name)obj;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Value.ToLowerInvariant().GetHashCode();
    }

    #endregion
}