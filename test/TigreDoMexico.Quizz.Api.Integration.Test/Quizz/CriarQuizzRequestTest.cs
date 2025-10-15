using System.Net;
using System.Text;
using System.Text.Json;
using AutoBogus;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Integration.Test.Factory;

namespace TigreDoMexico.Quizz.Api.Integration.Test.Quizz;

public class CriarQuizzRequestTest(QuizzClassFactory factory) : QuizzRequestTest(factory)
{
    [Fact]
    public async Task Dado_PerguntaCorreta_Quando_CriarQuizz_Deve_RetornarSucesso()
    {
        // ARRANGE
        var client = this.CriarClient();
        var pergunta = this.CriarMockRequest();
        
        var json = JsonSerializer.Serialize(pergunta);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        // ACT
        var response = await client.PostAsync("/api/v1/quizz", content);
        
        // ASSERT
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task Dado_PerguntaInvalida_Quando_CriarQuizz_Deve_RetornarError()
    {
        // ARRANGE
        var client = this.CriarClient();
        var pergunta = this.CriarMockRequest(false);
        
        var json = JsonSerializer.Serialize(pergunta);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        // ACT
        var response = await client.PostAsync("/api/v1/quizz", content);
        
        // ASSERT
        Assert.Equal(HttpStatusCode.UnprocessableContent, response.StatusCode);
    }

    private CriarPerguntaCommand CriarMockRequest(bool valido = true)
    {
        var respostas = new AutoFaker<CriarResposta>()
            .RuleFor(resp => resp.Correta, false)
            .Generate(4);

        if (valido)
        {
            respostas.Add(new AutoFaker<CriarResposta>()
                .RuleFor(resp => resp.Correta, true)
                .Generate());
        }
        
        return new AutoFaker<CriarPerguntaCommand>()
            .RuleFor(perg => perg.Respostas, respostas)
            .Generate();
    }
}