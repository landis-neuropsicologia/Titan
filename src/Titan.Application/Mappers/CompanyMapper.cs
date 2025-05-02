using Titan.Application.DTOs.Company;
using Titan.Domain.Entities;
using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Application.Mappers;

internal static class CompanyMapper
{
    internal static Company MapTo(this CreateCompanyRequest request)
    {
        var name = Name.Create(request.CompanyName);
        var taxNumber = TaxNumber.Create(request.TaxNumber);
        var commercialName = Name.Create(request.CommercialName);
        var domain = DomainName.Create(request.DomainName);

        return new Company(name, taxNumber, commercialName, domain);
    }
}

internal static class CompanyUserMapper
{
    internal static CompanyUser MapTo(this CreateCompanyRequest request, Guid companyId)
    {
        var email = Email.Create(request.ContactEmail);
        var name = Name.Create(request.ContactName);
        return new CompanyUser(email, name, companyId);
    }
}