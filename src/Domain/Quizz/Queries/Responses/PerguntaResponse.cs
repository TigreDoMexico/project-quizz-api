using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.Responses;

public class PerguntaResponse
{
    public string Enunciado { get; set; } = string.Empty;
    
    public int Id { get; set; }
    
    public string Categoria { get; set; } = string.Empty;
    
    public int CategoriaId { get; set; }
    
    public List<RespostaResponse> Alternativas { get; set; } = [];

    public static implicit operator PerguntaResponse(Pergunta pergunta)
        => new()
        {
            Id = pergunta.Id,
            Enunciado = pergunta.Enunciado,
            Categoria = pergunta.Categoria.ToString(),
            CategoriaId = (int)pergunta.Categoria,
            Alternativas = pergunta.Alternativas.Select(p => (RespostaResponse)p).ToList(),
        };
}