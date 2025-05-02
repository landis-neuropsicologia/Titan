namespace Titan.Domain.ValueObjects;

public sealed class JobTitle : ValueObject
{
    private static readonly string key = "JobTitle_";

    #region Common Job Titles

    public static readonly string CEO = "Chief Executive Officer";
    public static readonly string CFO = "Chief Financial Officer";
    public static readonly string CTO = "Chief Technology Officer";
    public static readonly string MANAGER = "Manager";
    public static readonly string DIRECTOR = "Director";
    public static readonly string DEVELOPER = "Developer";
    public static readonly string ANALYST = "Analyst";
    public static readonly string DESIGNER = "Designer";
    public static readonly string CONSULTANT = "Consultant";

    #endregion

    #region Properties

    public string Title { get; }

    public string Department { get; }

    public int Level { get; }

    #endregion

    #region C'tor

    private JobTitle(string title, string department = null, int level = 0)
    {
        Title = title;
        Department = department;
        Level = level;
    }

    #endregion

    #region Factory

    public static JobTitle Create(string title, string department = null, int level = 0)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(title));

        if (level < 0)
            throw new ArgumentException(GetResourceString($"{key}LevelNegative", CultureInfo.CurrentCulture), nameof(level));

        // Normalize strings
        var normalizedTitle = NormalizeTitle(title);
        var normalizedDepartment = string.IsNullOrWhiteSpace(department) ? null : department.Trim();

        return new JobTitle(normalizedTitle, normalizedDepartment, level);
    }

    public static JobTitle CreateWithAbbreviation(string abbreviation, string department = null, int level = 0)
    {
        if (string.IsNullOrWhiteSpace(abbreviation))
            throw new ArgumentException(GetResourceString($"{key}AbbreviationNegative", CultureInfo.CurrentCulture), nameof(abbreviation));

        var expandedTitle = ExpandAbbreviation(abbreviation);

        if (expandedTitle == null)
            throw new ArgumentException($"{GetResourceString($"{key}UnknownAbbreviation", CultureInfo.CurrentCulture)} {abbreviation}", nameof(abbreviation));

        return Create(expandedTitle, department, level);
    }

    private static string NormalizeTitle(string title)
    {
        // Remove extra whitespace and trim
        title = title.Trim();

        // Check for common variations and normalize
        var upperTitle = title.ToUpperInvariant();

        // Map common variations to standard titles
        var standardTitles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "CEO", CEO },
            { "CHIEF EXECUTIVE OFFICER", CEO },
            { "CFO", CFO },
            { "CHIEF FINANCIAL OFFICER", CFO },
            { "CTO", CTO },
            { "CHIEF TECHNOLOGY OFFICER", CTO },
            { "MGR", MANAGER },
            { "MANAGER", MANAGER },
            { "DIR", DIRECTOR },
            { "DIRECTOR", DIRECTOR },
            { "DEV", DEVELOPER },
            { "DEVELOPER", DEVELOPER },
            { "SOFTWARE DEVELOPER", DEVELOPER },
            { "SOFTWARE ENGINEER", DEVELOPER },
            { "PROGRAMMER", DEVELOPER },
            { "ANALYST", ANALYST },
            { "BUSINESS ANALYST", ANALYST },
            { "SYSTEMS ANALYST", ANALYST },
            { "DESIGNER", DESIGNER },
            { "UX DESIGNER", DESIGNER },
            { "UI DESIGNER", DESIGNER },
            { "CONSULTANT", CONSULTANT },
            { "ADVISOR", CONSULTANT }
        };

        return standardTitles.TryGetValue(upperTitle, out var standardTitle) ? standardTitle : title;
    }

    private static string ExpandAbbreviation(string abbreviation)
    {
        abbreviation = abbreviation.Trim().ToUpperInvariant();

        var abbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "CEO", CEO },
            { "CFO", CFO },
            { "CTO", CTO },
            { "MGR", MANAGER },
            { "DIR", DIRECTOR },
            { "DEV", DEVELOPER },
            { "SE", DEVELOPER }, // Software Engineer
            { "BA", ANALYST },   // Business Analyst
            { "SA", ANALYST },   // Systems Analyst
            { "DES", DESIGNER },
            { "CON", CONSULTANT }
        };

        return abbreviations.TryGetValue(abbreviation, out var expandedTitle) ? expandedTitle : null;
    }

    #endregion

    #region Methods

    public JobTitle WithDepartment(string department)
    {
        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException(GetResourceString($"{key}DepartmentEmpty", CultureInfo.CurrentCulture), nameof(department));

        return new JobTitle(Title, department.Trim(), Level);
    }

    public JobTitle WithLevel(int level)
    {
        if (level < 0)
            throw new ArgumentException(GetResourceString($"{key}LevelNegative", CultureInfo.CurrentCulture), nameof(level));

        return new JobTitle(Title, Department, level);
    }

    public bool IsManagement()
    {
        var managementTitles = new[]
        {
            CEO, CFO, CTO, MANAGER, DIRECTOR, "VP", "Vice President", "President", "Head"
        };

        return managementTitles.Any(t => Title.Contains(t, StringComparison.OrdinalIgnoreCase));
    }

    public bool IsExecutive()
    {
        var executiveTitles = new[]
        {
            CEO, CFO, CTO, "President", "Chief"
        };

        return executiveTitles.Any(t => Title.Contains(t, StringComparison.OrdinalIgnoreCase));
    }

    public string ToFullString()
    {
        var result = Title;

        if (Level > 0)
            result += $" {RomanNumeral(Level)}";

        if (!string.IsNullOrWhiteSpace(Department))
            result += $", {Department}";

        return result;
    }

    private static string RomanNumeral(int number)
    {
        if (number <= 0)
            return string.Empty;

        if (number > 10)
            return number.ToString();

        var numerals = new[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
        
        return numerals[number - 1];
    }

    #endregion

    #region Equals & HashCode

    public override string ToString() => ToFullString();

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (JobTitle)obj;

        return string.Equals(Title, other.Title, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Department, other.Department, StringComparison.OrdinalIgnoreCase) &&
               Level == other.Level;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + (Title?.ToUpperInvariant().GetHashCode() ?? 0);
            hash = hash * 23 + (Department?.ToUpperInvariant().GetHashCode() ?? 0);
            hash = hash * 23 + Level.GetHashCode();
            return hash;
        }
    }

    #endregion
}