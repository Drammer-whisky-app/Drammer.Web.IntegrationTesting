using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Drammer.Web.IntegrationTesting;

/// <summary>
/// The base class for KeyVault fixtures.
/// </summary>
public abstract class KeyVaultFixtureBase : IDisposable
{
    private readonly List<KeyVaultSecret> _secrets = new();

    private readonly SecretClient _client;

    /// <summary>
    /// Creates a new instance of the <see cref="KeyVaultFixtureBase"/> class.
    /// </summary>
    /// <param name="keyVaultUri">The key vault URI.</param>
    /// <param name="credentialOptions">The credential options.</param>
    /// <param name="keyVaultSecretNames">The secret names to read from the key vault.</param>
    protected KeyVaultFixtureBase(
        Uri keyVaultUri,
        DefaultAzureCredentialOptions? credentialOptions = null,
        params string[] keyVaultSecretNames) : this(
        new SecretClient(
            keyVaultUri,
            new DefaultAzureCredential(credentialOptions ?? new DefaultAzureCredentialOptions())),
        keyVaultSecretNames)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="KeyVaultFixtureBase"/> class.
    /// </summary>
    /// <param name="secretClient">The secret client.</param>
    /// <param name="keyVaultSecretNames">The secret names to read from the key vault.</param>
    protected KeyVaultFixtureBase(SecretClient secretClient, params string[] keyVaultSecretNames)
    {
        _client = secretClient;
        ReadSecrets(keyVaultSecretNames);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _secrets.Clear();
    }

    /// <summary>
    /// Gets the secrets.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the secrets are not initialized.</exception>
    public IReadOnlyList<KeyVaultSecret> Secrets =>
        _secrets ?? throw new InvalidOperationException($"{nameof(_secrets)} has not been initialized");

    /// <summary>
    /// Gets the additional configuration.
    /// </summary>
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