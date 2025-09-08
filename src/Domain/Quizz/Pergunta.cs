namespace TigreDoMexico.Quizz.Api.Domain.Quizz;

public class Pergunta
{
    public int Id { get; set; }
    
    public string Enunciado { get; set; } = string.Empty;
    
    public List<Resposta> Alternativas { get; set; } = [];
}