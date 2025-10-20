namespace TigreDoMexico.Quizz.Api.Integrations.Data.Exceptions;

public class ReadDatabaseException(Exception innerException)
    : Exception(string.Format(Message, innerException.Message), innerException)
{
    private new const string Message = "Erro de busca no Banco de Dados: {0}.";
}