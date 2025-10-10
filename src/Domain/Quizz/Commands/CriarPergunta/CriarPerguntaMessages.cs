namespace TigreDoMexico.Quizz.Api.Domain.Quizz.Commands.CriarPergunta;

public class CriarPerguntaMessages
{
    public const string ErroEnunciadoNulo = "Enunciado da pergunta não pode ser nulo.";
    public const string ErroEnunciadoMaiorQueEsperado = "Enunciado deve ter no máximo 500 caracteres.";
    public const string ErroCategoriaInvalida = "ategoria deve estar entre as permitidas.";
    public const string ErroPerguntasVazias = "Não é permitido cadastrar uma pergunta sem respostas.";
    public const string ErroSemRespostaCorreta = "Deve existir somente uma resposta certa na lista de respostas.";
    
}