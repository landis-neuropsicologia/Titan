using Titan.Domain.ValueObjects;

namespace Titan.Domain.Entities.Website;

public sealed class Testimonial : Entity<int>
{
    #region Properties

    public Name Name { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public DateTime Date { get; private set; }

    public int Assessment { get; private set; }

    public string? Photo { get; private set; }

    public string? Position { get; private set; }

    #endregion

    #region C'tor

    private Testimonial() : base() { }

    public Testimonial(Name name, string description, DateTime date, int assessment = 5, string? photo = null, string? position = null)
    {
        Name = name;
        Description = description;
        Date = date;
        Assessment = assessment;
        Photo = photo;
        Position = position;
    }

    #endregion
}
