using System.Globalization;
using Titan.Domain.Entities;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.Entities;

public sealed class CompanyTests
{
    [Fact]
    public void Constructor_WithValidParams_ShouldCreateCompany()
    {
        // Arrange
        var name = Name.Create("Test Company LTDA");
        var taxNumber = TaxNumber.Create("1234567890");
        var commercialName = Name.Create("Test Company");
        var domain = DomainName.Create("testcompany.com");

        // Act
        var company = new Company(name, taxNumber, commercialName, domain);

        // Assert
        Assert.NotEqual(Guid.Empty, company.Id);
        Assert.Equal(name, company.Name);
        Assert.Equal(domain, company.Domain);
        Assert.True((DateTime.UtcNow - company.CreatedAt).TotalSeconds < 10);
        Assert.Empty(company.Users);
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Company(null, null));
        Assert.Equal("Company name cannot be null or empty. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullDomain_ShouldCreateCompany()
    {
        // Arrange
        var name = Name.Create("Test Company");
        var taxNumber = TaxNumber.Create("1234567890");
        var commercialName = Name.Create("Test Company");

        // Act
        var company = new Company(name, taxNumber, commercialName);

        // Assert
        Assert.NotEqual(Guid.Empty, company.Id);
        Assert.Equal(name, company.Name);
        Assert.Null(company.Domain);
    }

    [Fact]
    public void UpdateInfo_WithValidParams_ShouldUpdateCompanyInfo()
    {
        // Arrange
        var name = Name.Create("Test Company");
        var taxNumber = TaxNumber.Create("1234567890");
        var domain = DomainName.Create("testcompany.com");
        var company = new Company(name, taxNumber, null, domain);

        var newName = Name.Create("Updated Company");
        var newTaxNumber = TaxNumber.Create("1234567890");
        var newDomain = DomainName.Create("updated-company.com");

        // Act
        company.UpdateInfo(newName, newTaxNumber, null, newDomain);

        // Assert
        Assert.Equal(newName, company.Name);
        Assert.Equal(newDomain, company.Domain);
    }

    [Fact]
    public void UpdateInfo_WithNullName_ShouldNotUpdateName()
    {
        // Arrange
        var name = Name.Create("Test Company");
        var taxNumber = TaxNumber.Create("1234567890");
        var domain = DomainName.Create("testcompany.com");
        var company = new Company(name, taxNumber, null, domain);
        var newDomain = DomainName.Create("updated-company.com");

        // Act
        company.UpdateInfo(null, taxNumber, null, newDomain);

        // Assert
        Assert.Equal(name, company.Name);
        Assert.Equal(newDomain, company.Domain);
    }

    [Fact]
    public void UpdateInfo_WithNullDomain_ShouldUpdateDomainToNull()
    {
        // Arrange
        var name = Name.Create("Test Company");
        var taxNumber = TaxNumber.Create("1234567890");
        var domain = DomainName.Create("testcompany.com");
        var company = new Company(name, taxNumber, null, domain);

        // Act
        company.UpdateInfo(name, null);

        // Assert
        Assert.Equal(name, company.Name);
        Assert.Null(company.Domain);
    }
}