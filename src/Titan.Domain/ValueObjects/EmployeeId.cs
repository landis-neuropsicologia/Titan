namespace Titan.Domain.ValueObjects;

public sealed class EmployeeId : ValueObject
{
    private static readonly string key = "EmployeeId_";

    #region Properties

    public string Value { get; }

    #endregion

    #region C'tor

    private EmployeeId(string value)
    {
        Value = value;
    }

    #endregion

    #region Factory

    public static EmployeeId Create(string employeeId)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(employeeId));

        employeeId = employeeId.Trim();

        if (employeeId.Length > 50)
            throw new ArgumentException(GetResourceString($"{key}TooLong", CultureInfo.CurrentCulture), nameof(employeeId));

        return new EmployeeId(employeeId);
    }

    #endregion

    #region Equals & HashCode

    public static implicit operator string(EmployeeId employeeId) => employeeId?.Value;

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (EmployeeId)obj;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Value.ToUpperInvariant().GetHashCode();
    }

    #endregion
}

