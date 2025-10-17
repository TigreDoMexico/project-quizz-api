using TigreDoMexico.Quizz.Api.Integrations.Data.UoW;

namespace TigreDoMexico.Quizz.Api.Middlewares;

public class UnitOfWorkMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
    {
        await unitOfWork.IniciarTransactionAsync();

        try
        {
            await next(context);

            if (context.Response.StatusCode < 404)
            {
                await unitOfWork.CommitAsync();
            }
            else
            {
                await unitOfWork.RollbackAsync();
            }
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}