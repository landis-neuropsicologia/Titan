using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class PhoneNumberTests
{
    [Theory]
    [InlineData("+1 555 123 4567", "+1", "555 123 4567", true)]
    [InlineData("+44 20 7946 0958", "+44", "20 7946 0958", true)]
    [InlineData("+55 11 98765 4321", "+55", "11 98765 4321", true)]
    [InlineData("11 98765 4321", "", "11 98765 4321", false)]
    [InlineData("11987654321", "", "11 98765 4321", false)]
    public void Create_WithValidPhoneNumber_ShouldCreatePhoneNumber(string input, string expectedCountryCode, string expectedNational, bool expectedInternational)
    {
        // Act
        var phoneNumber = PhoneNumber.Create(input);

        // Assert
        Assert.NotNull(phoneNumber);

        if (expectedInternational)
        {
            Assert.Equal(input, phoneNumber.Value);
        }
        else
        {
            if (input.Length == 13)
            {
                Assert.Equal(input, phoneNumber.NationalNumber);
            }
            else
            {
                Assert.Equal(input, phoneNumber.NationalNumberClear);
            }
        }

        Assert.Equal(expectedCountryCode, phoneNumber.CountryCode);
        Assert.Equal(expectedNational, phoneNumber.NationalNumber);
        Assert.Equal(expectedInternational, phoneNumber.IsInternational);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyPhoneNumber_ShouldThrowArgumentException(string emptyPhone)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => PhoneNumber.Create(emptyPhone));

        Assert.Equal("Phone number cannot be empty. (Parameter 'phoneNumber')", exception.Message);
    }

    [Theory]
    [InlineData("+12")]
    [InlineData("+abcdefg")]
    [InlineData("++1234567890")]
    [InlineData("+")]
    public void Create_WithInvalidInternationalFormat_ShouldThrowArgumentException(string invalidPhone)
    {
        // Act & Assert
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        var exception = Assert.Throws<ArgumentException>(() => PhoneNumber.Create(invalidPhone));

        Assert.Equal("Invalid international phone number format. (Parameter 'phoneNumber')", exception.Message);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("123")]
    [InlineData("12345")]
    public void Create_WithInvalidLocalFormat_ShouldThrowArgumentException(string invalidPhone)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => PhoneNumber.Create(invalidPhone));

        Assert.Equal("Invalid phone number format. (Parameter 'phoneNumber')", exception.Message);
    }

    [Fact]
    public void ToE164Format_WithInternationalNumber_ShouldReturnProperFormat()
    {
        // Arrange
        var phoneNumber = PhoneNumber.Create("+1 555 123 4567");

        // Act
        var result = phoneNumber.ToE164Format();

        // Assert
        Assert.Equal("+15551234567", result);
    }

    [Fact]
    public void ToE164Format_WithLocalNumber_ShouldThrowInvalidOperationException()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        var phoneNumber = PhoneNumber.Create("11 98765 4321");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => phoneNumber.ToE164Format());

        Assert.Equal("Cannot convert to E.164 format without country code.", exception.Message);
    }

    [Theory]
    [InlineData("+1 555 123 4567", "+1 555 123 4567")] // EUA
    [InlineData("+15551234567", "+1 555 123 4567")] // EUA
    [InlineData("+44 20 7946 0958", "+44 20 7946 0958")] // UK
    [InlineData("+442079460958", "+44 20 7946 0958")] // UK
    [InlineData("+55 11 98765 4321", "+55 11 98765 4321")] // BR
    [InlineData("+5511987654321", "+55 11 98765 4321")] // BR
    [InlineData("11 98765 4321", "11 98765 4321")] // BR
    [InlineData("11987654321", "11 98765 4321")] // BR
    [InlineData("11 8765 4321", "11 8765 4321")] // BR
    [InlineData("1187654321", "11 8765 4321")] // BR
    [InlineData("+351 919 352 449", "+351 919 352 449")] // PT
    [InlineData("+351919352449", "+351 919 352 449")] // PT
    public void ToFormattedString_ShouldReturnFormattedNumber(string input, string expected)
    {
        // Arrange
        var phoneNumber = PhoneNumber.Create(input);

        // Act
        var result = phoneNumber.ToFormattedString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        const string phoneStr = "+1 555 123 4567";
        var phoneNumber = PhoneNumber.Create(phoneStr);

        // Act
        string result = phoneNumber;

        // Assert
        Assert.Equal(phoneStr, result);
    }

    [Fact]
    public void ImplicitOperator_WithNull_ShouldReturnNull()
    {
        // Arrange
        PhoneNumber phoneNumber = null;

        // Act
        string result = phoneNumber;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string phoneStr = "+1 555 123 4567";
        var phoneNumber = PhoneNumber.Create(phoneStr);

        // Act
        var result = phoneNumber.ToString();

        // Assert
        Assert.Equal(phoneStr, result);
    }

    [Fact]
    public void Equals_WithIdenticalNumbers_ShouldReturnTrue()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("+1 555 123 4567");
        var phone2 = PhoneNumber.Create("+1 555 123 4567");

        // Act & Assert
        Assert.True(phone1.Equals(phone2));
    }

    [Fact]
    public void Equals_WithDifferentFormatSameNumber_ShouldReturnTrue()
    {
        // Arrange - Same number with different formatting
        var phone1 = PhoneNumber.Create("+1 555 123 4567");
        var phone2 = PhoneNumber.Create("+1 5551234567");

        // Act & Assert
        Assert.True(phone1.Equals(phone2));
    }

    [Fact]
    public void Equals_WithDifferentNumbers_ShouldReturnFalse()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("+1 555 123 4567");
        var phone2 = PhoneNumber.Create("+1 555 123 7890");

        // Act & Assert
        Assert.False(phone1.Equals(phone2));
    }

    [Fact]
    public void Equals_WithLocalAndInternational_ShouldNotCompareIfCountryCodeDifferent()
    {
        // Arrange
        // These would be the same number if local number was in country code +1
        var phone1 = PhoneNumber.Create("+1 555 123 4567");
        var phone2 = PhoneNumber.Create("555 123 4567");

        // Act & Assert
        Assert.False(phone1.Equals(phone2));
    }

    [Fact]
    public void Equals_WithDifferentTypes_ShouldReturnFalse()
    {
        // Arrange
        var phone = PhoneNumber.Create("+1 555 123 4567");
        var notPhone = "Just a string";

        // Act & Assert
        Assert.False(phone.Equals(notPhone));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var phone = PhoneNumber.Create("+1 555 123 4567");

        // Act & Assert
        Assert.False(phone.Equals(null));
    }
}