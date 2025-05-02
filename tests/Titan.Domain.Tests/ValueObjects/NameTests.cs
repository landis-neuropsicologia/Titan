using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class NameTests
{
    [Fact]
    public void Create_WithValidName_ShouldCreateName()
    {
        // Arrange
        const string validName = "John Doe";

        // Act
        var name = Name.Create(validName);

        // Assert
        Assert.NotNull(name);
        Assert.Equal(validName, name.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyName_ShouldThrowArgumentException(string emptyName)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Name.Create(emptyName));

        Assert.Equal("Name cannot be null or empty. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void Create_WithTooLongName_ShouldThrowArgumentException()
    {
        // Arrange
        var longName = new string('a', 257);
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Name.Create(longName));

        Assert.Equal("Name is too long. (Parameter 'name')", exception.Message);
    }

    [Fact]
    public void Create_WithNameContainingWhitespace_ShouldTrimAndCreateName()
    {
        // Arrange
        const string nameWithSpaces = "  John Doe  ";
        const string expected = "John Doe";

        // Act
        var name = Name.Create(nameWithSpaces);

        // Assert
        Assert.Equal(expected, name.Value);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        const string validName = "John Doe";
        var name = Name.Create(validName);

        // Act
        string result = name;

        // Assert
        Assert.Equal(validName, result);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string validName = "John Doe";
        var name = Name.Create(validName);

        // Act
        var result = name.ToString();

        // Assert
        Assert.Equal(validName, result);
    }

    [Fact]
    public void Equals_WithSameName_ShouldReturnTrue()
    {
        // Arrange
        const string validName = "John Doe";
        var name1 = Name.Create(validName);
        var name2 = Name.Create(validName);

        // Act & Assert
        Assert.True(name1.Equals(name2));
    }

    [Fact]
    public void Equals_WithDifferentName_ShouldReturnFalse()
    {
        // Arrange
        var name1 = Name.Create("John Doe");
        var name2 = Name.Create("Jane Doe");

        // Act & Assert
        Assert.False(name1.Equals(name2));
    }

    [Fact]
    public void Equals_WithDifferentCase_ShouldReturnTrue()
    {
        // Arrange
        var name1 = Name.Create("John Doe");
        var name2 = Name.Create("john doe");

        // Act & Assert
        Assert.True(name1.Equals(name2));
    }
}