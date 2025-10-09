using FluentValidation;
using MediatR;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Integrations.Data.Quizz;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Handlers;

public class CriarPerguntaHandler(
    IValidator<CriarPerguntaCommand> validator,
    QuizzDbContext context
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
        
        context.Perguntas.Add(request);
        await context.SaveChangesAsync(cancellationToken);

        return 0;
    }
}