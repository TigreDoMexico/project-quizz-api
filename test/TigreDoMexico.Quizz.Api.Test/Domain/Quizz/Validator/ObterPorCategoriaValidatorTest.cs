using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.ObterPorCategoria;
using TigreDoMexico.Quizz.Api.Test.Domain.Shared;

namespace TigreDoMexico.Quizz.Api.Test.Domain.Quizz.Validator;

public class ObterPorCategoriaValidatorTest : ValidatorTest<ObterPorCategoriaValidator, ObterPorCategoriaQuery>
{
    protected override ObterPorCategoriaQuery GenerateCorrectData()
        => new()
        {
            Categoria = Faker.PickRandom<Categoria>(),
            Limite = Faker.Random.Int(1, 10),
        };

    protected override List<ObterPorCategoriaQuery> GenerateIncorrectData()
        =>
        [
            new()
            {
                Categoria = (Categoria)10,
                Limite = Faker.Random.Int(1, 10),
            },
            new()
            {
                Categoria = Faker.PickRandom<Categoria>(),
                Limite = 0,
            }
        ];
}