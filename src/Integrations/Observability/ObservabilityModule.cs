using System.Globalization;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using TigreDoMexico.Quizz.Api.Integrations.Observability.Formatting;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Integrations.Observability;

/// <summary>
/// 
/// </summary>
public class ObservabilityModule : IModule
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSerilog();
        
        ConfigureLogging(builder.Logging, builder.Configuration);
    }

    public static ILoggingBuilder ConfigureLogging(
        ILoggingBuilder builder,
        IConfiguration configuration)
    {
        var destinationUrl = GetDestinationUrl(configuration, "OTel:Logging");

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(new LogFormatter())
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = destinationUrl;
                options.Protocol = OtlpProtocol.Grpc;
                options.FormatProvider = CultureInfo.InvariantCulture;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = "tigredomexico.quizz.api"
                };
            })
            .CreateLogger();
        
        Log.Logger = logger;

        builder.ClearProviders();
        builder.AddSerilog(logger);
        
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