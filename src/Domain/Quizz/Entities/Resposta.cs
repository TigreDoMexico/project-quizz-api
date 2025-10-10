using System.ComponentModel.DataAnnotations;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;

public class Resposta
{
    public int Id { get; set; }
    
    [MaxLength(500)]
    public string Enunciado { get; set; } = string.Empty;
    
    public bool Correta { get; set; }
}