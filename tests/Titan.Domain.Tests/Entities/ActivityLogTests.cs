using System.Globalization;
using Titan.Domain.Entities;

namespace Titan.Domain.Tests.Entities;

public sealed class ActivityLogTests
{
    [Fact]
    public void Constructor_WithValidParams_ShouldCreateActivityLog()
    {
        // Arrange
        var userId = Guid.NewGuid();
        const string action = "Login";
        const string description = "User logged in";
        const string ipAddress = "192.168.1.1";

        // Act
        var activityLog = new ActivityLog(userId, action, description, ipAddress);

        // Assert
        Assert.Equal(userId, activityLog.UserId);
        Assert.Equal(action, activityLog.Action);
        Assert.Equal(description, activityLog.Description);
        Assert.Equal(ipAddress, activityLog.IpAddress);
        Assert.True((DateTime.UtcNow - activityLog.Timestamp).TotalSeconds < 10);
    }

    [Fact]
    public void Constructor_WithEmptyUserId_ShouldThrowArgumentException()
    {
        // Arrange
        var userId = Guid.Empty;
        const string action = "Login";
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ActivityLog(userId, action));

        Assert.Equal("User ID cannot be empty. (Parameter 'userId')", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_WithInvalidAction_ShouldThrowArgumentException(string action)
    {
        // Arrange
        var userId = Guid.NewGuid();
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ActivityLog(userId, action));

        Assert.Equal("Action cannot be null or empty. (Parameter 'action')", exception.Message);
    }
}
