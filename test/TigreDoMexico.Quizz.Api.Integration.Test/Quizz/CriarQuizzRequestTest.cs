using System.Text;
using System.Text.Json;
using AutoBogus;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Integration.Test.Factory;
using TigreDoMexico.Quizz.Api.Shared.Responses;

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
        
        var result = await ObterCorpoDoResponse<SucessoResponse<int>>(response);
        
        Assert.NotNull(result);
        Assert.True(result.Sucesso);
        Assert.Equal(0, result.Data);
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
        var result = await ObterCorpoDoResponse<ErroResponse<string>>(response);
        
        Assert.NotNull(result);
        Assert.Equal(422, result.CodigoErro);
        Assert.Equal("Deve existir somente uma resposta certa na lista de respostas.", result.Data);
        Assert.False(result.Sucesso);
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