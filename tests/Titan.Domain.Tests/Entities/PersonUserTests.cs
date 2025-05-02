using System.Globalization;
using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.Entities;

public sealed class PersonUserTests
{
    [Fact]
    public void Constructor_WithValidParams_ShouldCreatePersonUser()
    {
        // Arrange
        var email = Email.Create("person@example.com");
        var name = Name.Create("John Doe");

        // Act
        var user = new PersonUser(email, name);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(email, user.Email);
        Assert.Equal(name, user.Name);
        Assert.False(user.RegisteredViaSocialMedia);
        Assert.Null(user.SocialMediaProvider);
        Assert.False(user.IsActive);
        Assert.NotNull(user.ActivationKey);
        Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 10);

        // Verify default role
        Assert.True(user.HasRole("person_user"));
        Assert.Single(user.Roles);
    }

    [Fact]
    public void Constructor_WithSocialMediaParams_ShouldCreatePersonUserWithSocialMedia()
    {
        // Arrange
        var email = Email.Create("person@example.com");
        var name = Name.Create("John Doe");
        var socialMediaProvider = SocialMediaProvider.Google;

        // Act
        var user = new PersonUser(email, name, true, socialMediaProvider);

        // Assert
        Assert.Equal(email, user.Email);
        Assert.Equal(name, user.Name);
        Assert.True(user.RegisteredViaSocialMedia);
        Assert.Equal(socialMediaProvider, user.SocialMediaProvider);
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentException()
    {
        // Arrange
        var email = Email.Create("person@example.com");
        Name name = null;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new PersonUser(email, name));
        Assert.Equal("Name cannot be null or empty. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var email = Email.Create("person@example.com");
        var name = Name.Create("John Doe");
        var user = new PersonUser(email, name);
        var newName = Name.Create("Jane Doe");

        // Act
        user.UpdateName(newName);

        // Assert
        Assert.Equal(newName, user.Name);
    }

    [Fact]
    public void UpdateName_WithNullName_ShouldThrowArgumentException()
    {
        // Arrange
        var email = Email.Create("person@example.com");
        var name = Name.Create("John Doe");
        var user = new PersonUser(email, name);
        Name newName = null;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => user.UpdateName(newName));
        Assert.Equal("Name cannot be null or empty. (Parameter 'name')", exception.Message);
    }
}