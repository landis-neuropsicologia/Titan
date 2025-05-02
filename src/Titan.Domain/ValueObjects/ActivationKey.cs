namespace Titan.Domain.ValueObjects;

public sealed class ActivationKey : ValueObject
{
    private static readonly string key = "ActivationKey_";

    #region Properties

    public string Value { get; }

    #endregion

    #region C'tor

    private ActivationKey(string value)
    {
        Value = value;
    }

    #endregion

    #region Factory

    public static ActivationKey Generate()
    {
        var key = Guid.NewGuid().ToString("N");

        return new ActivationKey(key);
    }

    public static ActivationKey Create(string keyCode)
    {
        if (string.IsNullOrWhiteSpace(keyCode))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(keyCode));

        return new ActivationKey(keyCode);
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(ActivationKey key) => key?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (ActivationKey)obj;
        return string.Equals(Value, other.Value, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    #endregion
}
