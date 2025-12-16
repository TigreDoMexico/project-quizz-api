using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using Scalar.AspNetCore;
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
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("version"),
                    new HeaderApiVersionReader("X-Version")
                );
            })
            .AddApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                var provider = builder.Services
                    .BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(
                        description.GroupName,
                        new Microsoft.OpenApi.Models.OpenApiInfo
                        {
                            Title = "Quizz API",
                            Version = description.ApiVersion.ToString(),
                            Description = "API para gerenciamento de quizzes"
                        });
                }
                
                // c.SwaggerDoc("v1", new OpenApiInfo
                // {
                //     Title = "Quizz API",
                //     Version = "v1",
                //     Description = "API para gerenciamento de quizzes"
                // });
                // c.SwaggerDoc("v2", new OpenApiInfo
                // {
                //     Title = "Quizz API",
                //     Version = "v2",
                //     Description = "API para gerenciamento de quizzes - Versão 2"
                // });
            })
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
        
        app.UseSwagger();
        app.MapScalarApiReference()

        app.UseMiddleware<ExceptionHandlerMiddleware>()
            .UseMiddleware<UnitOfWorkMiddleware>();

        return app;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("api/health");
        app.RegisterEndpoints();
        
        var environment = app.Services.GetRequiredService<IWebHostEnvironment>();
        if (!environment.IsProduction())
        {
            app.MapScalarWithConfiguration();
        }

        return app;
    }
}