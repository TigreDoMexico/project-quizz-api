using System.ComponentModel.DataAnnotations;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;

public class Pergunta
{
    public int Id { get; set; }
    
    [MaxLength(500)]
    public string Enunciado { get; set; } = string.Empty;

    public Categoria Categoria { get; set; }
    
    public List<Resposta> Alternativas { get; set; } = [];

    public override string ToString()
    {
        return $"Pergunta [Id={Id}]: {Enunciado}";
    }
}