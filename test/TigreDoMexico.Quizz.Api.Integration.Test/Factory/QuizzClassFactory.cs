using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TigreDoMexico.Quizz.Api.Integration.Test.TestContainer;
using TigreDoMexico.Quizz.Api.Integrations.Data.Quizz;

namespace TigreDoMexico.Quizz.Api.Integration.Test.Factory;

public class QuizzClassFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgresTestContainer _postgresContainer = new();
    
    private string? _postgresConnectionString = null;

    public QuizzDbContext DbContext { get; private set; } = null!;
    public DbConnection DbConnection { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _postgresContainer.IniciarAsync();
        _postgresConnectionString = _postgresContainer.Container.GetConnectionString();
        await ExecutarMigrationAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.FinalizarAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureTestServices(ConfigurarServiceCollection);
    }

    private void ConfigurarServiceCollection(IServiceCollection services)
    {
        RemoverDbContextConfigurado(services);
        services.AddDbContext<QuizzDbContext>(options => 
        {
            var connectionString = _postgresConnectionString ?? throw new InvalidOperationException("Connection string not initialized");
            options.UseNpgsql(connectionString);
        });
    }

    private async Task ExecutarMigrationAsync()
    {
        this.DbContext = this.Services.CreateScope().ServiceProvider.GetRequiredService<QuizzDbContext>();
        this.DbConnection = this.DbContext.Database.GetDbConnection();
        
        await this.DbConnection.OpenAsync();
        await this.DbContext.Database.MigrateAsync();
    }
    
    private static void RemoverDbContextConfigurado(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContext));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }
}