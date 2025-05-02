namespace Titan.Domain.ValueObjects;

public sealed class PasswordHash : ValueObject
{
    private static readonly string key = "PasswordHash_";

    #region Properties

    public string Value { get; }

    #endregion

    #region C'tor

    private PasswordHash(string value)
    {
        Value = value;
    }

    #endregion

    #region Factory

    /// <summary>
    /// Creates a new PasswordHash by hashing the provided plain text password.
    /// </summary>
    public static PasswordHash Create(string plainTextPassword, string salt = null)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(plainTextPassword));

        // Use a default salt if none provided
        salt ??= "Titan_DefaultSalt";

        // Combine password and salt
        var combinedString = $"{plainTextPassword}{salt}";

        // Create hash
        var bytes = Encoding.UTF8.GetBytes(combinedString);
        var hashBytes = SHA256.HashData(bytes);
        var hash = Convert.ToBase64String(hashBytes);

        return new PasswordHash(hash);
    }

    /// <summary>
    /// Creates a PasswordHash from an existing hash value.
    /// </summary>
    public static PasswordHash FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException(GetResourceString($"{key}HashEmpty", CultureInfo.CurrentCulture), nameof(hash));

        return new PasswordHash(hash);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Verifies if a plain text password matches this hash.
    /// </summary>
    public bool Verify(string plainTextPassword, string salt = null)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            return false;

        var passwordHash = Create(plainTextPassword, salt);
        return string.Equals(Value, passwordHash.Value, StringComparison.Ordinal);
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(PasswordHash passwordHash) => passwordHash?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (PasswordHash)obj;
        return string.Equals(Value, other.Value, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    #endregion
}