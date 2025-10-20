namespace TigreDoMexico.Quizz.Api.Shared.Responses;

public record ErroResponse<TData> : Response<TData>
{
    public int CodigoErro { get; set; }

    public ErroResponse(TData data, int codigoErro)
    {
        Data = data;
        CodigoErro = codigoErro;
        Sucesso = false;
    }
}

public record ErroResponse : Response
{
    public int CodigoErro { get; set; }
    
    public ErroResponse(object data, int codigoErro)
    {
        Data = data;
        CodigoErro = codigoErro;
        Sucesso = false;
    }
}