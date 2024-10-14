using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Drammer.Web.IntegrationTesting;

public abstract class KeyVaultFixtureBase : IDisposable
{
    private readonly List<KeyVaultSecret> _secrets = new();

    private readonly SecretClient _client;

    protected KeyVaultFixtureBase(
        Uri keyVaultUri,
        DefaultAzureCredentialOptions? credentialOptions = null,
        params string[] keyVaultSecretNames)
    {
        var azureCredentials = new DefaultAzureCredential(credentialOptions ?? new DefaultAzureCredentialOptions());
        _client = new SecretClient(keyVaultUri, azureCredentials);
        ReadSecrets(keyVaultSecretNames);
    }

    public void Dispose()
    {
        _secrets.Clear();
    }

    public IReadOnlyList<KeyVaultSecret> Secrets =>
        _secrets ?? throw new InvalidOperationException($"{nameof(_secrets)} has not been initialized");

    protected virtual Dictionary<string, string?> AdditionalConfiguration { get; } = new ();

    private void ReadSecrets(params string[] keyVaultSecretNames)
    {
        foreach (var name in keyVaultSecretNames)
        {
            var secret = _client.GetSecret(name);
            if (secret == null)
            {
                throw new ArgumentOutOfRangeException($"KeyVault secret with name {name} does not exist.");
            }

            _secrets.Add(new KeyVaultSecret { Name = name, Value = secret.Value.Value });
        }
    }
}