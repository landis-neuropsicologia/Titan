using System.Globalization;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Tests.ValueObjects;

public sealed class CompositeRoleNameTests
{
    [Fact]
    public void Create_WithValidParams_ShouldCreateCompositeRoleName()
    {
        // Arrange
        var roleNames = new[] { "admin", "editor", "viewer" };

        // Act
        var compositeRole = CompositeRoleName.Create(roleNames);

        // Assert
        Assert.NotNull(compositeRole);
        Assert.Equal(3, compositeRole.RoleNames.Count);
        Assert.Contains("admin", compositeRole.RoleNames);
        Assert.Contains("editor", compositeRole.RoleNames);
        Assert.Contains("viewer", compositeRole.RoleNames);
    }

    [Fact]
    public void Create_WithDuplicateRoles_ShouldDeduplicateRoles()
    {
        // Arrange
        var roleNames = new[] { "admin", "editor", "admin", "viewer", "EDITOR" };

        // Act
        var compositeRole = CompositeRoleName.Create(roleNames);

        // Assert
        Assert.Equal(3, compositeRole.RoleNames.Count);
        Assert.Contains("admin", compositeRole.RoleNames, StringComparer.OrdinalIgnoreCase);
        Assert.Contains("editor", compositeRole.RoleNames, StringComparer.OrdinalIgnoreCase);
        Assert.Contains("viewer", compositeRole.RoleNames, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimRoleNames()
    {
        // Arrange
        var roleNames = new[] { " admin ", "  editor", "viewer  " };

        // Act
        var compositeRole = CompositeRoleName.Create(roleNames);

        // Assert
        Assert.Equal(3, compositeRole.RoleNames.Count);
        Assert.Contains("admin", compositeRole.RoleNames);
        Assert.Contains("editor", compositeRole.RoleNames);
        Assert.Contains("viewer", compositeRole.RoleNames);
    }

    [Fact]
    public void Create_WithNullParams_ShouldThrowArgumentException()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CompositeRoleName.Create(null));

        Assert.Equal("At least one role name must be provided. (Parameter 'roleNames')", exception.Message);
    }

    [Fact]
    public void Create_WithEmptyArrayParams_ShouldThrowArgumentException()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var emptyArray = Array.Empty<string>();
        var exception = Assert.Throws<ArgumentException>(() => CompositeRoleName.Create(emptyArray));

        Assert.Equal("At least one role name must be provided. (Parameter 'roleNames')", exception.Message);
    }

    [Fact]
    public void Create_WithEmptyRoleNames_ShouldThrowArgumentException()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CompositeRoleName.Create(""));

        Assert.Equal("Role name cannot be empty. (Parameter 'roleNames')", exception.Message);
    }

    [Fact]
    public void Create_WithWhitespaceRoleNames_ShouldThrowArgumentException()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CompositeRoleName.Create("  "));

        Assert.Equal("Role name cannot be empty. (Parameter 'roleNames')", exception.Message);
    }

    [Fact]
    public void Create_WithMixedValidAndEmptyRoleNames_ShouldThrowArgumentException()
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CompositeRoleName.Create("admin", ""));

        Assert.Equal("Role name cannot be empty. (Parameter 'roleNames')", exception.Message);
    }

    [Fact]
    public void Create_ShouldSortRoleNames()
    {
        // Arrange
        var roleNames = new[] { "viewer", "admin", "editor" };

        // Act
        var compositeRole = CompositeRoleName.Create(roleNames);

        // Assert
        Assert.Equal("admin", compositeRole.RoleNames[0]);
        Assert.Equal("editor", compositeRole.RoleNames[1]);
        Assert.Equal("viewer", compositeRole.RoleNames[2]);
    }

    [Fact]
    public void Parse_WithValidString_ShouldCreateCompositeRoleName()
    {
        // Arrange
        var roleString = "admin|editor|viewer";

        // Act
        var compositeRole = CompositeRoleName.Parse(roleString);

        // Assert
        Assert.Equal(3, compositeRole.RoleNames.Count);
        Assert.Contains("admin", compositeRole.RoleNames);
        Assert.Contains("editor", compositeRole.RoleNames);
        Assert.Contains("viewer", compositeRole.RoleNames);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_WithEmptyString_ShouldThrowArgumentException(string emptyString)
    {
        // Arrange
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CompositeRoleName.Parse(emptyString));

        Assert.Equal("Composite role name cannot be empty. (Parameter 'compositeName')", exception.Message);
    }

    [Fact]
    public void Parse_WithOnlyDividers_ShouldThrowArgumentException()
    {
        // Arrange
        var roleString = "|||";
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CompositeRoleName.Parse(roleString));

        Assert.Equal("At least one role name must be provided. (Parameter 'compositeName')", exception.Message);
    }

    [Fact]
    public void Contains_WithExistingRole_ShouldReturnTrue()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.True(compositeRole.Contains("admin"));
        Assert.True(compositeRole.Contains("editor"));
        Assert.True(compositeRole.Contains("viewer"));
    }

    [Fact]
    public void Contains_WithDifferentCase_ShouldReturnTrue()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.True(compositeRole.Contains("ADMIN"));
        Assert.True(compositeRole.Contains("Editor"));
        Assert.True(compositeRole.Contains("viEWer"));
    }

    [Fact]
    public void Contains_WithWhitespace_ShouldTrimAndCheck()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.True(compositeRole.Contains(" admin "));
        Assert.True(compositeRole.Contains("  editor"));
        Assert.True(compositeRole.Contains("viewer  "));
    }

    [Fact]
    public void Contains_WithNonExistingRole_ShouldReturnFalse()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.False(compositeRole.Contains("manager"));
        Assert.False(compositeRole.Contains("owner"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Contains_WithEmptyRole_ShouldReturnFalse(string emptyRole)
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.False(compositeRole.Contains(emptyRole));
    }

    [Fact]
    public void ContainsAny_WithExistingRoles_ShouldReturnTrue()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.True(compositeRole.ContainsAny("admin", "manager"));
        Assert.True(compositeRole.ContainsAny("owner", "editor"));
        Assert.True(compositeRole.ContainsAny("viewer"));
    }

    [Fact]
    public void ContainsAny_WithNoExistingRoles_ShouldReturnFalse()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.False(compositeRole.ContainsAny("manager", "owner"));
    }

    [Fact]
    public void ContainsAny_WithNullParams_ShouldReturnFalse()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.False(compositeRole.ContainsAny(null));
    }

    [Fact]
    public void ContainsAny_WithEmptyArrayParams_ShouldReturnFalse()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");
        var emptyArray = Array.Empty<string>();

        // Act & Assert
        Assert.False(compositeRole.ContainsAny(emptyArray));
    }

    [Fact]
    public void ContainsAll_WithAllExistingRoles_ShouldReturnTrue()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.True(compositeRole.ContainsAll("admin"));
        Assert.True(compositeRole.ContainsAll("admin", "editor"));
        Assert.True(compositeRole.ContainsAll("admin", "editor", "viewer"));
    }

    [Fact]
    public void ContainsAll_WithSomeExistingRoles_ShouldReturnFalse()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.False(compositeRole.ContainsAll("admin", "manager"));
        Assert.False(compositeRole.ContainsAll("owner", "editor", "viewer"));
    }

    [Fact]
    public void ContainsAll_WithNullParams_ShouldReturnTrue()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.True(compositeRole.ContainsAll(null));
    }

    [Fact]
    public void ContainsAll_WithEmptyArrayParams_ShouldReturnTrue()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");
        var emptyArray = Array.Empty<string>();

        // Act & Assert
        Assert.True(compositeRole.ContainsAll(emptyArray));
    }

    [Fact]
    public void Add_WithNewRole_ShouldAddRole()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor");

        // Act
        var newCompositeRole = compositeRole.Add("viewer");

        // Assert
        Assert.Equal(3, newCompositeRole.RoleNames.Count);
        Assert.Contains("admin", newCompositeRole.RoleNames);
        Assert.Contains("editor", newCompositeRole.RoleNames);
        Assert.Contains("viewer", newCompositeRole.RoleNames);
    }

    [Fact]
    public void Add_WithExistingRole_ShouldReturnUnchangedCompositeRole()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act
        var newCompositeRole = compositeRole.Add("admin");

        // Assert
        Assert.Equal(compositeRole, newCompositeRole);
        Assert.Equal(3, newCompositeRole.RoleNames.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Add_WithEmptyRole_ShouldThrowArgumentException(string emptyRole)
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor");
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => compositeRole.Add(emptyRole));

        Assert.Equal("Role name cannot be empty. (Parameter 'roleName')", exception.Message);
    }

    [Fact]
    public void Remove_WithExistingRole_ShouldRemoveRole()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act
        var newCompositeRole = compositeRole.Remove("editor");

        // Assert
        Assert.Equal(2, newCompositeRole.RoleNames.Count);
        Assert.Contains("admin", newCompositeRole.RoleNames);
        Assert.Contains("viewer", newCompositeRole.RoleNames);
        Assert.DoesNotContain("editor", newCompositeRole.RoleNames);
    }

    [Fact]
    public void Remove_WithNonExistingRole_ShouldReturnUnchangedCompositeRole()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act
        var newCompositeRole = compositeRole.Remove("manager");

        // Assert
        Assert.Equal(compositeRole, newCompositeRole);
        Assert.Equal(3, newCompositeRole.RoleNames.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Remove_WithEmptyRole_ShouldReturnUnchangedCompositeRole(string emptyRole)
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act
        var newCompositeRole = compositeRole.Remove(emptyRole);

        // Assert
        Assert.Equal(compositeRole, newCompositeRole);
        Assert.Equal(3, newCompositeRole.RoleNames.Count);
    }

    [Fact]
    public void Remove_LastRole_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin");
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => compositeRole.Remove("admin"));

        Assert.Equal("Cannot remove the last role from a composite role.", exception.Message);
    }

    [Fact]
    public void Union_WithDifferentRoles_ShouldCombineRoles()
    {
        // Arrange
        var compositeRole1 = CompositeRoleName.Create("admin", "editor");
        var compositeRole2 = CompositeRoleName.Create("viewer", "manager");

        // Act
        var unionRole = compositeRole1.Union(compositeRole2);

        // Assert
        Assert.Equal(4, unionRole.RoleNames.Count);
        Assert.Contains("admin", unionRole.RoleNames);
        Assert.Contains("editor", unionRole.RoleNames);
        Assert.Contains("viewer", unionRole.RoleNames);
        Assert.Contains("manager", unionRole.RoleNames);
    }

    [Fact]
    public void Union_WithOverlappingRoles_ShouldDeduplicateRoles()
    {
        // Arrange
        var compositeRole1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var compositeRole2 = CompositeRoleName.Create("viewer", "manager", "ADMIN");

        // Act
        var unionRole = compositeRole1.Union(compositeRole2);

        // Assert
        Assert.Equal(4, unionRole.RoleNames.Count);
        Assert.Contains("admin", unionRole.RoleNames, StringComparer.OrdinalIgnoreCase);
        Assert.Contains("editor", unionRole.RoleNames);
        Assert.Contains("viewer", unionRole.RoleNames);
        Assert.Contains("manager", unionRole.RoleNames);
    }

    [Fact]
    public void Union_WithNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => compositeRole.Union(null));
    }

    [Fact]
    public void Intersect_WithOverlappingRoles_ShouldReturnCommonRoles()
    {
        // Arrange
        var compositeRole1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var compositeRole2 = CompositeRoleName.Create("viewer", "editor", "manager");

        // Act
        var intersectionRole = compositeRole1.Intersect(compositeRole2);

        // Assert
        Assert.Equal(2, intersectionRole.RoleNames.Count);
        Assert.Contains("editor", intersectionRole.RoleNames);
        Assert.Contains("viewer", intersectionRole.RoleNames);
    }

    [Fact]
    public void Intersect_WithNoCommonRoles_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var compositeRole1 = CompositeRoleName.Create("admin", "editor");
        var compositeRole2 = CompositeRoleName.Create("viewer", "manager");
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => compositeRole1.Intersect(compositeRole2));

        Assert.Equal("Role intersection resulted in empty set.", exception.Message);
    }

    [Fact]
    public void Intersect_WithNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => compositeRole.Intersect(null));
    }

    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");
        var expected = "admin|editor|viewer";

        // Act
        var serialized = compositeRole.Serialize();

        // Assert
        Assert.Equal(expected, serialized);
    }

    [Fact]
    public void ToString_ShouldCallSerialize()
    {
        // Arrange
        var compositeRole = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act
        var result = compositeRole.ToString();

        // Assert
        Assert.Equal(compositeRole.Serialize(), result);
    }

    [Fact]
    public void Equals_WithIdenticalRoles_ShouldReturnTrue()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act & Assert
        Assert.True(role1.Equals(role2));
    }

    [Fact]
    public void Equals_WithDifferentOrder_ShouldReturnTrue()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("viewer", "admin", "editor");

        // Act & Assert
        Assert.True(role1.Equals(role2));
    }

    [Fact]
    public void Equals_WithDifferentCase_ShouldReturnTrue()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("ADMIN", "Editor", "Viewer");

        // Act & Assert
        Assert.True(role1.Equals(role2));
    }

    [Fact]
    public void Equals_WithDifferentRoles_ShouldReturnFalse()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("admin", "manager", "viewer");

        // Act & Assert
        Assert.False(role1.Equals(role2));
    }

    [Fact]
    public void Equals_WithDifferentCount_ShouldReturnFalse()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("admin", "editor");

        // Act & Assert
        Assert.False(role1.Equals(role2));
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var role = CompositeRoleName.Create("admin", "editor");

        // Act & Assert
        Assert.False(role.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var role = CompositeRoleName.Create("admin", "editor");
        var notRole = "Just a string";

        // Act & Assert
        Assert.False(role.Equals(notRole));
    }

    [Fact]
    public void GetHashCode_WithIdenticalRoles_ShouldReturnSameValue()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("admin", "editor", "viewer");

        // Act
        var hashCode1 = role1.GetHashCode();
        var hashCode2 = role2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentOrder_ShouldReturnSameValue()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("viewer", "admin", "editor");

        // Act
        var hashCode1 = role1.GetHashCode();
        var hashCode2 = role2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentCase_ShouldReturnSameValue()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("ADMIN", "Editor", "Viewer");

        // Act
        var hashCode1 = role1.GetHashCode();
        var hashCode2 = role2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentRoles_ShouldReturnDifferentValues()
    {
        // Arrange
        var role1 = CompositeRoleName.Create("admin", "editor", "viewer");
        var role2 = CompositeRoleName.Create("admin", "manager", "viewer");

        // Act
        var hashCode1 = role1.GetHashCode();
        var hashCode2 = role2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }
}