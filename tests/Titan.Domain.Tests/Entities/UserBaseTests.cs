using System.Globalization;
using Titan.Domain.Entities;
using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.Entities;

public sealed class UserBaseTests
{
    private class TestUser : UserBase
    {
        public TestUser() : base() { }

        public TestUser(Email email, Name name) : base(email, name) { }
    }

    [Fact]
    public void Constructor_WithoutEmail_ShouldInitializeProperties()
    {
        // Act
        var user = new TestUser();

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.False(user.IsActive);
        Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 10);
        Assert.Null(user.LastLogin);
        Assert.Empty(user.Roles);
    }

    [Fact]
    public void Constructor_WithEmail_ShouldInitializeProperties()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var name = Name.Create("User Name");

        // Act
        var user = new TestUser(email, name);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(email, user.Email);
        Assert.False(user.IsActive);
        Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 10);
        Assert.Null(user.LastLogin);
        Assert.Empty(user.Roles);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        if (invalidEmail == null)
        {
            var exception = Assert.Throws<ArgumentException>(() => new TestUser(null, null));

            Assert.Equal("Email cannot be null or empty. (Parameter 'email')", exception.Message);
        }
        else
        {
            var exception = Assert.Throws<ArgumentException>(() => new TestUser(Email.Create(invalidEmail), null));

            Assert.Equal("Email cannot be null or empty. (Parameter 'email')", exception.Message);
        }
    }

    [Fact]
    public void RecordLogin_ShouldUpdateLastLogin()
    {
        // Arrange
        var user = new TestUser();
        Assert.Null(user.LastLogin);

        // Act
        user.RecordLogin();

        // Assert
        Assert.NotNull(user.LastLogin);
        Assert.True((DateTime.UtcNow - user.LastLogin.Value).TotalSeconds < 10);
    }

    [Fact]
    public void HasRole_WithExistingRole_ShouldReturnTrue()
    {
        // Arrange
        var user = new TestUser();
        const string roleName = "test_role";
        user.AddRole(new Role(roleName));

        // Act
        var result = user.HasRole(roleName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasRole_WithNonExistingRole_ShouldReturnFalse()
    {
        // Arrange
        var user = new TestUser();
        const string roleName = "test_role";

        // Act
        var result = user.HasRole(roleName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddRole_WithNewRole_ShouldAddRoleToCollection()
    {
        // Arrange
        var user = new TestUser();
        const string roleName = "test_role";
        var role = new Role(roleName);

        // Act
        user.AddRole(role);

        // Assert
        Assert.Single(user.Roles);
        Assert.Equal(roleName, user.Roles.First().Name);
    }

    [Fact]
    public void AddRole_WithDuplicateRole_ShouldNotAddDuplicate()
    {
        // Arrange
        var user = new TestUser();
        const string roleName = "test_role";
        var role1 = new Role(roleName);
        var role2 = new Role(roleName);

        // Act
        user.AddRole(role1);
        user.AddRole(role2);

        // Assert
        Assert.Single(user.Roles);
    }

    [Fact]
    public void AddRole_WithNullRole_ShouldThrowArgumentNullException()
    {
        // Arrange
        var user = new TestUser();
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => user.AddRole(null));

        Assert.Equal("Role cannot be null or empty. (Parameter 'role')", exception.Message);
    }

    [Fact]
    public void RemoveRole_WithExistingRole_ShouldRemoveRole()
    {
        // Arrange
        var user = new TestUser();
        const string roleName = "test_role";
        user.AddRole(new Role(roleName));
        Assert.Single(user.Roles);

        // Act
        user.RemoveRole(roleName);

        // Assert
        Assert.Empty(user.Roles);
    }

    [Fact]
    public void RemoveRole_WithNonExistingRole_ShouldNotModifyRoles()
    {
        // Arrange
        var user = new TestUser();
        const string existingRoleName = "existing_role";
        const string nonExistingRoleName = "non_existing_role";
        user.AddRole(new Role(existingRoleName));
        Assert.Single(user.Roles);

        // Act
        user.RemoveRole(nonExistingRoleName);

        // Assert
        Assert.Single(user.Roles);
        Assert.Equal(existingRoleName, user.Roles.First().Name);
    }
}