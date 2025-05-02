using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class EmployeeIdTests
{
    [Fact]
    public void Create_WithValidEmployeeId_ShouldCreateEmployeeId()
    {
        // Arrange
        const string validEmployeeId = "EMP12345";

        // Act
        var employeeId = EmployeeId.Create(validEmployeeId);

        // Assert
        Assert.NotNull(employeeId);
        Assert.Equal(validEmployeeId, employeeId.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyEmployeeId_ShouldThrowArgumentException(string emptyEmployeeId)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => EmployeeId.Create(emptyEmployeeId));
        Assert.Equal("Employee ID cannot be empty. (Parameter 'employeeId')", exception.Message);
    }

    [Fact]
    public void Create_WithTooLongEmployeeId_ShouldThrowArgumentException()
    {
        // Arrange
        var longEmployeeId = new string('E', 51);

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => EmployeeId.Create(longEmployeeId));
        Assert.Equal("Employee ID is too long. (Parameter 'employeeId')", exception.Message);
    }

    [Fact]
    public void Create_WithEmployeeIdContainingWhitespace_ShouldTrimAndCreateEmployeeId()
    {
        // Arrange
        const string employeeIdWithSpaces = "  EMP12345  ";
        const string expected = "EMP12345";

        // Act
        var employeeId = EmployeeId.Create(employeeIdWithSpaces);

        // Assert
        Assert.Equal(expected, employeeId.Value);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        const string validEmployeeId = "EMP12345";
        var employeeId = EmployeeId.Create(validEmployeeId);

        // Act
        string result = employeeId;

        // Assert
        Assert.Equal(validEmployeeId, result);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string validEmployeeId = "EMP12345";
        var employeeId = EmployeeId.Create(validEmployeeId);

        // Act
        var result = employeeId.ToString();

        // Assert
        Assert.Equal(validEmployeeId, result);
    }

    [Fact]
    public void Equals_WithSameEmployeeId_ShouldReturnTrue()
    {
        // Arrange
        const string validEmployeeId = "EMP12345";
        var employeeId1 = EmployeeId.Create(validEmployeeId);
        var employeeId2 = EmployeeId.Create(validEmployeeId);

        // Act & Assert
        Assert.True(employeeId1.Equals(employeeId2));
    }

    [Fact]
    public void Equals_WithDifferentEmployeeId_ShouldReturnFalse()
    {
        // Arrange
        var employeeId1 = EmployeeId.Create("EMP12345");
        var employeeId2 = EmployeeId.Create("EMP67890");

        // Act & Assert
        Assert.False(employeeId1.Equals(employeeId2));
    }

    [Fact]
    public void Equals_WithDifferentCase_ShouldReturnTrue()
    {
        // Arrange
        var employeeId1 = EmployeeId.Create("emp12345");
        var employeeId2 = EmployeeId.Create("EMP12345");

        // Act & Assert
        Assert.True(employeeId1.Equals(employeeId2));
    }
}