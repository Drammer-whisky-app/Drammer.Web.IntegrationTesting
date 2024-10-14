namespace Drammer.Web.IntegrationTesting;

public sealed record KeyVaultSecret
{
    public required string Name { get; init; }

    public required string Value { get; init; }

    public string ConfigurationName => Name.Replace("--", ":");
}