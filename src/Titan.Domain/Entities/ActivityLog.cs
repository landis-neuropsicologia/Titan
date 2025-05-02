namespace Titan.Domain.Entities;

public sealed class ActivityLog : Entity<long>
{
    private static readonly string key = "ActivityLog_";

    #region Properties
    
    public Guid UserId { get; private set; }
    
    public string Action { get; private set; } = string.Empty;
    
    public string Description { get; private set; } = string.Empty;
    
    public DateTime Timestamp { get; private set; }
    
    public string IpAddress { get; private set; } = string.Empty;

    #endregion

    #region C'tor

    private ActivityLog()
    {
        Timestamp = DateTime.UtcNow;
    }

    public ActivityLog(Guid userId, string action, string description = null, string ipAddress = null) : this()
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(GetResourceString($"{key}UserIdEmpty", CultureInfo.CurrentCulture), nameof(userId));

        if (string.IsNullOrEmpty(action))
            throw new ArgumentException(GetResourceString($"{key}ActionEmpty", CultureInfo.CurrentCulture), nameof(action));

        UserId = userId;
        Action = action;
        Description = description;
        IpAddress = ipAddress;
    }

    #endregion
}