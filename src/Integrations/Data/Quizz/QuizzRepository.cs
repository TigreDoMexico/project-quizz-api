using Microsoft.EntityFrameworkCore;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;
using TigreDoMexico.Quizz.Api.Integrations.Data.Exceptions;

namespace TigreDoMexico.Quizz.Api.Integrations.Data.Quizz;

public class QuizzRepository(QuizzDbContext context, ILogger<QuizzRepository> logger) : IQuizzRepository
{
    public async Task<int> CriarAsync(Pergunta entidade, CancellationToken token = default)
    {
        logger.LogInformation("Salvando pergunta no banco de dados.");

        try
        {
            await context.Perguntas.AddAsync(entidade, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao criar pergunta.");
            throw new WriteDatabaseException<Pergunta>(entidade, ex);
        }
        
        logger.LogInformation("Pergunta {id} salvo com sucesso.", entidade.Id);
        return entidade.Id;
    }

    public async Task<List<Pergunta>> ObterPorCategoria(Categoria categoria, int limite, CancellationToken token = default)
    {
        logger.LogInformation("Obtendo perguntas do banco de dados.");
        
        try
        {
            return await context
                .Perguntas
                .Include(p => p.Alternativas)
                .Where(p => p.Categoria == categoria)
                .Take(limite)
                .ToListAsync(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao buscar uma pergunta no banco de dados.");
            throw new ReadDatabaseException(ex);
        }
    }
}