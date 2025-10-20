using System.Reflection;
using FluentValidation;
using TigreDoMexico.Quizz.Api.Middlewares.Module;

namespace TigreDoMexico.Quizz.Api.Middlewares;

/// <summary>
/// Métodos de extensão para configurar os serviços e dependências da aplicação.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Método para configurar os serviços da aplicação.
    /// </summary>
    /// <param name="builder">Builder da aplicação.</param>
    /// <returns>Builder configurado com os serviços necessários para executar a aplicação.</returns>
    public static WebApplicationBuilder ConfigureAppServices(this WebApplicationBuilder builder)
    {
        var applicationName = builder.Environment.ApplicationName;
        var configuration = builder.Configuration;
        var currentAssembly = Assembly.GetAssembly(typeof(Program))!;

        builder.AddModules();

        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHealthChecks();

        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(currentAssembly));
        builder.Services.AddValidatorsFromAssembly(currentAssembly);

        return builder;
    }

    /// <summary>
    /// Método para adicionar os demais middlewares da aplicação.
    /// </summary>
    /// <param name="app">Builder da aplicação.</param>
    /// <returns>Builder configurado com os middlewares necessários para execução.</returns>
    public static IApplicationBuilder ConfigureMiddlewares(this IApplicationBuilder app)
    {
        var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        if (environment.IsProduction())
        {
            app.UseHttpsRedirection();
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseMiddleware<ExceptionHandlerMiddleware>()
            .UseMiddleware<UnitOfWorkMiddleware>();

        return app;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("api/health");
        app.RegisterEndpoints();

        return app;
    }
}