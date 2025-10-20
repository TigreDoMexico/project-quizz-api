namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;

public class CriarResposta
{
    public string Enunciado { get; set; } = string.Empty;

    public bool Correta { get; set; }
}