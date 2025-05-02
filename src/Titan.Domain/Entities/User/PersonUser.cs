using Titan.Domain.ValueObjects;

namespace Titan.Domain.Entities.User;

/// <summary>
/// Usuário Pessoa Física. Não é vinculado a uma empresa. 
/// </summary>
public sealed class PersonUser : UserBase
{
    private static readonly string key = "PersonUser_";

    #region Properties

    public ActivationKey ActivationKey { get; private set; }

    public bool RegisteredViaSocialMedia { get; private set; }
    
    public SocialMediaProvider SocialMediaProvider { get; private set; }

    #endregion

    #region C'tor

    private PersonUser() : base() { }

    public PersonUser(Email email, Name name, bool registeredViaSocialMedia = false, SocialMediaProvider socialMediaProvider = null) : base(email, name)
    {
        ActivationKey = ActivationKey.Generate();
        RegisteredViaSocialMedia = registeredViaSocialMedia;
        SocialMediaProvider = socialMediaProvider;

        AddRole(new Role("person_user"));
    }

    #endregion

    #region Behaviors

    public void UpdateName(Name name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException(GetResourceString($"{key}NameEmpty", CultureInfo.CurrentCulture), nameof(name));

        Name = name;
    }

    public void Activate()
    {
        IsActive = true;
        ActivationKey = null;
    }

    #endregion
}
