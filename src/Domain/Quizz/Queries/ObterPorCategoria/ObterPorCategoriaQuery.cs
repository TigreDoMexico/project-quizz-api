using MediatR;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Shared.Responses;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.ObterPorCategoria;

public class ObterPorCategoriaQuery : IRequest<Response>
{
    public Categoria Categoria { get; set; }
    public int Limite { get; set; } = 10;
}