using System.Net;
using Asp.Versioning;
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
        var group = endpoints.MapGroup("/api/v{version:apiVersion}/quizz")
            .WithApiVersionSet()
            .HasApiVersion(1.0);

        group.MapPost("/", async (CriarPerguntaCommand command, IMediator mediator) =>
            {
                var response = await mediator.Send(command);
                return response.ParaHttpResult(HttpStatusCode.Created);
            })
            .WithName("CriarPergunta")
            .WithTags("Quizz");
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