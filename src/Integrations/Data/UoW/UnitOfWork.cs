using Microsoft.EntityFrameworkCore.Storage;
using TigreDoMexico.Quizz.Api.Integrations.Data.Quizz;

namespace TigreDoMexico.Quizz.Api.Integrations.Data.UoW;

public class UnitOfWork(QuizzDbContext dbContext, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private bool _committed;
    private bool _disposed;

    public async Task IniciarTransactionAsync()
    {
        logger.LogInformation("[UNIT OF WORK] - Iniciando a transaction");
        _transaction = await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        logger.LogInformation("[UNIT OF WORK] - Commit da transaction");
        if (_transaction != null)
        {
            await dbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
            
            _committed = true;
        }
    }

    public async Task RollbackAsync()
    {
        logger.LogWarning("[UNIT OF WORK] - Rollback da transaction");
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
        
        _committed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_transaction != null)
        {
            if (!_committed)
            {
                try
                {
                    await dbContext.SaveChangesAsync();
                    await _transaction.CommitAsync();
                }
                catch
                {
                    await _transaction.RollbackAsync();
                }
            }
            
            _transaction.Dispose();
        }
        
        _disposed = true;
    }
}