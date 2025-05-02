using Titan.Domain.Entities.User;

namespace Titan.Domain.Entities;

public sealed class Role : Entity<int>
{
    private static readonly string key = "Role_";

    #region Properties

    public string Name { get; private set; } 
    
    public string Description { get; private set; }
    
    public ICollection<UserBase> Users { get; private set; }

    #endregion

    #region C'tor

    private Role()
    {
        Users = [];
    }

    public Role(string name, string description = null) : this()
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException(GetResourceString($"{key}Empty", CultureInfo.CurrentCulture), nameof(name));

        Name = name;
        Description = description;
    }

    #endregion
}
