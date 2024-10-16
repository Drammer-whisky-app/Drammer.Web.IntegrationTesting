namespace Drammer.Web.IntegrationTesting.Tests;

public sealed class KeyVaultSecretTests
{
    [Theory]
    [InlineData("My--Configuration--Name", "My:Configuration:Name")]
    [InlineData("My-Configuration-Name", "My-Configuration-Name")]
    public void ConfigurationName_ReplacesDashesInName(string name, string expected)
    {
        // arrange
        var secret = new KeyVaultSecret
        {
            Name = name,
            Value = string.Empty
        };

        // act
        var result = secret.ConfigurationName;

        // assert
        result.Should().Be(expected);
    }
}