namespace Titan.Domain.ValueObjects;

public sealed class SocialMediaProvider : ValueObject
{
    private static string key = "SocialMediaProvider_";

    public static readonly SocialMediaProvider Google = new("Google");
    public static readonly SocialMediaProvider Facebook = new("Facebook");
    public static readonly SocialMediaProvider Microsoft = new("Microsoft");

    #region Properties

    public string Value { get; }

    #endregion

    #region C'tor

    private SocialMediaProvider(string value)
    {
        Value = value;
    }

    #endregion

    #region Factory

    public static SocialMediaProvider Create(string provider)
    {
        if (string.IsNullOrWhiteSpace(provider))
            return null;

        provider = provider.Trim();

        return provider.ToLowerInvariant() switch
        {
            "google" => Google,
            "facebook" => Facebook,
            "microsoft" => Microsoft,
            _ => throw new ArgumentException($"{ GetResourceString($"{key}UnsupportedProvider", CultureInfo.CurrentCulture)} {provider}", nameof(provider))
        };
    }

    public string GetLocalizedName(CultureInfo culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        return Value switch
        {
            "Google" => GetResourceString($"{key}Google", culture),
            "Facebook" => GetResourceString($"{key}Facebook", culture),
            "Microsoft" => GetResourceString($"{key}Microsoft", culture),
            _ => Value
        };
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(SocialMediaProvider provider) => provider?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (SocialMediaProvider)obj;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Value.ToLowerInvariant().GetHashCode();
    }

    #endregion
}

