using System.Net;
using FluentValidation;
using MediatR;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;
using TigreDoMexico.Quizz.Api.Shared.Responses;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Handlers;

public class CriarPerguntaHandler(
    IValidator<CriarPerguntaCommand> validator,
    IQuizzRepository repository
) : IRequestHandler<CriarPerguntaCommand, Response>, IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/v1/quizz", async (CriarPerguntaCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return response.ParaHttpResult(HttpStatusCode.Created);
        });
    }

    public async Task<Response> Handle(CriarPerguntaCommand request, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            return new ErroResponse(result.ToString("\n"), (int)HttpStatusCode.UnprocessableContent);
        }
        
        var newId = await repository.CriarAsync(request, cancellationToken);
        return new SucessoResponse(newId);
    }
}