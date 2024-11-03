namespace Drammer.Web.IntegrationTesting.Tests;

public sealed class KeyVaultSecretExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ToConfigurationCollection_ReturnsConfigurationCollection()
    {
        // arrange
        var secrets = _fixture.CreateMany<KeyVaultSecret>().ToList();

        // act
        var result = secrets.ToConfigurationCollection();

        // assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(secrets.Count);
        foreach (var secret in secrets)
        {
            result.Should().ContainKey(secret.ConfigurationName);
            result[secret.ConfigurationName].Should().Be(secret.Value);
        }
    }
}