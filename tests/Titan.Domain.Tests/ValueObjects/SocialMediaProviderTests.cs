using System.Globalization;
using System.Resources;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class SocialMediaProviderTests
{
    [Theory]
    [InlineData("Google", "Google")]
    [InlineData("google", "Google")]
    [InlineData("GOOGLE", "Google")]
    [InlineData("Facebook", "Facebook")]
    [InlineData("facebook", "Facebook")]
    [InlineData("Microsoft", "Microsoft")]
    [InlineData("microsoft", "Microsoft")]
    [InlineData("  google  ", "Google")]
    public void Create_WithValidProvider_ShouldCreateSocialMediaProvider(string input, string expected)
    {
        // Act
        var provider = SocialMediaProvider.Create(input);

        // Assert
        Assert.NotNull(provider);
        Assert.Equal(expected, provider.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyProvider_ShouldReturnNull(string emptyProvider)
    {
        // Act
        var provider = SocialMediaProvider.Create(emptyProvider);

        // Assert
        Assert.Null(provider);
    }

    [Fact]
    public void Create_WithUnsupportedProvider_ShouldThrowArgumentException()
    {
        // Arrange
        const string unsupportedProvider = "Twitter";
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        var expected = $"Unsupported social media provider: {unsupportedProvider}";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => SocialMediaProvider.Create(unsupportedProvider));

        Assert.Contains(expected, exception.Message);
    }

    [Fact]
    public void StaticInstances_ShouldHaveCorrectValues()
    {
        // Act & Assert
        Assert.Equal("Google", SocialMediaProvider.Google.Value);
        Assert.Equal("Facebook", SocialMediaProvider.Facebook.Value);
        Assert.Equal("Microsoft", SocialMediaProvider.Microsoft.Value);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        var provider = SocialMediaProvider.Google;

        // Act
        string result = provider;

        // Assert
        Assert.Equal("Google", result);
    }

    [Fact]
    public void ImplicitOperator_WithNull_ShouldReturnNull()
    {
        // Arrange
        SocialMediaProvider provider = null;

        // Act
        string result = provider;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var provider = SocialMediaProvider.Facebook;

        // Act
        var result = provider.ToString();

        // Assert
        Assert.Equal("Facebook", result);
    }

    [Fact]
    public void Equals_WithSameProvider_ShouldReturnTrue()
    {
        // Arrange
        var provider1 = SocialMediaProvider.Create("Google");
        var provider2 = SocialMediaProvider.Create("Google");

        // Act & Assert
        Assert.True(provider1.Equals(provider2));
    }

    [Fact]
    public void Equals_WithDifferentProvider_ShouldReturnFalse()
    {
        // Arrange
        var provider1 = SocialMediaProvider.Google;
        var provider2 = SocialMediaProvider.Facebook;

        // Act & Assert
        Assert.False(provider1.Equals(provider2));
    }

    [Fact]
    public void Create_WithUnsupportedProvider_ShouldThrowArgumentExceptionWithLocalizedMessage()
    {
        // Arrange
        const string unsupportedProvider = "Twitter";
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => SocialMediaProvider.Create(unsupportedProvider));

        string expectedMessage = string.Format("Unsupported social media provider:" + " {0}", unsupportedProvider);

        Assert.Contains(expectedMessage, exception.Message);
    }

    [Fact]
    public void GetLocalizedName_WithDefaultCulture_ShouldReturnDefaultName()
    {
        // Arrange
        var provider = SocialMediaProvider.Google;
        var defaultCulture = CultureInfo.GetCultureInfo("en-US");

        // Act
        var result = provider.GetLocalizedName(defaultCulture);

        // Assert
        Assert.Equal("Google", result);
    }

    [Fact]
    public void GetLocalizedName_WithPortugueseCulture_ShouldReturnLocalizedName()
    {
        // Arrange
        var provider = SocialMediaProvider.Facebook;
        var portugueseCulture = CultureInfo.GetCultureInfo("pt-BR");

        // Act
        var result = provider.GetLocalizedName(portugueseCulture);

        // Assert
        Assert.Equal("Facebook", result); // Facebook is the same in Portuguese, but using the resource
    }

    [Fact]
    public void ToLocalizedString_WithDefaultCulture_ShouldReturnDefaultName()
    {
        // Arrange
        var provider = SocialMediaProvider.Microsoft;
        var defaultCulture = CultureInfo.GetCultureInfo("en-US");

        // Act
        var result = provider.GetLocalizedName(defaultCulture);

        // Assert
        Assert.Equal("Microsoft", result);
    }

    [Fact]
    public void ToLocalizedString_WithoutSpecifiedCulture_ShouldUseCurrentCulture()
    {
        // Arrange
        var provider = SocialMediaProvider.Google;
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            // Set current culture to pt-BR for this test
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");

            // Act
            var result = provider.GetLocalizedName();

            // Assert
            Assert.Equal("Google", result);
        }
        finally
        {
            // Restore original culture
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}