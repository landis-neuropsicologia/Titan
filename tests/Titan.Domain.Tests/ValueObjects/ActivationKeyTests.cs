using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class ActivationKeyTests
{
    [Fact]
    public void Generate_ShouldCreateNewActivationKey()
    {
        // Act
        var activationKey = ActivationKey.Generate();

        // Assert
        Assert.NotNull(activationKey);
        Assert.NotEmpty(activationKey.Value);
        Assert.Equal(32, activationKey.Value.Length); // Guid.ToString("N") has 32 chars
    }

    [Fact]
    public void Create_WithValidKey_ShouldCreateActivationKey()
    {
        // Arrange
        const string key = "abcd1234";

        // Act
        var activationKey = ActivationKey.Create(key);

        // Assert
        Assert.NotNull(activationKey);
        Assert.Equal(key, activationKey.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidKey_ShouldThrowArgumentException(string key)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ActivationKey.Create(key));

        Assert.Equal("Activation key cannot be empty. (Parameter 'keyCode')", exception.Message);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        const string key = "abcd1234";
        var activationKey = ActivationKey.Create(key);

        // Act
        string result = activationKey;

        // Assert
        Assert.Equal(key, result);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string key = "abcd1234";
        var activationKey = ActivationKey.Create(key);

        // Act
        var result = activationKey.ToString();

        // Assert
        Assert.Equal(key, result);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const string key = "abcd1234";
        var activationKey1 = ActivationKey.Create(key);
        var activationKey2 = ActivationKey.Create(key);

        // Act & Assert
        Assert.True(activationKey1.Equals(activationKey2));
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var activationKey1 = ActivationKey.Create("abcd1234");
        var activationKey2 = ActivationKey.Create("efgh5678");

        // Act & Assert
        Assert.False(activationKey1.Equals(activationKey2));
    }
}
