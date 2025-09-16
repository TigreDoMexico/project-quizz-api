using MediatR;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;

public class CriarPerguntaCommand : IRequest<int>
{
    public string Enunciado { get; set; } = string.Empty;

    public Categoria Categoria { get; set; }

    public List<CriarResposta> Respostas { get; set; } = [];
}