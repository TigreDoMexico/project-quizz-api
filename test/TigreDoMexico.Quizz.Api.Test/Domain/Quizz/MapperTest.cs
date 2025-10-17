using AutoBogus;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.Responses;

namespace TigreDoMexico.Quizz.Api.Test.Domain.Quizz;

public class MapperTest
{
    [Fact]
    public void CriarPerguntaCommand_Para_Pergunta()
    {
        // ARRANGE
        var respostas = new AutoFaker<CriarResposta>()
            .RuleFor(resp => resp.Correta, false)
            .Generate(4);
        
        respostas.Add(new AutoFaker<CriarResposta>()
            .RuleFor(resp => resp.Correta, true)
            .Generate());
        
        var command = new AutoFaker<CriarPerguntaCommand>()
            .RuleFor(perg => perg.Respostas, respostas)
            .Generate();
        
        // ACT
        Pergunta result = command;

        // ASSERT
        Assert.Equal(command.Enunciado, result.Enunciado);
        Assert.Equal(command.Categoria, result.Categoria);
        Assert.Equal(command.Respostas.Count, result.Alternativas.Count);

        for (int i = 0; i < result.Alternativas.Count; i++)
        {
            Assert.Equal(command.Respostas[i].Enunciado, result.Alternativas[i].Enunciado);
            Assert.Equal(command.Respostas[i].Correta, result.Alternativas[i].Correta);
        }
    }

    [Fact]
    public void Pergunta_Para_PerguntaResponse()
    {
        // ARRANGE
        var respostas = new AutoFaker<Resposta>()
            .RuleFor(resp => resp.Correta, false)
            .Generate(4);
        
        respostas.Add(new AutoFaker<Resposta>()
            .RuleFor(resp => resp.Correta, true)
            .Generate());
        
        var entity = new AutoFaker<Pergunta>()
            .RuleFor(perg => perg.Alternativas, respostas)
            .Generate();
        
        // ACT
        PerguntaResponse result = entity;
        
        // ASSERT
        Assert.Equal(entity.Enunciado, result.Enunciado);
        Assert.Equal(entity.Categoria.ToString(), result.Categoria);
        Assert.Equal((int)entity.Categoria, result.CategoriaId);
        Assert.Equal(entity.Alternativas.Count, result.Alternativas.Count);

        for (int i = 0; i < result.Alternativas.Count; i++)
        {
            Assert.Equal(entity.Alternativas[i].Enunciado, result.Alternativas[i].Enunciado);
            Assert.Equal(entity.Alternativas[i].Correta, result.Alternativas[i].Correta);
        }
    }
}