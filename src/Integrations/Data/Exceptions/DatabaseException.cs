namespace TigreDoMexico.Quizz.Api.Integrations.Data.Exceptions;

public class DatabaseException<T>(T entidade, Exception innerException)
    : Exception(string.Format(Message, entidade), innerException)
    where T : class
{
    private new const string Message = "Erro ao salvar os dados da entidade {0}.";
}