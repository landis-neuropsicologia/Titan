namespace Titan.Domain.ValueObjects;

public sealed class DateRange : ValueObject
{
    private static readonly string key = "DateRange_";

    #region Properties

    public DateTime Start { get; }

    public DateTime End { get; }

    public TimeSpan Duration => End - Start;

    #endregion

    #region C'tor

    private DateRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    #endregion

    #region Factory

    public static DateRange Create(DateTime start, DateTime end)
    {
        if (end < start)
            throw new ArgumentException(GetResourceString($"{key}GreaterThan", CultureInfo.CurrentCulture), nameof(end));

        return new DateRange(start, end);
    }

    public static DateRange CreateFromDuration(DateTime start, TimeSpan duration)
    {
        if (duration < TimeSpan.Zero)
            throw new ArgumentException(GetResourceString($"{key}Negative", CultureInfo.CurrentCulture), nameof(duration));

        return new DateRange(start, start.Add(duration));
    }

    public static DateRange CreateFromDays(DateTime start, int days)
    {
        if (days < 0)
            throw new ArgumentException(GetResourceString($"{key}DaysNotNegative", CultureInfo.CurrentCulture), nameof(days));

        return CreateFromDuration(start, TimeSpan.FromDays(days));
    }

    public static DateRange CreateToday()
    {
        var today = DateTime.Today;
        return new DateRange(today, today.AddDays(1).AddTicks(-1));
    }

    public static DateRange CreateThisWeek()
    {
        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

        return new DateRange(startOfWeek, endOfWeek);
    }

    public static DateRange CreateThisMonth()
    {
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

        return new DateRange(startOfMonth, endOfMonth);
    }

    public static DateRange CreateThisYear()
    {
        var today = DateTime.Today;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var endOfYear = startOfYear.AddYears(1).AddTicks(-1);

        return new DateRange(startOfYear, endOfYear);
    }

    #endregion

    #region Methods

    public bool Contains(DateTime dateTime)
    {
        return dateTime >= Start && dateTime <= End;
    }

    public bool Contains(DateRange other)
    {
        return other.Start >= Start && other.End <= End;
    }

    public bool Overlaps(DateRange other)
    {
        return Start <= other.End && End >= other.Start;
    }

    public DateRange Intersect(DateRange other)
    {
        if (!Overlaps(other))
            throw new InvalidOperationException(GetResourceString($"{key}Intersect", CultureInfo.CurrentCulture));

        var maxStart = Start > other.Start ? Start : other.Start;
        var minEnd = End < other.End ? End : other.End;

        return new DateRange(maxStart, minEnd);
    }

    public DateRange Union(DateRange other)
    {
        if (!Overlaps(other))
            throw new InvalidOperationException(GetResourceString($"{key}Union", CultureInfo.CurrentCulture));

        var minStart = Start < other.Start ? Start : other.Start;
        var maxEnd = End > other.End ? End : other.End;

        return new DateRange(minStart, maxEnd);
    }

    public DateRange[] Split(TimeSpan interval)
    {
        if (interval <= TimeSpan.Zero)
            throw new ArgumentException(GetResourceString($"{key}Split", CultureInfo.CurrentCulture), nameof(interval));

        int count = (int)Math.Ceiling(Duration / interval) + 1;
        var ranges = new DateRange[count - 1];

        for (int i = 0; i < count - 1; i++)
        {
            var rangeStart = Start.Add(interval * i);
            var rangeEnd = i < count - 2
                ? Start.Add(interval * (i + 1)).AddTicks(-1)
                : End;

            ranges[i] = new DateRange(rangeStart, rangeEnd);
        }

        return ranges;
    }

    public string Format(string format = "G")
    {
        if (Start.Date == End.Date)
        {
            // Same day
            return Start.ToString(format);
        }
        else
        {
            return $"{Start.ToString(format)} - {End.ToString(format)}";
        }
    }

    #endregion

    #region Equals & HashCode

    public override string ToString() => Format();

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (DateRange)obj;
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }

    #endregion
}