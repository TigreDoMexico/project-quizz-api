namespace TigreDoMexico.Quizz.Api.Domain.Quizz;

public class Resposta
{
    public int Id { get; set; }
    
    public string Enunciado { get; set; } = string.Empty;
    
    public bool Correta { get; set; }
}