using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.Responses;

public class RespostaResponse
{
    public int Id { get; set; }
    
    public string Enunciado { get; set; } = string.Empty;
    
    public bool Correta { get; set; }

    public static implicit operator RespostaResponse(Resposta resposta)
        => new()
        {
            Id = resposta.Id,
            Enunciado = resposta.Enunciado,
            Correta = resposta.Correta,
        };
}