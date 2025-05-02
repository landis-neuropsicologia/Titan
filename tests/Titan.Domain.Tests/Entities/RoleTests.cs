using System.Globalization;
using Titan.Domain.Entities;

namespace Titan.Domain.Tests.Entities;

public sealed class RoleTests
{
    [Fact]
    public void Constructor_WithValidParams_ShouldCreateRole()
    {
        // Arrange
        const string name = "admin";
        const string description = "Administrator role";

        // Act
        var role = new Role(name, description);

        // Assert
        Assert.Equal(name, role.Name);
        Assert.Equal(description, role.Description);
        Assert.Empty(role.Users);
    }

    [Fact]
    public void Constructor_WithoutDescription_ShouldCreateRoleWithNullDescription()
    {
        // Arrange
        const string name = "user";

        // Act
        var role = new Role(name);

        // Assert
        Assert.Equal(name, role.Name);
        Assert.Null(role.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Role(invalidName));
        Assert.Equal("Role name cannot be null or empty. (Parameter 'name')", exception.Message);
    }
}