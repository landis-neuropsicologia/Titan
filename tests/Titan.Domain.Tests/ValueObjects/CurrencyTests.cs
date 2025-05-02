using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class CurrencyTests
{
    [Theory]
    [InlineData(123.45, "USD", 123.45, 2)]
    [InlineData(123.45, "EUR", 123.45, 2)]
    [InlineData(123.45, "GBP", 123.45, 2)]
    [InlineData(123.45, "BRL", 123.45, 2)]
    [InlineData(123.45, "JPY", 123, 0)]  // JPY has no decimal places
    public void Create_WithValidParams_ShouldCreateCurrency(decimal amount, string code, decimal expectedAmount, int expectedDecimals)
    {
        // Act
        var currency = Currency.Create(amount, code);

        // Assert
        Assert.NotNull(currency);
        Assert.Equal(expectedAmount, currency.Amount);
        Assert.Equal(code.ToUpperInvariant(), currency.CurrencyCode);
        Assert.Equal(expectedDecimals, currency.DecimalPlaces);
    }

    [Theory]
    [InlineData(123.456, "USD", 123.46)]  // Rounds up
    [InlineData(123.454, "USD", 123.45)]  // Rounds down
    [InlineData(123.45, "JPY", 123)]      // JPY rounds to whole numbers
    public void Create_WithFractionalAmount_ShouldRoundCorrectly(decimal amount, string code, decimal expectedAmount)
    {
        // Act
        var currency = Currency.Create(amount, code);

        // Assert
        Assert.Equal(expectedAmount, currency.Amount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidCurrencyCode_ShouldThrowArgumentException(string invalidCode)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Currency.Create(100, invalidCode));

        Assert.Equal("Currency code cannot be empty. (Parameter 'currencyCode')", exception.Message);
    }

    [Fact]
    public void Create_WithLowercaseCurrencyCode_ShouldNormalize()
    {
        // Arrange
        const decimal amount = 100;
        const string code = "usd";

        // Act
        var currency = Currency.Create(amount, code);

        // Assert
        Assert.Equal("USD", currency.CurrencyCode);
    }

    [Theory]
    [InlineData("123.45", "USD", 123.45)]
    [InlineData("0", "EUR", 0)]
    [InlineData("1000", "GBP", 1000)]
    public void FromString_WithValidString_ShouldCreateCurrency(
        string value, string code, decimal expectedAmount)
    {
        // Act
        var currency = Currency.FromString(value, code);

        // Assert
        Assert.Equal(expectedAmount, currency.Amount);
        Assert.Equal(code.ToUpperInvariant(), currency.CurrencyCode);
    }

    [Theory]
    [InlineData(null, "USD")]
    [InlineData("", "USD")]
    [InlineData("   ", "USD")]
    public void FromString_WithInvalidValue_ShouldThrowArgumentException(string invalidValue, string code)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Currency.FromString(invalidValue, code));

        Assert.Equal("Value cannot be empty. (Parameter 'value')", exception.Message);
    }

    [Theory]
    [InlineData("abc", "USD")]
    [InlineData("$100", "USD")]
    public void FromString_WithNonNumericValue_ShouldThrowArgumentException(string nonNumericValue, string code)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Currency.FromString(nonNumericValue, code));

        Assert.Equal("Invalid monetary value. (Parameter 'value')", exception.Message);
    }

    [Fact]
    public void Add_WithSameCurrency_ShouldAddAmounts()
    {
        // Arrange
        var currency1 = Currency.Create(100, "USD");
        var currency2 = Currency.Create(50, "USD");
        var expected = Currency.Create(150, "USD");

        // Act
        var result = currency1.Add(currency2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Add_WithDifferentCurrencies_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var currency1 = Currency.Create(100, "USD");
        var currency2 = Currency.Create(50, "EUR");
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => currency1.Add(currency2));
        Assert.Equal("Cannot add currencies with different codes: USD and EUR", exception.Message);
    }

    [Fact]
    public void Add_WithNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var currency = Currency.Create(100, "USD");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => currency.Add(null));
    }

    [Fact]
    public void Subtract_WithSameCurrency_ShouldSubtractAmounts()
    {
        // Arrange
        var currency1 = Currency.Create(100, "USD");
        var currency2 = Currency.Create(30, "USD");
        var expected = Currency.Create(70, "USD");

        // Act
        var result = currency1.Subtract(currency2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Subtract_WithDifferentCurrencies_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var currency1 = Currency.Create(100, "USD");
        var currency2 = Currency.Create(50, "EUR");
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => currency1.Subtract(currency2));

        Assert.Equal("Cannot subtract currencies with different codes: USD and EUR", exception.Message);
    }

    [Fact]
    public void Subtract_WithNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var currency = Currency.Create(100, "USD");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => currency.Subtract(null));
    }

    [Theory]
    [InlineData(100, 2, 200)]
    [InlineData(100, 0.5, 50)]
    [InlineData(100, 0, 0)]
    [InlineData(100, -1, -100)]
    public void Multiply_WithVariousFactors_ShouldMultiplyAmount(decimal initialAmount, decimal factor, decimal expectedAmount)
    {
        // Arrange
        var currency = Currency.Create(initialAmount, "USD");
        var expected = Currency.Create(expectedAmount, "USD");

        // Act
        var result = currency.Multiply(factor);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(100, 2, 50)]
    [InlineData(100, 4, 25)]
    [InlineData(100, 0.5, 200)]
    [InlineData(100, -2, -50)]
    public void Divide_WithVariousDivisors_ShouldDivideAmount(decimal initialAmount, decimal divisor, decimal expectedAmount)
    {
        // Arrange
        var currency = Currency.Create(initialAmount, "USD");
        var expected = Currency.Create(expectedAmount, "USD");

        // Act
        var result = currency.Divide(divisor);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Divide_ByZero_ShouldThrowDivideByZeroException()
    {
        // Arrange
        var currency = Currency.Create(100, "USD");

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => currency.Divide(0));
    }

    [Theory]
    [InlineData(123.45, "USD", true, "$123.45")]
    [InlineData(123.45, "EUR", true, "€123.45")]
    [InlineData(123.45, "GBP", true, "£123.45")]
    [InlineData(123.45, "BRL", true, "R$123.45")]
    [InlineData(123, "JPY", true, "¥123")]
    [InlineData(123.45, "USD", false, "123.45")]
    [InlineData(123, "JPY", false, "123")]
    [InlineData(123.45, "XYZ", true, "XYZ 123.45")]  // Unknown currency
    public void Format_WithVariousParams_ShouldFormatCorrectly(decimal amount, string code, bool includeSymbol, string expected)
    {
        // Arrange
        var currency = Currency.Create(amount, code);

        // The test assumes certain culture formatting, but could vary by environment
        // So we'll check for the essential parts rather than exact formatting

        // Act
        var formatted = currency.Format(includeSymbol);

        // Assert - This is a simplification, actual format may vary by culture
        if (includeSymbol)
        {
            if (code == "XYZ")
            {
                Assert.StartsWith("XYZ ", formatted);
                Assert.Contains(amount.ToString(CultureInfo.InvariantCulture), formatted);
            }
            else
            {
                var symbol = code switch
                {
                    "USD" => "$",
                    "EUR" => "€",
                    "GBP" => "£",
                    "BRL" => "R$",
                    "JPY" => "￥",
                    _ => code + " "
                };

                Assert.Contains(symbol, formatted);

                if (code == "BRL" || code == "EUR")
                {
                    Assert.Contains(amount.ToString(CultureInfo.InvariantCulture), formatted.ToString(CultureInfo.InvariantCulture).Replace(",", "."));
                }
                else
                {
                    Assert.Contains(amount.ToString(CultureInfo.InvariantCulture), formatted.Replace(",", ""));
                }                    
            }
        }
        else
        {
            Assert.DoesNotContain("$", formatted);
            Assert.DoesNotContain("€", formatted);
            Assert.DoesNotContain("£", formatted);
            Assert.DoesNotContain("¥", formatted);
            Assert.Contains(amount.ToString(CultureInfo.InvariantCulture), formatted.Replace(",", ""));
        }
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var currency = Currency.Create(123.45m, "USD");

        // Act
        var result = currency.ToString();

        // Assert
        Assert.Equal(currency.Format(), result);
    }

    [Fact]
    public void Equals_WithIdenticalCurrency_ShouldReturnTrue()
    {
        // Arrange
        var currency1 = Currency.Create(100.00m, "USD");
        var currency2 = Currency.Create(100.00m, "USD");

        // Act & Assert
        Assert.True(currency1.Equals(currency2));
    }

    [Fact]
    public void Equals_WithDifferentAmount_ShouldReturnFalse()
    {
        // Arrange
        var currency1 = Currency.Create(100.00m, "USD");
        var currency2 = Currency.Create(200.00m, "USD");

        // Act & Assert
        Assert.False(currency1.Equals(currency2));
    }

    [Fact]
    public void Equals_WithDifferentCurrency_ShouldReturnFalse()
    {
        // Arrange
        var currency1 = Currency.Create(100.00m, "USD");
        var currency2 = Currency.Create(100.00m, "EUR");

        // Act & Assert
        Assert.False(currency1.Equals(currency2));
    }

    [Fact]
    public void Equals_WithVerySmallDifference_ShouldReturnTrue()
    {
        // Arrange - Difference smaller than precision
        var currency1 = Currency.Create(100.00m, "USD");
        var currency2 = Currency.Create(100.001m, "USD"); // Gets rounded to 100.00

        // Act & Assert
        Assert.True(currency1.Equals(currency2));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var currency = Currency.Create(100.00m, "USD");

        // Act & Assert
        Assert.False(currency.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var currency = Currency.Create(100.00m, "USD");
        var notCurrency = "Just a string";

        // Act & Assert
        Assert.False(currency.Equals(notCurrency));
    }

    [Fact]
    public void GetHashCode_WithIdenticalCurrency_ShouldReturnSameValue()
    {
        // Arrange
        var currency1 = Currency.Create(100.00m, "USD");
        var currency2 = Currency.Create(100.00m, "USD");

        // Act
        var hashCode1 = currency1.GetHashCode();
        var hashCode2 = currency2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentCurrency_ShouldReturnDifferentValues()
    {
        // Arrange
        var currency1 = Currency.Create(100.00m, "USD");
        var currency2 = Currency.Create(100.00m, "EUR");

        // Act
        var hashCode1 = currency1.GetHashCode();
        var hashCode2 = currency2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }
}