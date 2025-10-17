using MediatR;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Shared.Responses;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;

public class CriarPerguntaCommand : IRequest<Response>
{
    public string Enunciado { get; set; } = string.Empty;

    public Categoria Categoria { get; set; }

    public List<CriarResposta> Respostas { get; set; } = [];

    public static implicit operator Pergunta(CriarPerguntaCommand command)
        => new()
        {

            Enunciado = command.Enunciado,
            Categoria = command.Categoria,
            Alternativas = command.Respostas.Select(r => new Resposta
            {
                Enunciado = r.Enunciado,
                Correta = r.Correta
            }).ToList()
        };
}