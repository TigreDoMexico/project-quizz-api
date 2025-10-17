using AutoBogus;
using Bogus;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Handlers;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.ObterPorCategoria;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.Responses;
using TigreDoMexico.Quizz.Api.Shared.Responses;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TigreDoMexico.Quizz.Api.Test.Domain.Quizz.Handler;

public class ObterPorCategoriaHandlerTest
{
    private readonly Faker _faker = new();
    
    private readonly IValidator<ObterPorCategoriaQuery> _validator = Substitute.For<IValidator<ObterPorCategoriaQuery>>();
    private readonly IQuizzRepository _repository = Substitute.For<IQuizzRepository>();
    private ObterPorCategoriaHandler _handler;

    public ObterPorCategoriaHandlerTest()
        => _handler = new ObterPorCategoriaHandler(_validator, _repository);
    
    [Fact]
    public async Task ObterPorCategoriaHandler_Deve_EnviarDadosCorretosParaRepository()
    {
        // ARRANGE
        var returnData = new AutoFaker<Pergunta>().Generate(2);
        var query = new AutoFaker<ObterPorCategoriaQuery>().Generate();
        
        var validationResult = new AutoFaker<ValidationResult>()
            .RuleFor(v => v.Errors, [])
            .Generate();
        
        this._validator
            .ValidateAsync(Arg.Any<ObterPorCategoriaQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(validationResult));
        
        this._repository
            .ObterPorCategoriaAsync(Arg.Any<Categoria>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(returnData);
        
        // ACT
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // ASSERT
        Assert.True(result.Sucesso);
        Assert.Equal(returnData.Count, (result.Data as List<PerguntaResponse>)!.Count);
        
        await this._repository
            .Received(1)
            .ObterPorCategoriaAsync(
                Arg.Is(query.Categoria),
                Arg.Is(query.Limite),
                Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Dado_QueryInvalida_Quando_ObterPorCategoriaHandler_Deve_RetornarErro()
    {
        // ARRANGE
        var firstError = _faker.Lorem.Sentence();
        var secondError = _faker.Lorem.Sentence();
        
        var command = new AutoFaker<ObterPorCategoriaQuery>().Generate();
        
        var validationResult = new AutoFaker<ValidationResult>()
            .RuleFor(v => v.Errors, [
                new ValidationFailure("enunciado", firstError),
                new ValidationFailure("categoria", secondError)
            ])
            .Generate();
        
        this._validator
            .ValidateAsync(Arg.Any<ObterPorCategoriaQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(validationResult));
        
        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // ASSERT
        var erroResult = result as ErroResponse;
        
        Assert.NotNull(erroResult);
        Assert.False(erroResult.Sucesso);
        Assert.Equal(422, erroResult.CodigoErro);
        Assert.Contains(firstError, (string)erroResult.Data);
        Assert.Contains(secondError, (string)erroResult.Data);
        
        await this._repository
            .DidNotReceive()
            .ObterPorCategoriaAsync(Arg.Any<Categoria>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
    }
}