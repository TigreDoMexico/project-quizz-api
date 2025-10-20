# Quizz Api
Projeto .NET para gerenciamento de perguntas para o app de perguntas QUIZZ.

## Como Iniciar Localmente

```bash
dotnet ef migrations add PrimeiraMigration
dotnet ef database update
```

## Tecnologias Utilizadas
- [.NET 8](https://learn.microsoft.com/pt-br/dotnet/core/whats-new/dotnet-8/overview)
- [MediatR](https://github.com/LuckyPennySoftware/MediatR/wiki)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [OpenTelemetry](https://opentelemetry.io/docs/languages/dotnet/) para Observabilidade
- [Serilog](https://serilog.net/)
- Banco de Dados: [Postgres](https://www.postgresql.org/docs/)
