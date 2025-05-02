using System.Globalization;
using Titan.Domain.Entities;
using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.Entities;

public sealed class CompanyUserTests
{
    [Fact]
    public void Constructor_WithValidParams_ShouldCreateCompanyUser()
    {
        // Arrange
        var email = Email.Create("company@example.com");
        var companyName = Name.Create("Test Company");
        var companyId = Guid.NewGuid();

        // Act
        var user = new CompanyUser(email, companyName, companyId);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(email, user.Email);
        Assert.Equal(companyName, user.Name);
        Assert.Equal(companyId, user.CompanyId);
        Assert.False(user.IsActive);
        Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 10);

        // Verify default role
        Assert.True(user.HasRole("company_user"));
        Assert.Single(user.Roles);
    }

    [Fact]
    public void Constructor_WithNullCompanyName_ShouldThrowArgumentException()
    {
        // Arrange
        var email = Email.Create("company@example.com");
        Name companyName = null;
        var companyId = Guid.NewGuid();
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new CompanyUser(email, companyName, companyId));

        Assert.Equal("Company name cannot be null or empty. (Parameter 'companyName')", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullDomain_ShouldCreateCompanyUser()
    {
        // Arrange
        var email = Email.Create("company@example.com");
        var companyName = Name.Create("Test Company");
        var companyId = Guid.NewGuid();

        // Act
        var user = new CompanyUser(email, companyName, companyId);

        // Assert
        Assert.Equal(email, user.Email);
        Assert.Equal(companyName, user.Name);
        Assert.Equal(companyId, user.CompanyId);
    }

    [Fact]
    public void IsAdministrator_WithAdminRole_ShouldReturnTrue()
    {
        // Arrange
        var email = Email.Create("company@example.com");
        var companyName = Name.Create("Test Company");
        var companyId = Guid.NewGuid();
        var user = new CompanyUser(email, companyName, companyId);
        user.AddRole(new Role("company_admin"));

        // Act
        var result = user.IsAdministrator();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAdministrator_WithoutAdminRole_ShouldReturnFalse()
    {
        // Arrange
        var email = Email.Create("company@example.com");
        var companyName = Name.Create("Test Company");
        var companyId = Guid.NewGuid();
        var user = new CompanyUser(email, companyName, companyId);

        // Act
        var result = user.IsAdministrator();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MakeAdministrator_ShouldAddAdminRole()
    {
        // Arrange
        var email = Email.Create("company@example.com");
        var companyName = Name.Create("Test Company");
        var companyId = Guid.NewGuid();
        var user = new CompanyUser(email, companyName, companyId);
        Assert.False(user.IsAdministrator());

        // Act
        user.MakeAdministrator();

        // Assert
        Assert.True(user.IsAdministrator());
        Assert.Contains(user.Roles, r => r.Name == "company_admin");
    }

    [Fact]
    public void RemoveAdministrator_ShouldRemoveAdminRole()
    {
        // Arrange
        var email = Email.Create("company@example.com");
        var companyName = Name.Create("Test Company");
        var companyId = Guid.NewGuid();
        var user = new CompanyUser(email, companyName, companyId);
        user.MakeAdministrator();
        Assert.True(user.IsAdministrator());

        // Act
        user.RemoveAdministrator();

        // Assert
        Assert.False(user.IsAdministrator());
        Assert.DoesNotContain(user.Roles, r => r.Name == "company_admin");
    }
}