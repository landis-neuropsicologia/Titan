namespace Titan.Application.DTOs.Company;

public sealed record CreateCompanyRequest(string CompanyName, string CommercialName, string TaxNumber, string DomainName, string ContactName, string ContactEmail);