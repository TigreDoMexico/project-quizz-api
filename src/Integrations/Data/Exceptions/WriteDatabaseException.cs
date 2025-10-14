namespace TigreDoMexico.Quizz.Api.Integrations.Data.Exceptions;

public class WriteDatabaseException<T>(T entidade, Exception innerException)
    : Exception(string.Format(Message, entidade), innerException)
    where T : class
{
    private new const string Message = "Erro de escrita no Banco de Dados: {0}.";
}