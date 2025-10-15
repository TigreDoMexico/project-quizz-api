using Microsoft.EntityFrameworkCore;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Persistence;
using TigreDoMexico.Quizz.Api.Integrations.Data.Quizz;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Integrations.Data;

public class DataModule : IModule
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
        builder.Services.AddDbContext<QuizzDbContext>(options => options.UseNpgsql(connectionString));

        ConfigureRepositories(builder.Services);
        
        if (builder.Environment.EnvironmentName != "Test")
        {
            ExecutarMigrations(builder.Services);
        }
    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IQuizzRepository, QuizzRepository>();
    }

    private static IServiceCollection ExecutarMigrations(IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<QuizzDbContext>();
        dbContext.Database.Migrate();
        
        return services;
    }
}