namespace Titan.Domain.Entities;

public abstract class Entity<T>
{
    private static ResourceManager ResourceManager { get; set; } = new("Titan.Domain.Properties.Resources", typeof(Role).Assembly);

    public T Id { get; internal set; }

    public static string GetResourceString(string key, CultureInfo culture)
    {
        string localizedString = ResourceManager.GetString(key, culture);

        return localizedString ?? key;
    }
}
