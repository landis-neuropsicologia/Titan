namespace Titan.Domain.ValueObjects;

public sealed class CompositeRoleName : ValueObject
{
    private static readonly string key = "CompositeRoleName_";

    #region Constants

    public static readonly string Separator = "|";

    #endregion

    #region Properties

    public ReadOnlyCollection<string> RoleNames { get; }

    #endregion

    #region C'tor

    private CompositeRoleName(IEnumerable<string> roleNames)
    {
        RoleNames = new ReadOnlyCollection<string>(roleNames.ToList());
    }

    #endregion

    #region Factory

    public static CompositeRoleName Create(params string[] roleNames)
    {
        if (roleNames == null || roleNames.Length == 0)
            throw new ArgumentException(GetResourceString($"{key}RoleEmpty", CultureInfo.CurrentCulture), nameof(roleNames));

        var validatedRoles = new List<string>();

        foreach (var roleName in roleNames)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException(GetResourceString($"{key}RoleName", CultureInfo.CurrentCulture), nameof(roleNames));

            var normalizedRole = roleName.Trim();

            // Prevent duplicate roles
            if (!validatedRoles.Contains(normalizedRole, StringComparer.OrdinalIgnoreCase))
                validatedRoles.Add(normalizedRole);
        }

        // Sort the roles for consistency
        validatedRoles.Sort(StringComparer.OrdinalIgnoreCase);

        return new CompositeRoleName(validatedRoles);
    }

    public static CompositeRoleName Parse(string compositeName)
    {
        if (string.IsNullOrWhiteSpace(compositeName))
            throw new ArgumentException(GetResourceString($"{key}CompositeRoleEmpty", CultureInfo.CurrentCulture), nameof(compositeName));

        var roleNames = compositeName.Split(Separator, StringSplitOptions.RemoveEmptyEntries);

        if (roleNames.Length == 0)
            throw new ArgumentException(GetResourceString($"{key}RoleEmpty", CultureInfo.CurrentCulture), nameof(compositeName));

        return Create(roleNames);
    }

    #endregion

    #region Methods

    public bool Contains(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;

        return RoleNames.Any(r => string.Equals(r, roleName.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public bool ContainsAny(params string[] roleNames)
    {
        if (roleNames == null || roleNames.Length == 0)
            return false;

        return roleNames.Any(Contains);
    }

    public bool ContainsAll(params string[] roleNames)
    {
        if (roleNames == null || roleNames.Length == 0)
            return true;

        return roleNames.All(Contains);
    }

    public CompositeRoleName Add(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentException(GetResourceString($"{key}RoleName", CultureInfo.CurrentCulture), nameof(roleName));

        var normalizedRole = roleName.Trim();

        if (Contains(normalizedRole))
            return this;

        var newRoles = new List<string>(RoleNames) { normalizedRole };

        newRoles.Sort(StringComparer.OrdinalIgnoreCase);

        return new CompositeRoleName(newRoles);
    }

    public CompositeRoleName Remove(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return this;

        var normalizedRole = roleName.Trim();

        if (!Contains(normalizedRole))
            return this;

        var newRoles = RoleNames.Where(r => !string.Equals(r, normalizedRole, StringComparison.OrdinalIgnoreCase)).ToList();

        if (newRoles.Count == 0)
            throw new InvalidOperationException(GetResourceString($"{key}Remove", CultureInfo.CurrentCulture));

        return new CompositeRoleName(newRoles);
    }

    public CompositeRoleName Union(CompositeRoleName other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        var newRoles = new HashSet<string>(RoleNames, StringComparer.OrdinalIgnoreCase);

        foreach (var role in other.RoleNames)
        {
            newRoles.Add(role);
        }

        return new CompositeRoleName(newRoles.OrderBy(r => r, StringComparer.OrdinalIgnoreCase));
    }

    public CompositeRoleName Intersect(CompositeRoleName other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        var commonRoles = RoleNames
            .Where(r => other.Contains(r))
            .ToList();

        if (commonRoles.Count == 0)
            throw new InvalidOperationException(GetResourceString($"{key}Intersect", CultureInfo.CurrentCulture));

        return new CompositeRoleName(commonRoles);
    }

    #endregion

    #region Serialization

    public string Serialize()
    {
        return string.Join(Separator, RoleNames);
    }

    #endregion

    #region Equals & HashCode

    public override string ToString() => Serialize();

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (CompositeRoleName)obj;

        if (RoleNames.Count != other.RoleNames.Count)
            return false;

        // Since roles are sorted in creation, we can compare them in order
        for (int i = 0; i < RoleNames.Count; i++)
        {
            if (!string.Equals(RoleNames[i], other.RoleNames[i], StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;

            foreach (var role in RoleNames)
            {
                hash = hash * 23 + (role?.ToUpperInvariant().GetHashCode() ?? 0);
            }

            return hash;
        }
    }

    #endregion
}