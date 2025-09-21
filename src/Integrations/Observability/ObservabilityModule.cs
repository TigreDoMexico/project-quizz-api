using Serilog;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Integrations.Observability;

/// <summary>
/// 
/// </summary>
public class ObservabilityModule : IModule
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSerilog();
    }

    public static ILoggingBuilder ConfigureLogging(
        ILoggingBuilder builder,
        IConfiguration configuration,
        string applicationName)
    {
        var destinationUrl = GetDestinationUrl(configuration, "OTel:Logging");

        var logger = new LoggerConfiguration();
        
        return builder;
    }

    private static string GetDestinationUrl(IConfiguration configuration, string section)
    {
        var destinationUrl = configuration.GetSection(section).Value;

        return !Uri.IsWellFormedUriString(destinationUrl, UriKind.Absolute) ?
            throw new InvalidDataException($"URL inválida para {section}.") :
            destinationUrl;
    }
}