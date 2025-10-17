namespace TigreDoMexico.Quizz.Api.Integrations.Data.UoW;

public interface IUnitOfWork : IAsyncDisposable
{
    Task IniciarTransactionAsync();

    Task CommitAsync();

    Task RollbackAsync();
}