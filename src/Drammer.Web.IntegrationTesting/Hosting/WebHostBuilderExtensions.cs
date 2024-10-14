using System.Diagnostics.CodeAnalysis;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Drammer.Web.IntegrationTesting.Hosting;

[ExcludeFromCodeCoverage]
public static class WebHostBuilderExtensions
{
    /// <summary>
    /// Clears all the loggers and adds a <see cref="XUnitLoggerProvider"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="testOutputHelper">The test output helper</param>
    /// <param name="minimumLevel">The minimum log level.</param>
    /// <returns>A <see cref="IWebHostBuilder"/>.</returns>
    public static IWebHostBuilder ConfigureXunitLogging(
        this IWebHostBuilder builder,
        ITestOutputHelper testOutputHelper,
        LogLevel minimumLevel = LogLevel.Warning)
    {
        builder.ConfigureLogging(
            x =>
            {
                x.ClearProviders();
                x.SetMinimumLevel(minimumLevel);
                x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
            });


        return builder;
    }

    /// <summary>
    /// Sets the default service provider options.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="validateOnBuild">Validate on build.</param>
    /// <param name="validateScopes">Validate scopes.</param>
    /// <returns>A &lt;see cref="IWebHostBuilder"/&gt;.</returns>
    public static IWebHostBuilder AddServiceValidation(
        this IWebHostBuilder builder,
        bool validateOnBuild = true,
        bool validateScopes = true)
    {
        builder.UseDefaultServiceProvider(
            options =>
            {
                options.ValidateOnBuild = validateOnBuild;
                options.ValidateScopes = validateScopes;
            });

        return builder;
    }
}