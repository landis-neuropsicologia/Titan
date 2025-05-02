namespace Titan.Domain.ValueObjects;

public sealed class UserStatus : ValueObject
{
    private static readonly string key = "UserStatus_";

    #region Status Types

    public static readonly UserStatus Pending = new(StatusType.Pending, null);
    public static readonly UserStatus Active = new(StatusType.Active, null);
    public static readonly UserStatus Inactive = new(StatusType.Inactive, null);
    public static readonly UserStatus Suspended = new(StatusType.Suspended, null);
    public static readonly UserStatus Locked = new(StatusType.Locked, null);
    public static readonly UserStatus Deleted = new(StatusType.Deleted, null);

    public enum StatusType
    {
        Pending,    // Account created but not activated
        Active,     // Account active and can use the system
        Inactive,   // Account inactive (not logged in for a period)
        Suspended,  // Account temporarily suspended (e.g., by admin)
        Locked,     // Account locked (e.g., too many login attempts)
        Deleted     // Account has been deleted (soft delete)
    }

    #endregion

    #region Properties

    public StatusType Type { get; }

    public string Reason { get; }

    public DateTime CreatedAt { get; }

    public DateTime? ExpiresAt { get; }

    #endregion

    #region C'tor

    private UserStatus(StatusType type, string reason = null, DateTime? expiresAt = null)
    {    
        Type = type;
        Reason = reason;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
    }

    #endregion

    #region Factory

    public static UserStatus Create(StatusType type, string reason = null, DateTime? expiresAt = null)
    {
        if (reason == null && expiresAt == null)
        {
            return type switch
            {
                StatusType.Pending => Pending,
                StatusType.Active => Active,
                StatusType.Inactive => Inactive,
                StatusType.Suspended => Suspended,
                StatusType.Locked => Locked,
                StatusType.Deleted => Deleted,
                _ => throw new ArgumentException($"{GetResourceString($"{key}Unknown", CultureInfo.CurrentCulture)} {type}", nameof(type))
            };
        }

        return new UserStatus(type, reason, expiresAt);
    }

    public static UserStatus CreateSuspended(string reason, DateTime? expiresAt = null)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException(GetResourceString($"{key}CreateSuspended", CultureInfo.CurrentCulture), nameof(reason));

        return new UserStatus(StatusType.Suspended, reason, expiresAt);
    }

    public static UserStatus CreateLocked(string reason, DateTime? expiresAt = null)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException(GetResourceString($"{key}CreateLocked", CultureInfo.CurrentCulture), nameof(reason));

        return new UserStatus(StatusType.Locked, reason, expiresAt);
    }

    public static UserStatus CreateDeleted(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException(GetResourceString($"{key}CreateDeleted", CultureInfo.CurrentCulture), nameof(reason));
        
        return new UserStatus(StatusType.Deleted, reason);
    }

    #endregion

    #region Methods

    public bool IsActive()
    {
        return Type == StatusType.Active;
    }

    public bool IsPending()
    {
        return Type == StatusType.Pending;
    }

    public bool IsInactive()
    {
        return Type == StatusType.Inactive;
    }

    public bool IsSuspended()
    {
        return Type == StatusType.Suspended;
    }

    public bool IsLocked()
    {
        return Type == StatusType.Locked;
    }

    public bool IsDeleted()
    {
        return Type == StatusType.Deleted;
    }

    public bool IsExpired()
    {
        return ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
    }

    public bool IsActiveOrPending()
    {
        return Type == StatusType.Active || Type == StatusType.Pending;
    }

    /// <summary>
    /// User can login if active and not expired.
    /// </summary>
    /// <returns><![CDATA[true]]> or <![CDATA[false]]></returns>
    public bool CanLogin()
    {
        return Type == StatusType.Active && !IsExpired();
    }

    /// <summary>
    /// Only pending accounts can be activated.
    /// </summary>
    /// <returns><![CDATA[true]]> or <![CDATA[false]]></returns>
    public bool CanBeActivated()
    {
        return Type == StatusType.Pending;
    }

    /// <summary>
    /// Only locked accounts can be unlocked.
    /// </summary>
    /// <returns><![CDATA[true]]> or <![CDATA[false]]></returns>
    public bool CanBeUnlocked()
    {
        return Type == StatusType.Locked;
    }

    /// <summary>
    /// Only active or pending accounts can be suspended.
    /// </summary>
    /// <returns><![CDATA[true]]> or <![CDATA[false]]></returns>
    public bool CanBeSuspended()
    {
        return Type == StatusType.Active || Type == StatusType.Pending;
    }

    /// <summary>
    /// Add or Update Reason.
    /// </summary>
    /// <returns><![CDATA[UserStatus]]></returns>
    /// <exception cref="ArgumentException"></exception>
    public UserStatus WithReason(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException(GetResourceString($"{key}WithReason", CultureInfo.CurrentCulture), nameof(reason));

        return new UserStatus(Type, reason, ExpiresAt);
    }

    /// <summary>
    /// Sets the expiration date.
    /// </summary>
    /// <returns><![CDATA[UserStatus]]></returns>
    /// <exception cref="ArgumentException"></exception>
    public UserStatus WithExpiration(DateTime expiresAt)
    {
        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException(GetResourceString($"{key}WithExpiration", CultureInfo.CurrentCulture), nameof(expiresAt));

        return new UserStatus(Type, Reason, expiresAt);
    }

    /// <summary>
    /// Removes the expiration date.
    /// </summary>
    /// <returns><![CDATA[UserStatus]]></returns>
    public UserStatus WithoutExpiration()
    {
        return new UserStatus(Type, Reason, null);
    }

    /// <summary>
    /// Get the status description.
    /// </summary>
    /// <returns><![CDATA[string]]></returns>
    public string GetDescription(CultureInfo culture = null)
    {
        culture = culture ?? CultureInfo.CurrentCulture;

        string statusName = GetLocalizedStatusName(Type, culture);
        string description = statusName;

        if (!string.IsNullOrWhiteSpace(Reason))
            description += $" ({Reason})";

        if (ExpiresAt.HasValue)
        {
            var expirationText = ExpiresAt.Value < DateTime.UtcNow ? GetResourceString("UserStatus_Expired", culture) : $"{GetResourceString("UserStatus_ExpiresAt", culture)} {ExpiresAt.Value.ToString("g", culture)}";

            description += $" - {expirationText}";
        }

        return description;
    }

    #endregion

    #region Private Methods

    private static string GetLocalizedStatusName(StatusType type, CultureInfo culture)
    {
        string value = string.Empty;

        switch (type)
        {
            case StatusType.Pending:
                value = $"{key}{nameof(StatusType.Pending)}";
                break;

            case StatusType.Active:
                value = $"{key}{nameof(StatusType.Active)}";
                break;

            case StatusType.Inactive:
                value = $"{key}{nameof(StatusType.Inactive)}";
                break;

            case StatusType.Suspended:
                value = $"{key}{nameof(StatusType.Suspended)}";
                break;

            case StatusType.Locked:
                value = $"{key}{nameof(StatusType.Locked)}";
                break;

            case StatusType.Deleted:
                value = $"{key}{nameof(StatusType.Deleted)}";
                break;
            default:
                break;
        }

        return GetResourceString(value, culture);
    }

    #endregion

    #region Equals & HashCode

    public override string ToString() => GetDescription();

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        
        if (ReferenceEquals(this, obj)) return true;
        
        if (obj.GetType() != GetType()) return false;

        var other = (UserStatus)obj;

        return Type == other.Type && string.Equals(Reason, other.Reason, StringComparison.OrdinalIgnoreCase) &&
               ((!ExpiresAt.HasValue && !other.ExpiresAt.HasValue) || (ExpiresAt.HasValue && other.ExpiresAt.HasValue && ExpiresAt.Value.Equals(other.ExpiresAt.Value)));
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;

            hash = hash * 23 + Type.GetHashCode();
            hash = hash * 23 + (Reason?.ToUpperInvariant().GetHashCode() ?? 0);
            hash = hash * 23 + (ExpiresAt?.GetHashCode() ?? 0);
            
            return hash;
        }
    }

    #endregion
}