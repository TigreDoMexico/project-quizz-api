using AutoBogus;
using Bogus;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Handlers;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;
using TigreDoMexico.Quizz.Api.Shared.Responses;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TigreDoMexico.Quizz.Api.Test.Domain.Quizz.Handler;

public class CriarPerguntaHandlerTest
{
    private readonly Faker _faker = new();
    
    private readonly IValidator<CriarPerguntaCommand> _validator = Substitute.For<IValidator<CriarPerguntaCommand>>();
    private readonly IQuizzRepository _repository = Substitute.For<IQuizzRepository>();
    private CriarPerguntaHandler _handler;

    public CriarPerguntaHandlerTest()
        => _handler = new CriarPerguntaHandler(_validator, _repository);
    
    [Fact]
    public async Task CriarPerguntaHandler_Deve_EnviarDadosCorretosParaRepository()
    {
        // ARRANGE
        var returnedId = _faker.Random.Int();
        var command = new AutoFaker<CriarPerguntaCommand>().Generate();
        
        var validationResult = new AutoFaker<ValidationResult>()
            .RuleFor(v => v.Errors, [])
            .Generate();
        
        this._validator
            .ValidateAsync(Arg.Any<CriarPerguntaCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(validationResult));
        
        this._repository
            .CriarAsync(Arg.Any<Pergunta>(), Arg.Any<CancellationToken>())
            .Returns(returnedId);
        
        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // ASSERT
        Assert.True(result.Sucesso);
        Assert.Equal(returnedId, result.Data);
        
        await this._repository
            .Received(1)
            .CriarAsync(
                Arg.Is<Pergunta>(p =>
                p.Enunciado == command.Enunciado &&
                p.Categoria == command.Categoria &&
                p.Alternativas.Count == command.Respostas.Count), 
                Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Dado_CommandInvalido_Quando_CriarPerguntaHandler_Deve_RetornarErro()
    {
        // ARRANGE
        var firstError = _faker.Lorem.Sentence();
        var secondError = _faker.Lorem.Sentence();
        
        var command = new AutoFaker<CriarPerguntaCommand>().Generate();
        
        var validationResult = new AutoFaker<ValidationResult>()
            .RuleFor(v => v.Errors, [
                new ValidationFailure("enunciado", firstError),
                new ValidationFailure("categoria", secondError)
            ])
            .Generate();
        
        this._validator
            .ValidateAsync(Arg.Any<CriarPerguntaCommand>(), Arg.Any<CancellationToken>())
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
            .CriarAsync(Arg.Any<Pergunta>(), Arg.Any<CancellationToken>());
    }
}