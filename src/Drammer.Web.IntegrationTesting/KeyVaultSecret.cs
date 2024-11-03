namespace Drammer.Web.IntegrationTesting;

/// <summary>
/// The key vault secret.
/// </summary>
public sealed record KeyVaultSecret
{
    /// <summary>
    /// The name of the secret.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The value of the secret.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// The configuration name of the secret.
    /// </summary>
    public string ConfigurationName => Name.Replace("--", ":");
}