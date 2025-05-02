using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class AddressTests
{
    [Fact]
    public void Create_WithValidParams_ShouldCreateAddress()
    {
        // Arrange
        const string street = "Main Street";
        const string number = "123";
        const string city = "New York";
        const string state = "NY";
        const string country = "USA";
        const string zipCode = "10001";
        const string neighborhood = "Midtown";
        const string complement = "Apt 4B";

        // Act
        var address = Address.Create(street, number, city, state, country, zipCode, neighborhood, complement);

        // Assert
        Assert.NotNull(address);
        Assert.Equal(street, address.Street);
        Assert.Equal(number, address.Number);
        Assert.Equal(city, address.City);
        Assert.Equal(state, address.State);
        Assert.Equal(country, address.Country);
        Assert.Equal(zipCode, address.ZipCode);
        Assert.Equal(neighborhood, address.Neighborhood);
        Assert.Equal(complement, address.Complement);
    }

    [Fact]
    public void Create_WithoutOptionalParams_ShouldCreateAddress()
    {
        // Arrange
        const string street = "Main Street";
        const string number = "123";
        const string city = "New York";
        const string state = "NY";
        const string country = "USA";
        const string zipCode = "10001";

        // Act
        var address = Address.Create(street, number, city, state, country, zipCode);

        // Assert
        Assert.NotNull(address);
        Assert.Equal(street, address.Street);
        Assert.Equal(number, address.Number);
        Assert.Equal(city, address.City);
        Assert.Equal(state, address.State);
        Assert.Equal(country, address.Country);
        Assert.Equal(zipCode, address.ZipCode);
        Assert.Null(address.Neighborhood);
        Assert.Null(address.Complement);
    }

    [Theory]
    [InlineData(null, "123", "New York", "NY", "USA", "10001")]
    [InlineData("", "123", "New York", "NY", "USA", "10001")]
    [InlineData("  ", "123", "New York", "NY", "USA", "10001")]
    public void Create_WithEmptyStreet_ShouldThrowArgumentException(string street, string number, string city, string state, string country, string zipCode)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Address.Create(street, number, city, state, country, zipCode));
        
        Assert.Equal("Street cannot be empty. (Parameter 'street')", exception.Message);
    }

    [Theory]
    [InlineData("Main Street", null, "New York", "NY", "USA", "10001")]
    [InlineData("Main Street", "", "New York", "NY", "USA", "10001")]
    [InlineData("Main Street", "  ", "New York", "NY", "USA", "10001")]
    public void Create_WithEmptyNumber_ShouldThrowArgumentException(string street, string number, string city, string state, string country, string zipCode)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Address.Create(street, number, city, state, country, zipCode));
        
        Assert.Equal("Number cannot be empty. (Parameter 'number')", exception.Message);
    }

    [Theory]
    [InlineData("Main Street", "123", null, "NY", "USA", "10001")]
    [InlineData("Main Street", "123", "", "NY", "USA", "10001")]
    [InlineData("Main Street", "123", "  ", "NY", "USA", "10001")]
    public void Create_WithEmptyCity_ShouldThrowArgumentException(string street, string number, string city, string state, string country, string zipCode)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Address.Create(street, number, city, state, country, zipCode));
        
        Assert.Equal("City cannot be empty. (Parameter 'city')", exception.Message);
    }

    [Theory]
    [InlineData("Main Street", "123", "New York", null, "USA", "10001")]
    [InlineData("Main Street", "123", "New York", "", "USA", "10001")]
    [InlineData("Main Street", "123", "New York", "  ", "USA", "10001")]
    public void Create_WithEmptyState_ShouldThrowArgumentException(string street, string number, string city, string state, string country, string zipCode)
    {        
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");


        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Address.Create(street, number, city, state, country, zipCode));
        
        Assert.Equal("State cannot be empty. (Parameter 'state')", exception.Message);
    }

    [Theory]
    [InlineData("Main Street", "123", "New York", "NY", null, "10001")]
    [InlineData("Main Street", "123", "New York", "NY", "", "10001")]
    [InlineData("Main Street", "123", "New York", "NY", "  ", "10001")]
    public void Create_WithEmptyCountry_ShouldThrowArgumentException(string street, string number, string city, string state, string country, string zipCode)
    {        
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Address.Create(street, number, city, state, country, zipCode));
        
        Assert.Equal("Country cannot be empty. (Parameter 'country')", exception.Message);
    }

    [Theory]
    [InlineData("Main Street", "123", "New York", "NY", "USA", null)]
    [InlineData("Main Street", "123", "New York", "NY", "USA", "")]
    [InlineData("Main Street", "123", "New York", "NY", "USA", "  ")]
    public void Create_WithEmptyZipCode_ShouldThrowArgumentException(string street, string number, string city, string state, string country, string zipCode)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");


        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Address.Create(street, number, city, state, country, zipCode));

        Assert.Equal("Zip code cannot be empty. (Parameter 'zipCode')", exception.Message);
    }

    [Fact]
    public void Create_WithWhitespaceInFields_ShouldTrimValues()
    {
        // Arrange
        const string street = "  Main Street  ";
        const string number = "  123  ";
        const string city = "  New York  ";
        const string state = "  NY  ";
        const string country = "  USA  ";
        const string zipCode = "  10001  ";

        // Act
        var address = Address.Create(street, number, city, state, country, zipCode);

        // Assert
        Assert.Equal("Main Street", address.Street);
        Assert.Equal("123", address.Number);
        Assert.Equal("New York", address.City);
        Assert.Equal("NY", address.State);
        Assert.Equal("USA", address.Country);
        Assert.Equal("10001", address.ZipCode);
    }

    [Theory]
    [InlineData("12345-6789", "12345-6789")]
    [InlineData("12345", "12345")]
    [InlineData("123456789", "123456789")]
    [InlineData("12345-6789", "12345-6789")]
    [InlineData("12.345-678", "12345678")]
    public void Create_WithDifferentZipCodeFormats_ShouldNormalizeZipCode(string input, string expected)
    {
        // Arrange & Act
        var address = Address.Create("Main Street", "123", "New York", "NY", "USA", input);

        // Assert
        Assert.Equal(expected.Replace("-", ""), address.ZipCode);
    }

    [Fact]
    public void ToFormattedString_WithCompleteAddress_ShouldFormatCorrectly()
    {
        // Arrange
        var address = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001", "Midtown", "Apt 4B");

        // Act
        var formatted = address.ToFormattedString();

        // Assert
        Assert.Equal("Main Street, 123 - Apt 4B, Midtown, New York, NY, USA - 10001", formatted);
    }

    [Fact]
    public void ToMultiLineString_WithCompleteAddress_ShouldFormatCorrectly()
    {
        // Arrange
        var address = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001", "Midtown", "Apt 4B");

        // Act
        var multiLine = address.ToMultiLineString();

        // Assert
        var expected =
            "Main Street, 123" + Environment.NewLine +
            "Apt 4B" + Environment.NewLine +
            "Midtown" + Environment.NewLine +
            "New York, NY" + Environment.NewLine +
            "USA" + Environment.NewLine +
            "10001";

        Assert.Equal(expected, multiLine);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var address = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");

        // Act
        var result = address.ToString();

        // Assert
        Assert.Equal(address.ToFormattedString(), result);
    }

    [Fact]
    public void Equals_WithIdenticalAddresses_ShouldReturnTrue()
    {
        // Arrange
        var address1 = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");
        var address2 = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");

        // Act & Assert
        Assert.True(address1.Equals(address2));
    }

    [Fact]
    public void Equals_WithDifferentAddresses_ShouldReturnFalse()
    {
        // Arrange
        var address1 = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");
        var address2 = Address.Create("Broadway", "456", "Chicago", "IL", "USA", "60007");

        // Act & Assert
        Assert.False(address1.Equals(address2));
    }

    [Fact]
    public void Equals_WithCaseDifference_ShouldReturnTrue()
    {
        // Arrange
        var address1 = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");
        var address2 = Address.Create("main street", "123", "new york", "ny", "usa", "10001");

        // Act & Assert
        Assert.True(address1.Equals(address2));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var address = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");

        // Act & Assert
        Assert.False(address.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var address = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");
        var notAddress = "Just a string";

        // Act & Assert
        Assert.False(address.Equals(notAddress));
    }

    [Fact]
    public void GetHashCode_WithIdenticalAddresses_ShouldReturnSameValue()
    {
        // Arrange
        var address1 = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");
        var address2 = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");

        // Act
        var hashCode1 = address1.GetHashCode();
        var hashCode2 = address2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentCaseAddresses_ShouldReturnSameValue()
    {
        // Arrange
        var address1 = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");
        var address2 = Address.Create("main street", "123", "new york", "ny", "usa", "10001");

        // Act
        var hashCode1 = address1.GetHashCode();
        var hashCode2 = address2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentAddresses_ShouldReturnDifferentValues()
    {
        // Arrange
        var address1 = Address.Create("Main Street", "123", "New York", "NY", "USA", "10001");
        var address2 = Address.Create("Broadway", "456", "Chicago", "IL", "USA", "60007");

        // Act
        var hashCode1 = address1.GetHashCode();
        var hashCode2 = address2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }
}