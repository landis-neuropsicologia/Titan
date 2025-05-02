namespace Titan.Domain.ValueObjects;

public sealed class DomainName : ValueObject
{
    private static readonly string key = "DomainName_";

    #region Properties

    public string Value { get; }

    #endregion

    #region C'tor

    private DomainName(string value)
    {
        Value = value;
    }

    #endregion

    #region Factory

    public static DomainName Create(string domain)
    {
        if (string.IsNullOrWhiteSpace(domain)) return null; 

        domain = domain.Trim().ToLowerInvariant();

        // Remover protocolo, se presente
        if (domain.StartsWith("http://")) domain = domain.Substring(7);
        else if (domain.StartsWith("https://")) domain = domain.Substring(8);

        // Remover www, se presente
        if (domain.StartsWith("www.")) domain = domain.Substring(4);

        // Remover path, se presente
        var pathIndex = domain.IndexOf('/');

        if (pathIndex >= 0) domain = domain.Substring(0, pathIndex);

        if (domain.Length > 256) throw new ArgumentException(GetResourceString($"{key}TooLong", CultureInfo.CurrentCulture), nameof(domain));

        // Validação básica de domínio
        if (!domain.Contains('.')) throw new ArgumentException(GetResourceString($"{key}EmptyPeriod", CultureInfo.CurrentCulture), nameof(domain));

        return new DomainName(domain);
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(DomainName domain) => domain?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (DomainName)obj;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Value.ToLowerInvariant().GetHashCode();
    }

    #endregion
}

