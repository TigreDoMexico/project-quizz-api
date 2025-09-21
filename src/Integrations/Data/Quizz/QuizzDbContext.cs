using Microsoft.EntityFrameworkCore;
using TigreDoMexico.Quizz.Api.Domain.Quizz.Entities;

namespace TigreDoMexico.Quizz.Api.Integrations.Data.Quizz;

public class QuizzDbContext(DbContextOptions<QuizzDbContext> options) : DbContext(options)
{
    public DbSet<Pergunta> Perguntas => Set<Pergunta>();
    
    public DbSet<Resposta> Respostas => Set<Resposta>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pergunta>().ToTable("pergunta");
        modelBuilder.Entity<Resposta>().ToTable("resposta");
        
        base.OnModelCreating(modelBuilder);
    }
}