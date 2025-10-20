namespace TigreDoMexico.Quizz.Api.Integrations.Observability.Formatting;

public class LogOutputData
{
    public required string Timestamp { get; init; } = string.Empty;
    
    public required string LogLevel { get; init; } = string.Empty;

    public required string Message { get; init; } = string.Empty;
    
    public required string HttpRequest { get; init; } = string.Empty;
    
    public required string HttpMethod { get; init; } = string.Empty;
    
    public required string HttpStatusCode { get; init; } = string.Empty;
    
    public required string TraceId { get; init; } = string.Empty;
    
    public required string SpanId { get; init; } = string.Empty;
}