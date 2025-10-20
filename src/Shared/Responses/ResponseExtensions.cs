using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TigreDoMexico.Quizz.Api.Shared.Responses;

public static class ResponseExtensions
{
    private static readonly JsonSerializerOptions CamelCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        IncludeFields = false,
    };

    public static IResult ParaHttpResult(this Response response, HttpStatusCode customStatus = HttpStatusCode.OK)
    {
        return response.Sucesso
            ? response.ConverterParaSucesso(customStatus)
            : response.ConverterParaFalha();
    }

    private static IResult ConverterParaSucesso(this Response response, HttpStatusCode customStatus = HttpStatusCode.OK)
    {
        if (!response.Sucesso)
        {
            throw new Exception("[INTERNO] - Não é possível transformar um resultado falho em sucesso!");
        }

        if ((int)customStatus >= 300)
        {
            throw new Exception("[INTERNO] - Resultao bem-sucedido de ver código de status igual a 2xx!");
        }

        var jsonResponse = JsonSerializer.Serialize((response as SucessoResponse), CamelCaseOptions);
        return customStatus == HttpStatusCode.NoContent
            ? Results.NoContent()
            : Results.Content(content: jsonResponse, contentType: "application/json", statusCode: (int)customStatus);
    }

    private static IResult ConverterParaFalha(this Response response)
    {
        if (response.Sucesso)
        {
            throw new Exception("[INTERNO] - Não é possível transformar um resultado sucesso em falho!");
        }
        
        var statusCode = (response as ErroResponse)?.CodigoErro ?? 500;
        var jsonResponse = JsonSerializer.Serialize((response as ErroResponse), CamelCaseOptions);
        
        return Results.Content(content: jsonResponse, contentType: "application/json", statusCode: statusCode);
    }
}