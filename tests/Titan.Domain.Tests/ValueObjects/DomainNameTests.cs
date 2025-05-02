using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class DomainNameTests
{
    [Theory]
    [InlineData("example.com", "example.com")]
    [InlineData("sub.domain.co.uk", "sub.domain.co.uk")]
    [InlineData("www.example.com", "example.com")]
    [InlineData("http://example.com", "example.com")]
    [InlineData("https://example.com", "example.com")]
    [InlineData("http://www.example.com", "example.com")]
    [InlineData("https://www.example.com", "example.com")]
    [InlineData("example.com/path", "example.com")]
    [InlineData("  example.com  ", "example.com")]
    [InlineData("EXAMPLE.COM", "example.com")]
    public void Create_WithValidDomain_ShouldCreateDomainName(string input, string expected)
    {
        // Act
        var domain = DomainName.Create(input);

        // Assert
        Assert.NotNull(domain);
        Assert.Equal(expected, domain.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyDomain_ShouldReturnNull(string emptyDomain)
    {
        // Act
        var domain = DomainName.Create(emptyDomain);

        // Assert
        Assert.Null(domain);
    }

    [Fact]
    public void Create_WithTooLongDomain_ShouldThrowArgumentException()
    {
        // Arrange
        var longDomain = new string('a', 253) + ".com"; // 257 chars total

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => DomainName.Create(longDomain));

        Assert.Equal("Domain is too long. (Parameter 'domain')", exception.Message);
    }

    [Fact]
    public void Create_WithoutPeriod_ShouldThrowArgumentException()
    {
        // Arrange
        const string invalidDomain = "example";

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => DomainName.Create(invalidDomain));
        Assert.Equal("Domain must contain at least one period. (Parameter 'domain')", exception.Message);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        const string validDomain = "example.com";
        var domain = DomainName.Create(validDomain);

        // Act
        string result = domain;

        // Assert
        Assert.Equal(validDomain, result);
    }

    [Fact]
    public void ImplicitOperator_WithNull_ShouldReturnNull()
    {
        // Arrange
        DomainName domain = null;

        // Act
        string result = domain;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string validDomain = "example.com";
        var domain = DomainName.Create(validDomain);

        // Act
        var result = domain.ToString();

        // Assert
        Assert.Equal(validDomain, result);
    }

    [Fact]
    public void Equals_WithSameDomain_ShouldReturnTrue()
    {
        // Arrange
        var domain1 = DomainName.Create("example.com");
        var domain2 = DomainName.Create("example.com");

        // Act & Assert
        Assert.True(domain1.Equals(domain2));
    }

    [Fact]
    public void Equals_WithDifferentCase_ShouldReturnTrue()
    {
        // Arrange
        var domain1 = DomainName.Create("Example.Com");
        var domain2 = DomainName.Create("example.com");

        // Act & Assert
        Assert.True(domain1.Equals(domain2));
    }

    [Fact]
    public void Equals_WithDifferentDomain_ShouldReturnFalse()
    {
        // Arrange
        var domain1 = DomainName.Create("example1.com");
        var domain2 = DomainName.Create("example2.com");

        // Act & Assert
        Assert.False(domain1.Equals(domain2));
    }
}