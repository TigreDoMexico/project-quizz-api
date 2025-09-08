namespace TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

public interface IModule
{
    static abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}