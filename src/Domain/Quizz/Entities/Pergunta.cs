namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;

public class Pergunta
{
    public int Id { get; set; }
    
    public string Enunciado { get; set; } = string.Empty;
    
    public List<Resposta> Alternativas { get; set; } = [];
}