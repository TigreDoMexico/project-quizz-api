using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Shared.Responses;

namespace TigreDoMexico.Quizz.Api.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private const string ErrorMessage = "ErrorMessage";
    private const string ErrorCode = "ErrorCode";
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
            if (context.Items.ContainsKey(ErrorMessage) && context.Items.ContainsKey(ErrorCode))
            {
                await CriarErroResponse(context);
            }
        }
        catch (Exception e)
        {
            await CriarErroResponse(context, (int)HttpStatusCode.InternalServerError, e);
        }
    }

    private static async Task CriarErroResponse(HttpContext context)
    {
        context.Items.TryGetValue(ErrorMessage, out var mensagemErro);
        context.Items.TryGetValue(ErrorCode, out var codigoErro);

        if (mensagemErro is not null &&
            int.TryParse(codigoErro?.ToString(), out var statusCode) &&
            Enum.IsDefined(typeof(HttpStatusCode), statusCode))
        {
            var response = new ErroResponse<string>(mensagemErro?.ToString() ?? string.Empty, statusCode);
            await GerarHttpResponse(context, response, statusCode);
        }
    }

    private static async Task CriarErroResponse(HttpContext context, int statusCode, Exception e)
        => await GerarHttpResponse(context, new ErroResponse<string>(e.Message, statusCode), statusCode);
    
    private static async Task GerarHttpResponse(HttpContext context, ErroResponse<string> response, int statusCode)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            IncludeFields = false,
        };
        
        var jsonResponse = JsonSerializer.Serialize(response, options);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(jsonResponse);
    }
}