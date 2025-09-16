using System.Text.Json;
using Bogus;
using FluentValidation;

namespace TigreDoMexico.Quizz.Api.Test.Domain.Shared;

/// <summary>
/// Classe abstrata para testar os validators do FluentValidation.
/// </summary>
/// <typeparam name="TValidator">Classe que implementa o FluentValidation</typeparam>
/// <typeparam name="TObject">Classe que será validada</typeparam>
public abstract class ValidatorTest<TValidator, TObject>
    where TValidator : AbstractValidator<TObject>, new()
{
    protected Faker Faker { get; private set; } = new();

    protected abstract TObject GenerateCorrectData();

    protected abstract List<TObject> GenerateIncorrectData();

    [Fact]
    public void Given_CorrectObject_When_Validate_Should_NotHaveAnyError()
    {
        var correctObject = this.GenerateCorrectData();
        IValidator<TObject> validator = new TValidator();

        var result = validator.Validate(correctObject);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Given_IncorrectObject_When_Validate_Should_HaveError()
    {
        var incorrectDataList = this.GenerateIncorrectData();
        IValidator<TObject> validator = new TValidator();

        foreach (var incorrectData in incorrectDataList)
        {
            var stringData = JsonSerializer.Serialize(incorrectData);
            var result = validator.Validate(incorrectData);

            Assert.False(result.IsValid, $"{stringData} expected to be invalid");
            Assert.NotEmpty(result.Errors);
        }
    }
}