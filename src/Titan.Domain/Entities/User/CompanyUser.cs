using Titan.Domain.ValueObjects;

namespace Titan.Domain.Entities.User;

/// <summary>
/// Usuário Pessoa Jurídica. É vinculado a uma empresa.
/// </summary>
public sealed class CompanyUser : UserBase
{
    #region Properties

    public Guid CompanyId { get; private set; }

    #endregion

    #region C'tor

    private CompanyUser() : base() { }

    public CompanyUser(Email email, Name name, Guid companyId) : base(email, name)
    {
        CompanyId = companyId;

        AddRole(new Role("company_user"));
    }

    #endregion

    #region Behaviors

    public bool IsAdministrator()
    {
        return HasRole("company_admin");
    }

    public void MakeAdministrator()
    {
        AddRole(new Role("company_admin"));
    }

    public void RemoveAdministrator()
    {
        RemoveRole("company_admin");
    }

    #endregion
}
