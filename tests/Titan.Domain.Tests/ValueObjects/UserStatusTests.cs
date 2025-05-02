using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public class UserStatusTests
{
    [Fact]
    public void PredefinedStatuses_ShouldHaveCorrectTypes()
    {
        // Act & Assert
        Assert.Equal(UserStatus.StatusType.Pending, UserStatus.Pending.Type);
        Assert.Equal(UserStatus.StatusType.Active, UserStatus.Active.Type);
        Assert.Equal(UserStatus.StatusType.Inactive, UserStatus.Inactive.Type);
        Assert.Equal(UserStatus.StatusType.Suspended, UserStatus.Suspended.Type);
        Assert.Equal(UserStatus.StatusType.Locked, UserStatus.Locked.Type);
        Assert.Equal(UserStatus.StatusType.Deleted, UserStatus.Deleted.Type);
    }

    [Fact]
    public void Create_WithValidParams_ShouldCreateUserStatus()
    {
        // Arrange
        const string reason = "Violation of terms";

        var type = UserStatus.StatusType.Suspended;
        var expiresAt = DateTime.UtcNow.AddDays(7);

        // Act
        var status = UserStatus.Create(type, reason, expiresAt);

        // Assert
        Assert.NotNull(status);
        Assert.Equal(type, status.Type);
        Assert.Equal(reason, status.Reason);
        Assert.Equal(expiresAt, status.ExpiresAt);
        Assert.True((DateTime.UtcNow - status.CreatedAt).TotalSeconds < 10);
    }

    [Fact]
    public void Create_WithoutOptionalParams_ShouldReturnPredefinedInstance()
    {
        // Act
        var status = UserStatus.Create(UserStatus.StatusType.Active);

        // Assert
        Assert.Same(UserStatus.Active, status);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Pending)]
    [InlineData(UserStatus.StatusType.Active)]
    [InlineData(UserStatus.StatusType.Inactive)]
    [InlineData(UserStatus.StatusType.Suspended)]
    [InlineData(UserStatus.StatusType.Locked)]
    [InlineData(UserStatus.StatusType.Deleted)]
    public void Create_WithoutOptionalParams_ShouldReturnCorrectPredefinedInstance(UserStatus.StatusType type)
    {
        // Act
        var status = UserStatus.Create(type);

        // Assert
        Assert.Equal(type, status.Type);
        Assert.Null(status.Reason);
        Assert.Null(status.ExpiresAt);
    }

    [Fact]
    public void CreateSuspended_WithValidParams_ShouldCreateSuspendedStatus()
    {
        // Arrange
        const string reason = "Violation of terms";

        var expiresAt = DateTime.UtcNow.AddDays(7);

        // Act
        var status = UserStatus.CreateSuspended(reason, expiresAt);

        // Assert
        Assert.Equal(UserStatus.StatusType.Suspended, status.Type);
        Assert.Equal(reason, status.Reason);
        Assert.Equal(expiresAt, status.ExpiresAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateSuspended_WithoutReason_ShouldThrowArgumentException(string emptyReason)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => UserStatus.CreateSuspended(emptyReason));

        Assert.Contains("Reason is required for suspended status", exception.Message);
    }

    [Fact]
    public void CreateLocked_WithValidParams_ShouldCreateLockedStatus()
    {
        // Arrange
        const string reason = "Too many login attempts";

        var expiresAt = DateTime.UtcNow.AddHours(1);

        // Act
        var status = UserStatus.CreateLocked(reason, expiresAt);

        // Assert
        Assert.Equal(UserStatus.StatusType.Locked, status.Type);
        Assert.Equal(reason, status.Reason);
        Assert.Equal(expiresAt, status.ExpiresAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateLocked_WithoutReason_ShouldThrowArgumentException(string emptyReason)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => UserStatus.CreateLocked(emptyReason));

        Assert.Contains("Reason is required for locked status", exception.Message);
    }

    [Fact]
    public void CreateDeleted_WithValidReason_ShouldCreateDeletedStatus()
    {
        // Arrange
        const string reason = "User requested deletion";

        // Act
        var status = UserStatus.CreateDeleted(reason);

        // Assert
        Assert.Equal(UserStatus.StatusType.Deleted, status.Type);
        Assert.Equal(reason, status.Reason);
        Assert.Null(status.ExpiresAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateDeleted_WithoutReason_ShouldThrowArgumentException(string emptyReason)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => UserStatus.CreateDeleted(emptyReason));

        Assert.Contains("Reason is required for deleted status", exception.Message);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Active, true)]
    [InlineData(UserStatus.StatusType.Pending, false)]
    [InlineData(UserStatus.StatusType.Inactive, false)]
    [InlineData(UserStatus.StatusType.Suspended, false)]
    [InlineData(UserStatus.StatusType.Locked, false)]
    [InlineData(UserStatus.StatusType.Deleted, false)]
    public void IsActive_ShouldReturnCorrectValue(UserStatus.StatusType type, bool expectedResult)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.IsActive();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Pending, true)]
    [InlineData(UserStatus.StatusType.Active, false)]
    [InlineData(UserStatus.StatusType.Inactive, false)]
    [InlineData(UserStatus.StatusType.Suspended, false)]
    [InlineData(UserStatus.StatusType.Locked, false)]
    [InlineData(UserStatus.StatusType.Deleted, false)]
    public void IsPending_ShouldReturnCorrectValue(UserStatus.StatusType type, bool expectedResult)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.IsPending();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Inactive, true)]
    [InlineData(UserStatus.StatusType.Pending, false)]
    [InlineData(UserStatus.StatusType.Active, false)]
    [InlineData(UserStatus.StatusType.Suspended, false)]
    [InlineData(UserStatus.StatusType.Locked, false)]
    [InlineData(UserStatus.StatusType.Deleted, false)]
    public void IsInactive_ShouldReturnCorrectValue(UserStatus.StatusType type, bool expectedResult)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.IsInactive();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Suspended, true)]
    [InlineData(UserStatus.StatusType.Pending, false)]
    [InlineData(UserStatus.StatusType.Active, false)]
    [InlineData(UserStatus.StatusType.Inactive, false)]
    [InlineData(UserStatus.StatusType.Locked, false)]
    [InlineData(UserStatus.StatusType.Deleted, false)]
    public void IsSuspended_ShouldReturnCorrectValue(UserStatus.StatusType type, bool expectedResult)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.IsSuspended();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Locked, true)]
    [InlineData(UserStatus.StatusType.Pending, false)]
    [InlineData(UserStatus.StatusType.Active, false)]
    [InlineData(UserStatus.StatusType.Inactive, false)]
    [InlineData(UserStatus.StatusType.Suspended, false)]
    [InlineData(UserStatus.StatusType.Deleted, false)]
    public void IsLocked_ShouldReturnCorrectValue(UserStatus.StatusType type, bool expectedResult)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.IsLocked();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Deleted, true)]
    [InlineData(UserStatus.StatusType.Pending, false)]
    [InlineData(UserStatus.StatusType.Active, false)]
    [InlineData(UserStatus.StatusType.Inactive, false)]
    [InlineData(UserStatus.StatusType.Suspended, false)]
    [InlineData(UserStatus.StatusType.Locked, false)]
    public void IsDeleted_ShouldReturnCorrectValue(UserStatus.StatusType type, bool expectedResult)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.IsDeleted();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsExpired_WithFutureExpiration_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(1);
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Reason", expiresAt);

        // Act
        var result = status.IsExpired();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsExpired_WithPastExpiration_ShouldReturnTrue()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Reason", expiresAt);

        // Act
        var result = status.IsExpired();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsExpired_WithoutExpiration_ShouldReturnFalse()
    {
        // Arrange
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Reason");

        // Act
        var result = status.IsExpired();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Active, true)]
    [InlineData(UserStatus.StatusType.Pending, true)]
    [InlineData(UserStatus.StatusType.Inactive, false)]
    [InlineData(UserStatus.StatusType.Suspended, false)]
    [InlineData(UserStatus.StatusType.Locked, false)]
    [InlineData(UserStatus.StatusType.Deleted, false)]
    public void IsActiveOrPending_ShouldReturnCorrectValue(UserStatus.StatusType type, bool expectedResult)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.IsActiveOrPending();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CanLogin_WithActiveUser_ShouldReturnTrue()
    {
        // Arrange
        var status = UserStatus.Active;

        // Act
        var result = status.CanLogin();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanLogin_WithExpiredActiveUser_ShouldReturnFalse()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var status = UserStatus.Create(UserStatus.StatusType.Active, null, expiresAt);

        // Act
        var result = status.CanLogin();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Pending)]
    [InlineData(UserStatus.StatusType.Inactive)]
    [InlineData(UserStatus.StatusType.Suspended)]
    [InlineData(UserStatus.StatusType.Locked)]
    [InlineData(UserStatus.StatusType.Deleted)]
    public void CanLogin_WithNonActiveUser_ShouldReturnFalse(UserStatus.StatusType type)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.CanLogin();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanBeActivated_WithPendingUser_ShouldReturnTrue()
    {
        // Arrange
        var status = UserStatus.Pending;

        // Act
        var result = status.CanBeActivated();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Active)]
    [InlineData(UserStatus.StatusType.Inactive)]
    [InlineData(UserStatus.StatusType.Suspended)]
    [InlineData(UserStatus.StatusType.Locked)]
    [InlineData(UserStatus.StatusType.Deleted)]
    public void CanBeActivated_WithNonPendingUser_ShouldReturnFalse(UserStatus.StatusType type)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.CanBeActivated();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanBeUnlocked_WithLockedUser_ShouldReturnTrue()
    {
        // Arrange
        var status = UserStatus.Locked;

        // Act
        var result = status.CanBeUnlocked();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Pending)]
    [InlineData(UserStatus.StatusType.Active)]
    [InlineData(UserStatus.StatusType.Inactive)]
    [InlineData(UserStatus.StatusType.Suspended)]
    [InlineData(UserStatus.StatusType.Deleted)]
    public void CanBeUnlocked_WithNonLockedUser_ShouldReturnFalse(UserStatus.StatusType type)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.CanBeUnlocked();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Active, true)]
    [InlineData(UserStatus.StatusType.Pending, true)]
    [InlineData(UserStatus.StatusType.Inactive, false)]
    [InlineData(UserStatus.StatusType.Suspended, false)]
    [InlineData(UserStatus.StatusType.Locked, false)]
    [InlineData(UserStatus.StatusType.Deleted, false)]
    public void CanBeSuspended_ShouldReturnCorrectValue(UserStatus.StatusType type, bool expectedResult)
    {
        // Arrange
        var status = UserStatus.Create(type);

        // Act
        var result = status.CanBeSuspended();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void WithReason_ShouldSetReason()
    {
        // Arrange
        var status = UserStatus.Active;
        const string reason = "Testing";

        // Act
        var newStatus = status.WithReason(reason);

        // Assert
        Assert.Equal(UserStatus.StatusType.Active, newStatus.Type);
        Assert.Equal(reason, newStatus.Reason);
        Assert.Null(newStatus.ExpiresAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void WithReason_WithEmptyReason_ShouldThrowArgumentException(string emptyReason)
    {
        // Arrange
        var status = UserStatus.Active;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => status.WithReason(emptyReason));

        Assert.Contains("Reason cannot be empty", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void WithReason_WithEmptyReasonAndPortugueseCulture_ShouldThrowArgumentExceptionInPortuguese(string emptyReason)
    {
        // Arrange
        var status = UserStatus.Active;
        var portugueseCulture = new CultureInfo("pt-BR");

        CultureInfo.CurrentCulture = portugueseCulture;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => status.WithReason(emptyReason));

        Assert.Contains("O campo Reason não pode ser vazio.", exception.Message);
    }

    [Fact]
    public void WithExpiration_ShouldSetExpiration()
    {
        // Arrange
        var status = UserStatus.Suspended;
        var expiresAt = DateTime.UtcNow.AddDays(7);

        // Act
        var newStatus = status.WithExpiration(expiresAt);

        // Assert
        Assert.Equal(UserStatus.StatusType.Suspended, newStatus.Type);
        Assert.Equal(expiresAt, newStatus.ExpiresAt);
        Assert.Null(newStatus.Reason);
    }

    [Fact]
    public void WithExpiration_WithPortugueseCulture_ShouldReturnLocalizedString()
    {
        // Arrange
        var status = UserStatus.Suspended;
        var expiresAt = DateTime.UtcNow.AddDays(-7);
        var portugueseCulture = new CultureInfo("pt-BR");

        CultureInfo.CurrentCulture = portugueseCulture;

        // Act
        var exception = Assert.Throws<ArgumentException>(() => status.WithExpiration(expiresAt));

        // Assert
        Assert.Contains("Data de Expiração precisa ser maior que Data Atual", exception.Message);
    }

    [Fact]
    public void WithExpiration_WithPastDate_ShouldThrowArgumentException()
    {
        // Arrange
        var status = UserStatus.Suspended;
        var pastDate = DateTime.UtcNow.AddDays(-1);
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => status.WithExpiration(pastDate));

        Assert.Contains("Expiration date must be in the future", exception.Message);
    }

    [Fact]
    public void WithoutExpiration_ShouldRemoveExpiration()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Reason", expiresAt);

        // Act
        var newStatus = status.WithoutExpiration();

        // Assert
        Assert.Equal(UserStatus.StatusType.Suspended, newStatus.Type);
        Assert.Equal("Reason", newStatus.Reason);
        Assert.Null(newStatus.ExpiresAt);
    }

    [Theory]
    [InlineData(UserStatus.StatusType.Active, null, null, "Active")]
    [InlineData(UserStatus.StatusType.Suspended, "Violation", null, "Suspended (Violation)")]
    public void GetDescription_WithoutExpiration_ShouldReturnCorrectString(UserStatus.StatusType type, string reason, DateTime? expiresAt, string expected)
    {
        // Arrange
        var status = UserStatus.Create(type, reason, expiresAt);
        var englishCulture = new CultureInfo("en-US");

        // Act
        var description = status.GetDescription(englishCulture);

        // Assert
        Assert.Equal(expected, description);
    }

    [Fact]
    public void GetDescription_WithPortugueseCulture_ShouldReturnLocalizedString()
    {
        // Arrange
        var status = UserStatus.Create(UserStatus.StatusType.Active);
        var portugueseCulture = new CultureInfo("pt-BR");

        // Act
        var description = status.GetDescription(portugueseCulture);

        // Assert
        Assert.Equal("Ativo", description);
    }

    [Fact]
    public void GetDescription_WithFutureExpiration_ShouldIncludeExpiration()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", expiresAt);
        var englishCulture = new CultureInfo("en-US");

        // Act
        var description = status.GetDescription(englishCulture);

        // Assert
        Assert.Contains("Suspended (Violation) - Expires at", description);
        Assert.Contains(expiresAt.ToString("g", englishCulture), description);
    }

    [Fact]
    public void GetDescription_WithFutureExpirationInPortuguese_ShouldIncludeLocalizedExpiration()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", expiresAt);
        var portugueseCulture = new CultureInfo("pt-BR");

        // Act
        var description = status.GetDescription(portugueseCulture);

        // Assert
        Assert.Contains("Suspenso (Violation) - Expira em", description);
        Assert.Contains(expiresAt.ToString("g", portugueseCulture), description);
    }

    [Fact]
    public void GetDescription_WithPastExpiration_ShouldShowExpired()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", expiresAt);
        var englishCulture = new System.Globalization.CultureInfo("en-US");

        // Act
        var description = status.GetDescription(englishCulture);

        // Assert
        Assert.Equal("Suspended (Violation) - Expired", description);
    }

    [Fact]
    public void GetDescription_WithPastExpirationInPortuguese_ShouldShowLocalizedExpired()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", expiresAt);
        var portugueseCulture = new System.Globalization.CultureInfo("pt-BR");

        // Act
        var description = status.GetDescription(portugueseCulture);

        // Assert
        Assert.Equal("Suspenso (Violation) - Expirado", description);
    }

    [Fact]
    public void ToString_ShouldReturnDescriptionWithCurrentCulture()
    {
        // Arrange
        var status = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation");

        // Act
        var result = status.ToString();

        // Assert
        Assert.Equal(status.GetDescription(System.Globalization.CultureInfo.CurrentCulture), result);
    }

    [Fact]
    public void Equals_WithIdenticalStatuses_ShouldReturnTrue()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var status1 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", expiresAt);
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", expiresAt);

        // Act & Assert
        Assert.True(status1.Equals(status2));
    }

    [Fact]
    public void Equals_WithDifferentTypes_ShouldReturnFalse()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Active);
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended);

        // Act & Assert
        Assert.False(status1.Equals(status2));
    }

    [Fact]
    public void Equals_WithDifferentReasons_ShouldReturnFalse()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation 1");
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation 2");

        // Act & Assert
        Assert.False(status1.Equals(status2));
    }

    [Fact]
    public void Equals_WithDifferentExpirations_ShouldReturnFalse()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", DateTime.UtcNow.AddDays(1));
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", DateTime.UtcNow.AddDays(2));

        // Act & Assert
        Assert.False(status1.Equals(status2));
    }

    [Fact]
    public void Equals_WithOneNullExpiration_ShouldReturnFalse()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation");
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation", DateTime.UtcNow.AddDays(7));

        // Act & Assert
        Assert.False(status1.Equals(status2));
    }

    [Fact]
    public void Equals_WithBothNullExpirations_ShouldCompareOtherProperties()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation");
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation");

        // Act & Assert
        Assert.True(status1.Equals(status2));
    }

    [Fact]
    public void Equals_WithCaseInsensitiveReasons_ShouldReturnTrue()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Suspended, "violation");
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "VIOLATION");

        // Act & Assert
        Assert.True(status1.Equals(status2));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var status = UserStatus.Create(UserStatus.StatusType.Active);

        // Act & Assert
        Assert.False(status.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var status = UserStatus.Create(UserStatus.StatusType.Active);
        var notStatus = "Just a string";

        // Act & Assert
        Assert.False(status.Equals(notStatus));
    }

    [Fact]
    public void GetHashCode_WithIdenticalStatuses_ShouldReturnSameValue()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation");
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation");

        // Act
        var hashCode1 = status1.GetHashCode();
        var hashCode2 = status2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithCaseInsensitiveReasons_ShouldReturnSameValue()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Suspended, "violation");
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "VIOLATION");

        // Act
        var hashCode1 = status1.GetHashCode();
        var hashCode2 = status2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentStatuses_ShouldReturnDifferentValues()
    {
        // Arrange
        var status1 = UserStatus.Create(UserStatus.StatusType.Active);
        var status2 = UserStatus.Create(UserStatus.StatusType.Suspended, "Violation");

        // Act
        var hashCode1 = status1.GetHashCode();
        var hashCode2 = status2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }
}