namespace Drammer.Web.IntegrationTesting;

/// <summary>
/// The key vault secret extensions.
/// </summary>
public static class KeyVaultSecretExtensions
{
    /// <summary>
    /// Converts the secrets to a configuration collection.
    /// </summary>
    /// <param name="secrets">The secrets.</param>
    /// <returns>A <see cref="Dictionary{TKey,TValue}"/>.</returns>
    public static Dictionary<string, string?> ToConfigurationCollection(this IEnumerable<KeyVaultSecret> secrets)
    {
        return secrets.ToDictionary(x => x.ConfigurationName, string? (x) => x.Value);
    }
}