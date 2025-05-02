using Titan.Domain.ValueObjects;

namespace Titan.Domain.Entities.User;

/// <summary>
/// Classe abstrata que serve de herança para as classes StaffUser, PersonUser e CompanyUser
/// </summary>
public abstract class UserBase : Entity<Guid>
{
    private static readonly string key = "UserBase_";

    #region Properties

    public Name Name { get; protected set; }

    public Email Email { get; protected set; }

    public bool IsActive { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    public DateTime? LastLogin { get; protected set; }

    public ICollection<Role> Roles { get; protected set; }

    #endregion

    #region C'tor

    protected UserBase()
    {
        Id = Guid.NewGuid();
        IsActive = false;
        CreatedAt = DateTime.UtcNow;
        Roles = [];
    }

    protected UserBase(Email email, Name name) : this()
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException(GetResourceString($"{key}EmailEmpty", CultureInfo.CurrentCulture), nameof(email));

        if (string.IsNullOrEmpty(name))
            throw new ArgumentException(GetResourceString($"{key}NameEmpty", CultureInfo.CurrentCulture), nameof(name));

        Email = email;
        Name = name;
    }

    #endregion

    #region Behaviors

    public void RecordLogin()
    {
        LastLogin = DateTime.UtcNow;
    }

    public bool HasRole(string roleName)
    {
        return Roles.Any(r => r.Name == roleName);
    }

    public void AddRole(Role role)
    {
        if (role is null)
            throw new ArgumentException(GetResourceString($"{key}RoleEmpty", CultureInfo.CurrentCulture), nameof(role));

        if (!Roles.Any(r => r.Name == role.Name))
        {
            Roles.Add(role);
        }
    }

    public void RemoveRole(string roleName)
    {
        var role = Roles.FirstOrDefault(r => r.Name == roleName);
        
        if (role != null)
        {
            Roles.Remove(role);
        }
    }

    #endregion
}
