using TigreDoMexico.Quizz.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureAppServices();

var app = builder.Build();
app.ConfigureMiddlewares();
app.MapEndpoints();

app.Run();

namespace TigreDoMexico.Quizz.Api
{
    public partial class Program;
}