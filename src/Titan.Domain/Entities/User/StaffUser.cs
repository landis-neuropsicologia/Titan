using Titan.Domain.ValueObjects;

namespace Titan.Domain.Entities.User;

/// <summary>
/// Usuário Interno. Vinculado à Reliance.
/// </summary>
public sealed class StaffUser : UserBase
{
    private static readonly string key = "StaffUser_";

    #region Properties

    public EmployeeId EmployeeId { get; private set; }

    public string Department { get; private set; }

    #endregion

    #region C'tor

    private StaffUser() : base() { }

    public StaffUser(Email email, Name name, EmployeeId employeeId, string department) : base(email, name)
    {
        if (string.IsNullOrEmpty(employeeId))
            throw new ArgumentException(GetResourceString($"{key}EmployeeIdEmpty", CultureInfo.CurrentCulture), nameof(employeeId));

        EmployeeId = employeeId;
        Department = department;

        AddRole(new Role("staff"));
    }

    #endregion

    #region Behaviors

    public bool IsFullAccess()
    {
        return HasRole("full_access");
    }

    public void GrantFullAccess()
    {
        AddRole(new Role("full_access"));
    }

    public void RevokeFullAccess()
    {
        RemoveRole("full_access");
    }

    #endregion
}
