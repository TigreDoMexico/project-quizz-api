namespace TigreDoMexico.Quizz.Api.Shared.Responses;

public abstract record Response<TData>
{
    public TData Data { get; set; } = default!;
    
    public bool Sucesso { get; set; }
}

public abstract record Response : Response<object>;