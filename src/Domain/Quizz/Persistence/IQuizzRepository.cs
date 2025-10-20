using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;

public interface IQuizzRepository
{
    Task<int> CriarAsync(Pergunta entidade, CancellationToken token = default);
    
    Task<List<Pergunta>> ObterPorCategoriaAsync(Categoria categoria, int limite, CancellationToken token = default);
}