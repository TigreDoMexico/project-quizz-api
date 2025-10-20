using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTelemetry.Trace;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace TigreDoMexico.Quizz.Api.Integrations.Observability.Formatting;

public class LogFormatter : ITextFormatter
{
    private static readonly JsonSerializerOptions CamelCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        IncludeFields = true,
    };
    
    public void Format(LogEvent logEvent, TextWriter output)
    {
        FormatEvent(logEvent, output);
        output.WriteLine();
    }

    private static void FormatEvent(LogEvent logEvent, TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(logEvent, nameof(logEvent));
        ArgumentNullException.ThrowIfNull(output, nameof(output));
        
        var outputData = new LogOutputData
        {
            Timestamp = logEvent.Timestamp.UtcDateTime.ToString("O"),
            LogLevel = logEvent.Level.ToString().ToUpper(CultureInfo.InvariantCulture),
            Message = logEvent.MessageTemplate.Render(logEvent.Properties, CultureInfo.InvariantCulture),
            HttpMethod = GetValue("Method", logEvent),
            HttpRequest = BuildHttpRequestUrl(logEvent),
            HttpStatusCode = GetValue("StatusCode", logEvent),
            TraceId = Tracer.CurrentSpan.Context.TraceId.ToString(),
            SpanId = Tracer.CurrentSpan.Context.SpanId.ToString(),
        };

        var jsonOutputData = JsonSerializer.Serialize(outputData, options: CamelCaseOptions);
        JsonValueFormatter.WriteQuotedJsonString(jsonOutputData, output);
    }

    private static string BuildHttpRequestUrl(LogEvent logEvent)
        => logEvent.Properties.TryGetValue("Scheme", out _)
            ? $"{GetValue("Scheme", logEvent)}" +
              $"://" +
              $"{GetValue("Host", logEvent)}" +
              $"{GetValue("PathBase", logEvent)}" +
              $"{GetValue("Path", logEvent)}" +
              $"{GetValue("QueryString", logEvent)}"
            : string.Empty;

    private static string GetValue(string propertyName, LogEvent logEvent)
        => logEvent.Properties.TryGetValue(propertyName, out var schemeValue)
            ? schemeValue.ToString().Replace("\"", string.Empty).Replace("[", string.Empty).Replace("]", String.Empty)
            : string.Empty;
}