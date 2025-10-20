using System.Text.Json;
using TigreDoMexico.Quizz.Api.Integration.Test.Factory;

namespace TigreDoMexico.Quizz.Api.Integration.Test.Quizz;

public abstract class QuizzRequestTest(QuizzClassFactory factory) : IClassFixture<QuizzClassFactory>
{
    protected HttpClient CriarClient() => factory.CreateClient();
    
    protected static async Task<T?> ObterCorpoDoResponse<T>(HttpResponseMessage? response)
    {
        if (response == null) return default;
        
        var content = await response.Content.ReadAsStringAsync();
        var parsedResult = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
        
        return parsedResult;
    }
}