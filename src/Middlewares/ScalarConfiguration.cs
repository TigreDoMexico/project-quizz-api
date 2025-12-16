using Scalar.AspNetCore;

namespace TigreDoMexico.Quizz.Api.Middlewares;

public static class ScalarConfiguration
{
    public static WebApplication MapScalarWithConfiguration(this IApplicationBuilder app)
    {
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Quizz API Documentation")
                .WithTheme(ScalarTheme.Purple)
                .WithDarkMode(true)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }
}