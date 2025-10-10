using FluentValidation;
using MediatR;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Handlers;

public class CriarPerguntaHandler(
    IValidator<CriarPerguntaCommand> validator,
    IQuizzRepository repository
) : IRequestHandler<CriarPerguntaCommand, int>, IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/v1/quizz", async (CriarPerguntaCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });
    }

    public async Task<int> Handle(CriarPerguntaCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var newId = await repository.CriarAsync(request, cancellationToken);
        return newId;
    }
}