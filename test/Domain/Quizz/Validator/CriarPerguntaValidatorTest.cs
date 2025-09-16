using AutoBogus;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Test.Domain.Shared;

namespace TigreDoMexico.Quizz.Api.Test.Domain.Quizz.Validator;

public class CriarPerguntaValidatorTest : ValidatorTest<CriarPerguntaValidator, CriarPerguntaCommand>
{
    protected override CriarPerguntaCommand GenerateCorrectData()
        => new()
        {
            Enunciado = Faker.Lorem.Sentence(),
            Categoria = Faker.PickRandom<Categoria>(),
            Respostas = this.GerarRespostasNoFormatoCorreto()
        };
    
    protected override List<CriarPerguntaCommand> GenerateIncorrectData()
        =>
        [
            new()
            {
                Enunciado = string.Empty,
                Categoria = Faker.PickRandom<Categoria>(),
                Respostas = this.GerarRespostasNoFormatoCorreto()
            },
            new()
            {
                Enunciado = Faker.Lorem.Sentence(),
                Categoria = (Categoria)10,
                Respostas = this.GerarRespostasNoFormatoCorreto()
            },
            new()
            {
                Enunciado = Faker.Lorem.Sentence(),
                Categoria = Faker.PickRandom<Categoria>(),
                Respostas = []
            },
            new()
            {
                Enunciado = Faker.Lorem.Sentence(),
                Categoria = Faker.PickRandom<Categoria>(),
                Respostas = new AutoFaker<CriarResposta>()
                    .RuleFor(resposta => resposta.Correta, false)
                    .Generate(4)
            }
        ];

    private List<CriarResposta> GerarRespostasNoFormatoCorreto()
    {
        var respostas = new AutoFaker<CriarResposta>()
            .RuleFor(resposta => resposta.Correta, false)
            .Generate(4);

        respostas.Add(new AutoFaker<CriarResposta>()
            .RuleFor(resposta => resposta.Correta, true)
            .Generate());

        return respostas;
    }
}