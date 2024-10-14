namespace Drammer.Web.IntegrationTesting;

public static class KeyVaultSecretExtensions
{
    public static Dictionary<string, string?> ToConfigurationCollection(this IEnumerable<KeyVaultSecret> secrets)
    {
        return secrets.ToDictionary(x => x.ConfigurationName, x => (string?)x.Value);
    }
}