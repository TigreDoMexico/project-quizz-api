using FluentValidation;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Queries.ObterPorCategoria;

public class ObterPorCategoriaValidator : AbstractValidator<ObterPorCategoriaQuery>
{
    public ObterPorCategoriaValidator()
    {
        this.RuleFor(x => x.Categoria)
            .IsInEnum()
            .WithMessage(ObterPorCategoriaMessages.ErroCategoriaInvalida);
        
        this.RuleFor(x => x.Limite)
            .GreaterThan(0)
            .WithMessage(ObterPorCategoriaMessages.ErroLimiteInvalido);
    }
}