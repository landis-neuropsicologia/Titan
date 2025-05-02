using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class PasswordHashTests
{
    [Fact]
    public void Create_WithValidPassword_ShouldCreatePasswordHash()
    {
        // Arrange
        const string password = "SecurePassword123";

        // Act
        var passwordHash = PasswordHash.Create(password);

        // Assert
        Assert.NotNull(passwordHash);
        Assert.NotEmpty(passwordHash.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidPassword_ShouldThrowArgumentException(string invalidPassword)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => PasswordHash.Create(invalidPassword));

        Assert.Equal("Password cannot be empty. (Parameter 'plainTextPassword')", exception.Message);
    }

    [Fact]
    public void Create_WithSamePwd_ShouldCreateSameHash()
    {
        // Arrange
        const string password = "SecurePassword123";
        const string salt = "TestSalt";

        // Act
        var hash1 = PasswordHash.Create(password, salt);
        var hash2 = PasswordHash.Create(password, salt);

        // Assert
        Assert.Equal(hash1.Value, hash2.Value);
    }

    [Fact]
    public void Create_WithDifferentSalts_ShouldCreateDifferentHashes()
    {
        // Arrange
        const string password = "SecurePassword123";
        const string salt1 = "Salt1";
        const string salt2 = "Salt2";

        // Act
        var hash1 = PasswordHash.Create(password, salt1);
        var hash2 = PasswordHash.Create(password, salt2);

        // Assert
        Assert.NotEqual(hash1.Value, hash2.Value);
    }

    [Fact]
    public void Create_WithDifferentPasswords_ShouldCreateDifferentHashes()
    {
        // Arrange
        const string password1 = "Password1";
        const string password2 = "Password2";
        const string salt = "SameSalt";

        // Act
        var hash1 = PasswordHash.Create(password1, salt);
        var hash2 = PasswordHash.Create(password2, salt);

        // Assert
        Assert.NotEqual(hash1.Value, hash2.Value);
    }

    [Fact]
    public void FromHash_WithValidHash_ShouldCreatePasswordHash()
    {
        // Arrange
        const string hashValue = "TestHashValue";

        // Act
        var passwordHash = PasswordHash.FromHash(hashValue);

        // Assert
        Assert.NotNull(passwordHash);
        Assert.Equal(hashValue, passwordHash.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void FromHash_WithInvalidHash_ShouldThrowArgumentException(string invalidHash)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => PasswordHash.FromHash(invalidHash));

        Assert.Equal("Hash value cannot be empty. (Parameter 'hash')", exception.Message);
    }

    [Fact]
    public void Verify_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        const string password = "SecurePassword123";
        const string salt = "TestSalt";
        var passwordHash = PasswordHash.Create(password, salt);

        // Act
        var result = passwordHash.Verify(password, salt);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        const string password = "SecurePassword123";
        const string incorrectPassword = "WrongPassword";
        const string salt = "TestSalt";
        var passwordHash = PasswordHash.Create(password, salt);

        // Act
        var result = passwordHash.Verify(incorrectPassword, salt);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_WithIncorrectSalt_ShouldReturnFalse()
    {
        // Arrange
        const string password = "SecurePassword123";
        const string salt = "CorrectSalt";
        const string incorrectSalt = "WrongSalt";

        var passwordHash = PasswordHash.Create(password, salt);

        // Act
        var result = passwordHash.Verify(password, incorrectSalt);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Verify_WithInvalidPassword_ShouldReturnFalse(string invalidPassword)
    {
        // Arrange
        var passwordHash = PasswordHash.Create("ValidPassword");

        // Act
        var result = passwordHash.Verify(invalidPassword);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        const string password = "SecurePassword123";

        var passwordHash = PasswordHash.Create(password);

        // Act
        string result = passwordHash;

        // Assert
        Assert.Equal(passwordHash.Value, result);
    }

    [Fact]
    public void ImplicitOperator_WithNull_ShouldReturnNull()
    {
        // Arrange
        PasswordHash passwordHash = null;

        // Act
        string result = passwordHash;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string password = "SecurePassword123";

        var passwordHash = PasswordHash.Create(password);

        // Act
        var result = passwordHash.ToString();

        // Assert
        Assert.Equal(passwordHash.Value, result);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const string hashValue = "SameHashValue";
        var hash1 = PasswordHash.FromHash(hashValue);
        var hash2 = PasswordHash.FromHash(hashValue);

        // Act & Assert
        Assert.True(hash1.Equals(hash2));
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var hash1 = PasswordHash.FromHash("Hash1");
        var hash2 = PasswordHash.FromHash("Hash2");

        // Act & Assert
        Assert.False(hash1.Equals(hash2));
    }
}