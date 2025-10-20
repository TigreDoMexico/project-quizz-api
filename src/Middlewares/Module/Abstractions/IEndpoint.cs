namespace TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

public interface IEndpoint
{
    static abstract void MapEndpoint(IEndpointRouteBuilder endpoints);
}