using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class JobTitleTests
{
    [Theory]
    [InlineData("Developer", null, 0, "Developer")]
    [InlineData("Manager", "IT", 2, "Manager")]
    [InlineData("Chief Executive Officer", "Executive", 0, "Chief Executive Officer")]
    public void Create_WithValidParams_ShouldCreateJobTitle(string title, string department, int level, string expectedTitle)
    {
        // Act
        var jobTitle = JobTitle.Create(title, department, level);

        // Assert
        Assert.NotNull(jobTitle);
        Assert.Equal(expectedTitle, jobTitle.Title);
        Assert.Equal(department, jobTitle.Department);
        Assert.Equal(level, jobTitle.Level);
    }

    [Theory]
    [InlineData("  Developer  ", "Developer")]
    [InlineData("DEV", "Developer")]
    [InlineData("dev", "Developer")]
    [InlineData("Software Engineer", "Developer")]
    [InlineData("Programmer", "Developer")]
    [InlineData("CEO", "Chief Executive Officer")]
    [InlineData("CTO", "Chief Technology Officer")]
    [InlineData("CFO", "Chief Financial Officer")]
    [InlineData("MGR", "Manager")]
    [InlineData("Business Analyst", "Analyst")]
    public void Create_WithVariousTitles_ShouldNormalizeTitle(string input, string expected)
    {
        // Act
        var jobTitle = JobTitle.Create(input);

        // Assert
        Assert.Equal(expected, jobTitle.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyTitle_ShouldThrowArgumentException(string emptyTitle)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => JobTitle.Create(emptyTitle));
        
        Assert.Equal("Job title cannot be empty. (Parameter 'title')", exception.Message);
    }

    [Fact]
    public void Create_WithNegativeLevel_ShouldThrowArgumentException()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => JobTitle.Create("Developer", null, -1));

        Assert.Equal("Level cannot be negative. (Parameter 'level')", exception.Message);
    }

    [Theory]
    [InlineData("CEO", "Chief Executive Officer")]
    [InlineData("CTO", "Chief Technology Officer")]
    [InlineData("DEV", "Developer")]
    [InlineData("MGR", "Manager")]
    [InlineData("BA", "Analyst")]
    public void CreateWithAbbreviation_WithValidAbbreviations_ShouldCreateJobTitle(string abbreviation, string expectedTitle)
    {
        // Act
        var jobTitle = JobTitle.CreateWithAbbreviation(abbreviation);

        // Assert
        Assert.Equal(expectedTitle, jobTitle.Title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateWithAbbreviation_WithEmptyAbbreviation_ShouldThrowArgumentException(string emptyAbbreviation)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => JobTitle.CreateWithAbbreviation(emptyAbbreviation));

        Assert.Equal("Abbreviation cannot be empty. (Parameter 'abbreviation')", exception.Message);
    }

    [Fact]
    public void CreateWithAbbreviation_WithUnknownAbbreviation_ShouldThrowArgumentException()
    {
        // Arrange
        const string unknownAbbreviation = "XYZ";

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => JobTitle.CreateWithAbbreviation(unknownAbbreviation));

        Assert.Equal($"Unknown job title abbreviation: {unknownAbbreviation} (Parameter 'abbreviation')", exception.Message);
    }

    [Fact]
    public void WithDepartment_ShouldSetDepartment()
    {
        // Arrange
        var jobTitle = JobTitle.Create("Developer");
        const string department = "Engineering";

        // Act
        var newJobTitle = jobTitle.WithDepartment(department);

        // Assert
        Assert.Equal(jobTitle.Title, newJobTitle.Title);
        Assert.Equal(department, newJobTitle.Department);
        Assert.Equal(jobTitle.Level, newJobTitle.Level);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void WithDepartment_WithEmptyDepartment_ShouldThrowArgumentException(string emptyDepartment)
    {
        // Arrange
        var jobTitle = JobTitle.Create("Developer");

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => jobTitle.WithDepartment(emptyDepartment));
        Assert.Equal("Department cannot be empty. (Parameter 'department')", exception.Message);
    }

    [Fact]
    public void WithLevel_ShouldSetLevel()
    {
        // Arrange
        var jobTitle = JobTitle.Create("Developer");
        const int level = 3;

        // Act
        var newJobTitle = jobTitle.WithLevel(level);

        // Assert
        Assert.Equal(jobTitle.Title, newJobTitle.Title);
        Assert.Equal(jobTitle.Department, newJobTitle.Department);
        Assert.Equal(level, newJobTitle.Level);
    }

    [Fact]
    public void WithLevel_WithNegativeLevel_ShouldThrowArgumentException()
    {
        // Arrange
        var jobTitle = JobTitle.Create("Developer");
        const int negativeLevel = -1;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => jobTitle.WithLevel(negativeLevel));

        Assert.Equal("Level cannot be negative. (Parameter 'level')", exception.Message);
    }

    [Theory]
    [InlineData("Chief Executive Officer", true)]
    [InlineData("CEO", true)]
    [InlineData("Chief Financial Officer", true)]
    [InlineData("Manager", true)]
    [InlineData("IT Manager", true)]
    [InlineData("Director", true)]
    [InlineData("Developer", false)]
    [InlineData("Analyst", false)]
    [InlineData("Designer", false)]
    public void IsManagement_ShouldReturnCorrectValue(string title, bool expectedResult)
    {
        // Arrange
        var jobTitle = JobTitle.Create(title);

        // Act
        var result = jobTitle.IsManagement();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("Chief Executive Officer", true)]
    [InlineData("CEO", true)]
    [InlineData("Chief Technology Officer", true)]
    [InlineData("President", true)]
    [InlineData("Manager", false)]
    [InlineData("Director", false)]
    [InlineData("Developer", false)]
    public void IsExecutive_ShouldReturnCorrectValue(string title, bool expectedResult)
    {
        // Arrange
        var jobTitle = JobTitle.Create(title);

        // Act
        var result = jobTitle.IsExecutive();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("Developer", null, 0, "Developer")]
    [InlineData("Developer", "IT", 0, "Developer, IT")]
    [InlineData("Developer", null, 2, "Developer II")]
    [InlineData("Developer", "IT", 2, "Developer II, IT")]
    [InlineData("Manager", "Finance", 3, "Manager III, Finance")]
    public void ToFullString_ShouldFormatCorrectly(
        string title, string department, int level, string expected)
    {
        // Arrange
        var jobTitle = JobTitle.Create(title, department, level);

        // Act
        var result = jobTitle.ToFullString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_ShouldCallToFullString()
    {
        // Arrange
        var jobTitle = JobTitle.Create("Developer", "IT", 2);

        // Act
        var result = jobTitle.ToString();

        // Assert
        Assert.Equal(jobTitle.ToFullString(), result);
    }

    [Fact]
    public void Equals_WithIdenticalJobTitles_ShouldReturnTrue()
    {
        // Arrange
        var jobTitle1 = JobTitle.Create("Developer", "IT", 2);
        var jobTitle2 = JobTitle.Create("Developer", "IT", 2);

        // Act & Assert
        Assert.True(jobTitle1.Equals(jobTitle2));
    }

    [Fact]
    public void Equals_WithDifferentTitle_ShouldReturnFalse()
    {
        // Arrange
        var jobTitle1 = JobTitle.Create("Developer", "IT", 2);
        var jobTitle2 = JobTitle.Create("Analyst", "IT", 2);

        // Act & Assert
        Assert.False(jobTitle1.Equals(jobTitle2));
    }

    [Fact]
    public void Equals_WithDifferentDepartment_ShouldReturnFalse()
    {
        // Arrange
        var jobTitle1 = JobTitle.Create("Developer", "IT", 2);
        var jobTitle2 = JobTitle.Create("Developer", "Engineering", 2);

        // Act & Assert
        Assert.False(jobTitle1.Equals(jobTitle2));
    }

    [Fact]
    public void Equals_WithDifferentLevel_ShouldReturnFalse()
    {
        // Arrange
        var jobTitle1 = JobTitle.Create("Developer", "IT", 2);
        var jobTitle2 = JobTitle.Create("Developer", "IT", 3);

        // Act & Assert
        Assert.False(jobTitle1.Equals(jobTitle2));
    }

    [Fact]
    public void Equals_WithDifferentCase_ShouldReturnTrue()
    {
        // Arrange
        var jobTitle1 = JobTitle.Create("Developer", "IT", 2);
        var jobTitle2 = JobTitle.Create("DEVELOPER", "it", 2);

        // Act & Assert
        Assert.True(jobTitle1.Equals(jobTitle2));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var jobTitle = JobTitle.Create("Developer", "IT", 2);

        // Act & Assert
        Assert.False(jobTitle.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var jobTitle = JobTitle.Create("Developer", "IT", 2);
        var notJobTitle = "Just a string";

        // Act & Assert
        Assert.False(jobTitle.Equals(notJobTitle));
    }

    [Fact]
    public void GetHashCode_WithIdenticalJobTitles_ShouldReturnSameValue()
    {
        // Arrange
        var jobTitle1 = JobTitle.Create("Developer", "IT", 2);
        var jobTitle2 = JobTitle.Create("Developer", "IT", 2);

        // Act
        var hashCode1 = jobTitle1.GetHashCode();
        var hashCode2 = jobTitle2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentCase_ShouldReturnSameValue()
    {
        // Arrange
        var jobTitle1 = JobTitle.Create("Developer", "IT", 2);
        var jobTitle2 = JobTitle.Create("DEVELOPER", "it", 2);

        // Act
        var hashCode1 = jobTitle1.GetHashCode();
        var hashCode2 = jobTitle2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentJobTitles_ShouldReturnDifferentValues()
    {
        // Arrange
        var jobTitle1 = JobTitle.Create("Developer", "IT", 2);
        var jobTitle2 = JobTitle.Create("Analyst", "Finance", 3);

        // Act
        var hashCode1 = jobTitle1.GetHashCode();
        var hashCode2 = jobTitle2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }
}