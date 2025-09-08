using MediatR;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;
using TigreDoMexico.Quizz.Api.Middlewares.Module.Abstractions;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Handlers;

public class CriarPerguntaHandler : IRequestHandler<CriarPerguntaCommand, int>, IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        throw new NotImplementedException();
    }

    public Task<int> Handle(CriarPerguntaCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}