namespace TigreDoMexico.Quizz.Api.Shared.Responses;

public record SucessoResponse<TData> : Response<TData>
{
    public SucessoResponse(TData data)
    {
        Data = data;
        Sucesso = true;
    }
}

public record SucessoResponse : Response
{
    public SucessoResponse(object data)
    {
        Data = data;
        Sucesso = true;
    }
}