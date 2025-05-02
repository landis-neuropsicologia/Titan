using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class EmailTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name+tag@example.com")]
    [InlineData("user-name@example.co.uk")]
    public void Create_WithValidEmail_ShouldCreateEmail(string validEmail)
    {
        // Act
        var email = Email.Create(validEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(validEmail, email.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyEmail_ShouldThrowArgumentException(string emptyEmail)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Email.Create(emptyEmail));
        
        Assert.Equal("Email cannot be null or empty. (Parameter 'email')", exception.Message);
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@dotcom")]
    [InlineData("@example.com")]
    [InlineData("user@.com")]
    public void Create_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
        
        Assert.Equal("Email is invalid. (Parameter 'email')", exception.Message);
    }

    [Fact]
    public void Create_WithTooLongEmail_ShouldThrowArgumentException()
    {
        // Arrange
        var longEmail = new string('a', 247) + "@example.com"; // 257 chars total

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Email.Create(longEmail));
        
        Assert.Equal("Email is too long. (Parameter 'email')", exception.Message);
    }

    [Fact]
    public void Create_WithEmailContainingWhitespace_ShouldTrimAndCreateEmail()
    {
        // Arrange
        const string emailWithSpaces = "  user@example.com  ";
        const string expected = "user@example.com";

        // Act
        var email = Email.Create(emailWithSpaces);

        // Assert
        Assert.Equal(expected, email.Value);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        const string validEmail = "test@example.com";
        var email = Email.Create(validEmail);

        // Act
        string result = email;

        // Assert
        Assert.Equal(validEmail, result);
    }

    [Fact]
    public void Equals_WithSameEmail_ShouldReturnTrue()
    {
        // Arrange
        const string validEmail = "test@example.com";
        var email1 = Email.Create(validEmail);
        var email2 = Email.Create(validEmail);

        // Act & Assert
        Assert.True(email1.Equals(email2));
    }

    [Fact]
    public void Equals_WithDifferentEmail_ShouldReturnFalse()
    {
        // Arrange
        var email1 = Email.Create("test1@example.com");
        var email2 = Email.Create("test2@example.com");

        // Act & Assert
        Assert.False(email1.Equals(email2));
    }

    [Fact]
    public void Equals_WithDifferentCase_ShouldReturnTrue()
    {
        // Arrange
        var email1 = Email.Create("Test@Example.com");
        var email2 = Email.Create("test@example.com");

        // Act & Assert
        Assert.True(email1.Equals(email2));
    }
}
