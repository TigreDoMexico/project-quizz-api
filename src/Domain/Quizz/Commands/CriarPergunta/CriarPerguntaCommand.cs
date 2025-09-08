using MediatR;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;

public class CriarPerguntaCommand : IRequest<int>
{
    public string Enunciado { get; set; } = string.Empty;
}