using Titan.Domain.ValueObjects;

namespace Titan.Domain.Entities.Website;

public sealed class Professional : Entity<int>
{
    #region Fields

    public Name Name { get; private set; }

    public string Specialty { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string Image { get; private set; } = string.Empty;

    public string Curriculum { get; private set; } = string.Empty;

    public Email Email { get; private set; }

    public List<string>? Trainings { get; private set; }

    public List<string>? Certifications { get; private set; }

    #endregion

    #region C'tor

    private Professional() : base() { }

    public Professional(Name name, Email email, string specialty, string description, string image, string curriculum)
    {
        Name = name;
        Email = email;
        Specialty = specialty;
        Description = description;
        Image = image;
        Curriculum = curriculum;
    }

    #endregion
}
