using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Drammer.Web.IntegrationTesting;

/// <summary>
/// The key vault web application factory.
/// </summary>
/// <typeparam name="TProgram">The program type.</typeparam>
public class KeyVaultWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram: class
{
    private readonly IEnumerable<KeyVaultSecret> _secrets;

    private readonly string _environment;

    private readonly Dictionary<string, string?>? _configuration;

    /// <summary>
    /// Creates a new instance of the <see cref="KeyVaultWebApplicationFactory{TProgram}"/> class.
    /// </summary>
    /// <param name="secrets">The secrets.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="additionalConfiguration">Additional configuration properties.</param>
    public KeyVaultWebApplicationFactory(
        IEnumerable<KeyVaultSecret> secrets,
        string environment = "IntegrationTest",
        Dictionary<string, string?>? additionalConfiguration = null)
    {
        _secrets = secrets;
        _environment = environment;
        _configuration = additionalConfiguration;
    }

    /// <inheritdoc />
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