using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.ObterPorCategoria;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.Responses;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Handlers;

public class ObterPorCategoriaHandler(
    IValidator<ObterPorCategoriaQuery> validator,
    IQuizzRepository repository
) : IRequestHandler<ObterPorCategoriaQuery, List<PerguntaResponse>>, IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v1/quizz", async (
            [FromQuery] Categoria categoria,
            [FromQuery] int? limite,
            IMediator mediator) =>
        {
            var query = new ObterPorCategoriaQuery { Categoria = categoria, Limite = limite ?? 10 };

            var result = await mediator.Send(query);
            return Results.Ok(result);
        });
    }

    public async Task<List<PerguntaResponse>> Handle(
        ObterPorCategoriaQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        var perguntas = await repository.ObterPorCategoria(request.Categoria, request.Limite, cancellationToken);

        return perguntas
            .Select(p => (PerguntaResponse)p)
            .ToList();
    }
}