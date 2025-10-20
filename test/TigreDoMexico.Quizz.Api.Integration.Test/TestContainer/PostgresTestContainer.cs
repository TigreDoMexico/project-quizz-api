using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;

namespace TigreDoMexico.Quizz.Api.Integration.Test.TestContainer;

public class PostgresTestContainer
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithDatabase("db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
        .WithCleanUp(true)
        .Build();
    
    public PostgreSqlContainer Container => _container;
    
    public async Task IniciarAsync() => await _container.StartAsync();
    
    public async Task FinalizarAsync() => await _container.DisposeAsync();
}