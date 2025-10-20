using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.ObterPorCategoria;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.Responses;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;
using TigreDoMexico.Quizz.Api.Shared.Responses;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Handlers;

public class ObterPorCategoriaHandler(
    IValidator<ObterPorCategoriaQuery> validator,
    IQuizzRepository repository
) : IRequestHandler<ObterPorCategoriaQuery, Response>, IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v1/quizz", async (
            [FromQuery] Categoria categoria,
            [FromQuery] int? limite,
            IMediator mediator) =>
        {
            var query = new ObterPorCategoriaQuery { Categoria = categoria, Limite = limite ?? 10 };

            var response = await mediator.Send(query);
            return response.ParaHttpResult();
        });
    }

    public async Task<Response> Handle(
        ObterPorCategoriaQuery request,
        CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            return new ErroResponse(result.ToString("\n"), (int)HttpStatusCode.UnprocessableContent);
        }
        
        var perguntas = await repository.ObterPorCategoriaAsync(request.Categoria, request.Limite, cancellationToken);
        var listaPerguntas = perguntas
            .Select(p => (PerguntaResponse)p)
            .ToList();
        
        return new SucessoResponse(listaPerguntas);
    }
}