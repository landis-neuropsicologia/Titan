namespace Titan.Domain.ValueObjects;

public abstract class ValueObject
{
    private static ResourceManager ResourceManager { get; set; } = new("Titan.Domain.Properties.Resources", typeof(UserStatus).Assembly);

    public static string GetResourceString(string key, CultureInfo culture)
    {
        string localizedString = ResourceManager.GetString(key, culture);

        return localizedString ?? key;
    }
}
