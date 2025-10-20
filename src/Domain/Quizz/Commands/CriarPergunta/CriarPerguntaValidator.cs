using FluentValidation;

namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;

public class CriarPerguntaValidator : AbstractValidator<CriarPerguntaCommand>
{
    public CriarPerguntaValidator()
    {
        this.RuleFor(x => x.Enunciado)
            .NotEmpty()
            .WithMessage(CriarPerguntaMessages.ErroEnunciadoNulo)
            .MaximumLength(500)
            .WithMessage(CriarPerguntaMessages.ErroEnunciadoMaiorQueEsperado);

        this.RuleFor(x => x.Categoria)
            .IsInEnum()
            .WithMessage(CriarPerguntaMessages.ErroCategoriaInvalida);

        this.RuleFor(x => x.Respostas)
            .NotEmpty()
            .WithMessage(CriarPerguntaMessages.ErroPerguntasVazias)
            .Must(respostas => respostas.Count(resposta => resposta.Correta) == 1)
            .WithMessage(CriarPerguntaMessages.ErroSemRespostaCorreta);

        this.RuleForEach(x => x.Respostas)
            .ChildRules(resposta => resposta.RuleFor(r => r.Enunciado).MaximumLength(500))
            .WithMessage(CriarPerguntaMessages.ErroEnunciadoMaiorQueEsperado);
    }
}