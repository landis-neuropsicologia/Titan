using System;
using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class DateRangeTests
{
    [Fact]
    public void Create_WithValidDates_ShouldCreateDateRange()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);

        // Act
        var dateRange = DateRange.Create(start, end);

        // Assert
        Assert.NotNull(dateRange);
        Assert.Equal(start, dateRange.Start);
        Assert.Equal(end, dateRange.End);
        Assert.Equal(TimeSpan.FromDays(30), dateRange.Duration);
    }

    [Fact]
    public void Create_WithEqualDates_ShouldCreateDateRange()
    {
        // Arrange
        var date = new DateTime(2023, 1, 1);

        // Act
        var dateRange = DateRange.Create(date, date);

        // Assert
        Assert.NotNull(dateRange);
        Assert.Equal(date, dateRange.Start);
        Assert.Equal(date, dateRange.End);
        Assert.Equal(TimeSpan.Zero, dateRange.Duration);
    }

    [Fact]
    public void Create_WithEndBeforeStart_ShouldThrowArgumentException()
    {
        // Arrange
        var start = new DateTime(2023, 1, 31);
        var end = new DateTime(2023, 1, 1);
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => DateRange.Create(start, end));
        Assert.Equal("End date must be equal to or greater than start date. (Parameter 'end')", exception.Message);
    }

    [Fact]
    public void CreateFromDuration_WithValidParams_ShouldCreateDateRange()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var duration = TimeSpan.FromDays(30);
        var expectedEnd = new DateTime(2023, 1, 31);

        // Act
        var dateRange = DateRange.CreateFromDuration(start, duration);

        // Assert
        Assert.Equal(start, dateRange.Start);
        Assert.Equal(expectedEnd, dateRange.End);
    }

    [Fact]
    public void CreateFromDuration_WithNegativeDuration_ShouldThrowArgumentException()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var negativeDuration = TimeSpan.FromDays(-10);
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            DateRange.CreateFromDuration(start, negativeDuration));
        Assert.Equal("Duration cannot be negative. (Parameter 'duration')", exception.Message);
    }

    [Fact]
    public void CreateFromDays_WithValidParams_ShouldCreateDateRange()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        const int days = 30;
        var expectedEnd = new DateTime(2023, 1, 31);

        // Act
        var dateRange = DateRange.CreateFromDays(start, days);

        // Assert
        Assert.Equal(start, dateRange.Start);
        Assert.Equal(expectedEnd, dateRange.End);
    }

    [Fact]
    public void CreateFromDays_WithNegativeDays_ShouldThrowArgumentException()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        const int negativeDays = -10;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => DateRange.CreateFromDays(start, negativeDays));

        Assert.Equal("Number of days cannot be negative. (Parameter 'days')", exception.Message);
    }

    [Fact]
    public void CreateToday_ShouldCreateDateRangeForCurrentDay()
    {
        // Act
        var today = DateRange.CreateToday();

        // Assert
        Assert.Equal(DateTime.Today, today.Start);
        Assert.Equal(DateTime.Today.AddDays(1).AddTicks(-1), today.End);
        Assert.Equal(TimeSpan.FromDays(1).Subtract(TimeSpan.FromTicks(1)), today.Duration);
    }

    [Fact]
    public void CreateThisWeek_ShouldCreateDateRangeForCurrentWeek()
    {
        // Arrange
        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

        // Act
        var thisWeek = DateRange.CreateThisWeek();

        // Assert
        Assert.Equal(startOfWeek, thisWeek.Start);
        Assert.Equal(endOfWeek, thisWeek.End);
    }

    [Fact]
    public void CreateThisMonth_ShouldCreateDateRangeForCurrentMonth()
    {
        // Arrange
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

        // Act
        var thisMonth = DateRange.CreateThisMonth();

        // Assert
        Assert.Equal(startOfMonth, thisMonth.Start);
        Assert.Equal(endOfMonth, thisMonth.End);
    }

    [Fact]
    public void CreateThisYear_ShouldCreateDateRangeForCurrentYear()
    {
        // Arrange
        var today = DateTime.Today;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var endOfYear = startOfYear.AddYears(1).AddTicks(-1);

        // Act
        var thisYear = DateRange.CreateThisYear();

        // Assert
        Assert.Equal(startOfYear, thisYear.Start);
        Assert.Equal(endOfYear, thisYear.End);
    }

    [Fact]
    public void Contains_DateTimeWithinRange_ShouldReturnTrue()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);
        var dateRange = DateRange.Create(start, end);
        var dateWithin = new DateTime(2023, 1, 15);

        // Act
        var result = dateRange.Contains(dateWithin);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_DateTimeAtBoundary_ShouldReturnTrue()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);
        var dateRange = DateRange.Create(start, end);

        // Act & Assert
        Assert.True(dateRange.Contains(start));
        Assert.True(dateRange.Contains(end));
    }

    [Fact]
    public void Contains_DateTimeOutsideRange_ShouldReturnFalse()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);
        var dateRange = DateRange.Create(start, end);
        var dateBefore = new DateTime(2022, 12, 31);
        var dateAfter = new DateTime(2023, 2, 1);

        // Act & Assert
        Assert.False(dateRange.Contains(dateBefore));
        Assert.False(dateRange.Contains(dateAfter));
    }

    [Fact]
    public void Contains_DateRangeWithinRange_ShouldReturnTrue()
    {
        // Arrange
        var outerRange = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));
        var innerRange = DateRange.Create(new DateTime(2023, 4, 1), new DateTime(2023, 6, 30));

        // Act
        var result = outerRange.Contains(innerRange);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_DateRangeAtBoundary_ShouldReturnTrue()
    {
        // Arrange
        var outerRange = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));
        var boundaryRange = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));

        // Act
        var result = outerRange.Contains(boundaryRange);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_DateRangePartiallyOutside_ShouldReturnFalse()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 6, 30));
        var range2 = DateRange.Create(new DateTime(2023, 6, 1), new DateTime(2023, 12, 31));

        // Act & Assert
        Assert.False(range1.Contains(range2));
        Assert.False(range2.Contains(range1));
    }

    [Fact]
    public void Contains_DateRangeCompletelyOutside_ShouldReturnFalse()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 3, 31));
        var range2 = DateRange.Create(new DateTime(2023, 4, 1), new DateTime(2023, 6, 30));

        // Act & Assert
        Assert.False(range1.Contains(range2));
        Assert.False(range2.Contains(range1));
    }

    [Fact]
    public void Overlaps_WithOverlappingRanges_ShouldReturnTrue()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 3, 31));
        var range2 = DateRange.Create(new DateTime(2023, 3, 1), new DateTime(2023, 6, 30));

        // Act & Assert
        Assert.True(range1.Overlaps(range2));
        Assert.True(range2.Overlaps(range1));
    }

    [Fact]
    public void Overlaps_WithAdjacentRanges_ShouldReturnTrue()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 3, 31));
        var range2 = DateRange.Create(new DateTime(2023, 3, 31), new DateTime(2023, 6, 30));

        // Act & Assert
        Assert.True(range1.Overlaps(range2));
        Assert.True(range2.Overlaps(range1));
    }

    [Fact]
    public void Overlaps_WithNonOverlappingRanges_ShouldReturnFalse()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 3, 30));
        var range2 = DateRange.Create(new DateTime(2023, 4, 1), new DateTime(2023, 6, 30));

        // Act & Assert
        Assert.False(range1.Overlaps(range2));
        Assert.False(range2.Overlaps(range1));
    }

    [Fact]
    public void Intersect_WithOverlappingRanges_ShouldReturnIntersection()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 3, 31));
        var range2 = DateRange.Create(new DateTime(2023, 3, 1), new DateTime(2023, 6, 30));
        var expected = DateRange.Create(new DateTime(2023, 3, 1), new DateTime(2023, 3, 31));

        // Act
        var intersection = range1.Intersect(range2);

        // Assert
        Assert.Equal(expected, intersection);
    }

    [Fact]
    public void Intersect_WithNonOverlappingRanges_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 3, 31));
        var range2 = DateRange.Create(new DateTime(2023, 4, 1), new DateTime(2023, 6, 30));
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => range1.Intersect(range2));

        Assert.Equal("Cannot intersect non-overlapping date ranges.", exception.Message);
    }

    [Fact]
    public void Union_WithOverlappingRanges_ShouldReturnUnion()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 3, 31));
        var range2 = DateRange.Create(new DateTime(2023, 3, 1), new DateTime(2023, 6, 30));
        var expected = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 6, 30));

        // Act
        var union = range1.Union(range2);

        // Assert
        Assert.Equal(expected, union);
    }

    [Fact]
    public void Union_WithNonOverlappingRanges_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 3, 31));
        var range2 = DateRange.Create(new DateTime(2023, 4, 1), new DateTime(2023, 6, 30));
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => range1.Union(range2));

        Assert.Equal("Cannot union non-overlapping date ranges.", exception.Message);
    }

    [Fact]
    public void Split_WithValidInterval_ShouldReturnCorrectRanges()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 5);
        var dateRange = DateRange.Create(start, end);
        var interval = TimeSpan.FromDays(1);

        // Act
        var splitRanges = dateRange.Split(interval);

        // Assert
        Assert.Equal(4, splitRanges.Length);


        Assert.Equal(new DateTime(2023, 1, 1), splitRanges[0].Start);
        Assert.Equal(new DateTime(2023, 1, 1, 23, 59, 59, 999, 999).AddTicks(9), splitRanges[0].End);

        Assert.Equal(new DateTime(2023, 1, 2), splitRanges[1].Start);
        Assert.Equal(new DateTime(2023, 1, 2, 23, 59, 59, 999, 999).AddTicks(9), splitRanges[1].End);

        Assert.Equal(new DateTime(2023, 1, 3), splitRanges[2].Start);
        Assert.Equal(new DateTime(2023, 1, 3, 23, 59, 59, 999, 999).AddTicks(9), splitRanges[2].End);

        Assert.Equal(new DateTime(2023, 1, 4), splitRanges[3].Start);
        Assert.Equal(new DateTime(2023, 1, 5), splitRanges[3].End);
    }

    [Fact]
    public void Split_WithNegativeInterval_ShouldThrowArgumentException()
    {
        // Arrange
        var dateRange = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
        var negativeInterval = TimeSpan.FromDays(-1);
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => dateRange.Split(negativeInterval));

        Assert.Equal("Interval must be positive. (Parameter 'interval')", exception.Message);
    }

    [Fact]
    public void Format_WithDefaultFormat_ShouldReturnFormattedString()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);
        var dateRange = DateRange.Create(start, end);

        // Act
        var formatted = dateRange.Format();

        // Assert
        Assert.Equal($"{start.ToString("G")} - {end.ToString("G")}", formatted);
    }

    [Fact]
    public void Format_WithCustomFormat_ShouldReturnFormattedString()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);
        var dateRange = DateRange.Create(start, end);
        const string format = "yyyy-MM-dd";

        // Act
        var formatted = dateRange.Format(format);

        // Assert
        Assert.Equal($"{start.ToString(format)} - {end.ToString(format)}", formatted);
    }

    [Fact]
    public void Format_WithSameDayRange_ShouldReturnSingleDate()
    {
        // Arrange
        var date = new DateTime(2023, 1, 1);
        var dateRange = DateRange.Create(date, date);

        // Act
        var formatted = dateRange.Format();

        // Assert
        Assert.Equal(date.ToString("G"), formatted);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);
        var dateRange = DateRange.Create(start, end);

        // Act
        var result = dateRange.ToString();

        // Assert
        Assert.Equal(dateRange.Format(), result);
    }

    [Fact]
    public void Equals_WithIdenticalDateRanges_ShouldReturnTrue()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);
        var range1 = DateRange.Create(start, end);
        var range2 = DateRange.Create(start, end);

        // Act & Assert
        Assert.True(range1.Equals(range2));
    }

    [Fact]
    public void Equals_WithDifferentDateRanges_ShouldReturnFalse()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
        var range2 = DateRange.Create(new DateTime(2023, 2, 1), new DateTime(2023, 2, 28));

        // Act & Assert
        Assert.False(range1.Equals(range2));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var dateRange = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));

        // Act & Assert
        Assert.False(dateRange.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var dateRange = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
        var notDateRange = "Just a string";

        // Act & Assert
        Assert.False(dateRange.Equals(notDateRange));
    }

    [Fact]
    public void GetHashCode_WithIdenticalDateRanges_ShouldReturnSameValue()
    {
        // Arrange
        var start = new DateTime(2023, 1, 1);
        var end = new DateTime(2023, 1, 31);
        var range1 = DateRange.Create(start, end);
        var range2 = DateRange.Create(start, end);

        // Act
        var hashCode1 = range1.GetHashCode();
        var hashCode2 = range2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentDateRanges_ShouldReturnDifferentValues()
    {
        // Arrange
        var range1 = DateRange.Create(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
        var range2 = DateRange.Create(new DateTime(2023, 2, 1), new DateTime(2023, 2, 28));

        // Act
        var hashCode1 = range1.GetHashCode();
        var hashCode2 = range2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }
}