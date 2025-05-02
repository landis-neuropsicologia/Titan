using System.Globalization;
using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.Entities;

public sealed class StaffUserTests
{
    [Fact]
    public void Constructor_WithValidParams_ShouldCreateStaffUser()
    {
        // Arrange
        var email = Email.Create("staff@example.com");
        var name = Name.Create("John Staff");
        var employeeId = EmployeeId.Create("EMP12345");
        const string department = "IT";

        // Act
        var user = new StaffUser(email, name, employeeId, department);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(email, user.Email);
        Assert.Equal(name, user.Name);
        Assert.Equal(employeeId, user.EmployeeId);
        Assert.Equal(department, user.Department);
        Assert.False(user.IsActive);
        Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 10);

        // Verify default role
        Assert.True(user.HasRole("staff"));
        Assert.Single(user.Roles);
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentException()
    {
        // Arrange
        var email = Email.Create("staff@example.com");
        Name name = null;
        var employeeId = EmployeeId.Create("EMP12345");
        const string department = "IT";
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new StaffUser(email, name, employeeId, department));

        Assert.Equal("Name cannot be null or empty. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullEmployeeId_ShouldThrowArgumentException()
    {
        // Arrange
        const string department = "IT";
        var email = Email.Create("staff@example.com");
        var name = Name.Create("John Staff");
        EmployeeId employeeId = null;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new StaffUser(email, name, employeeId, department));
        
        Assert.Equal("Employee ID cannot be null or empty. (Parameter 'employeeId')", exception.Message);
    }

    [Fact]
    public void IsFullAccess_WithFullAccessRole_ShouldReturnTrue()
    {
        // Arrange
        var email = Email.Create("staff@example.com");
        var name = Name.Create("John Staff");
        var employeeId = EmployeeId.Create("EMP12345");
        const string department = "IT";
        var user = new StaffUser(email, name, employeeId, department);
        user.GrantFullAccess();

        // Act
        var result = user.IsFullAccess();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsFullAccess_WithoutFullAccessRole_ShouldReturnFalse()
    {
        // Arrange
        var email = Email.Create("staff@example.com");
        var name = Name.Create("John Staff");
        var employeeId = EmployeeId.Create("EMP12345");
        const string department = "IT";
        var user = new StaffUser(email, name, employeeId, department);

        // Act
        var result = user.IsFullAccess();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GrantFullAccess_ShouldAddFullAccessRole()
    {
        // Arrange
        var email = Email.Create("staff@example.com");
        var name = Name.Create("John Staff");
        var employeeId = EmployeeId.Create("EMP12345");
        const string department = "IT";
        var user = new StaffUser(email, name, employeeId, department);
        Assert.False(user.IsFullAccess());

        // Act
        user.GrantFullAccess();

        // Assert
        Assert.True(user.IsFullAccess());
        Assert.Contains(user.Roles, r => r.Name == "full_access");
    }

    [Fact]
    public void RevokeFullAccess_ShouldRemoveFullAccessRole()
    {
        // Arrange
        var email = Email.Create("staff@example.com");
        var name = Name.Create("John Staff");
        var employeeId = EmployeeId.Create("EMP12345");
        const string department = "IT";
        var user = new StaffUser(email, name, employeeId, department);
        user.GrantFullAccess();
        Assert.True(user.IsFullAccess());

        // Act
        user.RevokeFullAccess();

        // Assert
        Assert.False(user.IsFullAccess());
        Assert.DoesNotContain(user.Roles, r => r.Name == "full_access");
    }
}