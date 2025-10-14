namespace TigreDoMexico.Quizz.Api.Shared.Pagination;

public record PageQuery(int NumeroPagina, int LimitePorPagina)
{
    public int Pular =>  NumeroPagina * LimitePorPagina;
    
    public bool IsValido => NumeroPagina > 0 && LimitePorPagina > 0;
}