using Microsoft.EntityFrameworkCore;
using TigreDoMexico.Quizz.Api.Integrations.Data.Quizz;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Integrations.Data;

public class DataModule : IModule
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQL");
        services.AddDbContext<QuizzDbContext>(options => options.UseNpgsql(connectionString));
    }
}