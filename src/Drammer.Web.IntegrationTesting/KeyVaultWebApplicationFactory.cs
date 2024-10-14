using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Drammer.Web.IntegrationTesting;

public class KeyVaultWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram: class
{
    private readonly IEnumerable<KeyVaultSecret> _secrets;

    private readonly string _environment;

    private readonly Dictionary<string, string?>? _configuration;

    public KeyVaultWebApplicationFactory(
        IEnumerable<KeyVaultSecret> secrets,
        string environment = "IntegrationTest",
        Dictionary<string, string?>? additionalConfiguration = null)
    {
        _secrets = secrets;
        _environment = environment;
        _configuration = additionalConfiguration;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = _configuration ?? new Dictionary<string, string?>();
        foreach (var s in _secrets.ToConfigurationCollection())
        {
            configuration.Add(s.Key, s.Value);
        }

        builder.ConfigureAppConfiguration(x => x.AddInMemoryCollection(configuration));

        builder.UseEnvironment(_environment);

        base.ConfigureWebHost(builder);
    }
}