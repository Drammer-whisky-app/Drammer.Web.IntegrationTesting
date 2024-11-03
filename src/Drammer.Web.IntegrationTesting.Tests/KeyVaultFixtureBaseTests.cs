using Azure;
using Azure.Security.KeyVault.Secrets;

namespace Drammer.Web.IntegrationTesting.Tests;

public sealed class KeyVaultFixtureBaseTests
{
    [Fact]
    public void Secrets_ReturnsSecrets()
    {
        // arrange
        var secretClient = new SecretClientMock();
        var keyVaultFixture = new KeyVaultFixtureImpl(secretClient, "secret1");

        // act
        var secrets = keyVaultFixture.Secrets;

        // assert
        secrets.Should().NotBeEmpty();
        secrets.Should().HaveCount(1);
        secrets[0].Name.Should().Be("secret1");
    }

    private sealed class KeyVaultFixtureImpl : KeyVaultFixtureBase
    {
        public KeyVaultFixtureImpl(SecretClient secretClient, params string[] keyVaultSecretNames) : base(
            secretClient,
            keyVaultSecretNames)
        {
        }
    }

    private sealed class SecretClientMock : SecretClient
    {
        private readonly Fixture _fixture = new();

        public override Response<Azure.Security.KeyVault.Secrets.KeyVaultSecret> GetSecret(
            string name,
            string? version = null,
            CancellationToken cancellationToken = default)
        {
            var secret = new Azure.Security.KeyVault.Secrets.KeyVaultSecret(name, _fixture.Create<string>());
            var responseMock = new Mock<Response<Azure.Security.KeyVault.Secrets.KeyVaultSecret>>();
            responseMock.Setup(x => x.Value).Returns(secret);
            return responseMock.Object;
        }
    }
}