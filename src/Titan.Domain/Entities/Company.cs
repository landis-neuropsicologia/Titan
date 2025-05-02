using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Entities;

public sealed class Company : Entity<Guid>
{
    private static readonly string key = "Company_";

    #region Properties

    public Name Name { get; private set; } 

    public Name CommercialName { get; private set; }

    public TaxNumber TaxNumber { get; private set; }

    public DomainName Domain { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public bool IsActive { get; protected set; }

    public ActivationKey ActivationKey { get; private set; }


    public ICollection<CompanyUser> Users { get; private set; }

    #endregion

    #region C'tor

    private Company()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Users = [];
        IsActive = false;
    }

    public Company(Name name, TaxNumber taxNumber, Name commercialName = null,  DomainName domain = null) : this()
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(name));

        if (string.IsNullOrEmpty(taxNumber))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(taxNumber));

        ActivationKey = ActivationKey.Generate();
        Name = name;
        CommercialName = commercialName;
        TaxNumber = taxNumber;
        Domain = domain;
    }

    #endregion

    #region Behaviors

    public void UpdateInfo(Name name, TaxNumber taxNumber, Name commercialName = null, DomainName domain = null)
    {
        if (name is not null)
            Name = name;

        if (taxNumber is not null)
            TaxNumber = taxNumber;

        CommercialName = commercialName;
        Domain = domain;
    }

    #endregion
}
