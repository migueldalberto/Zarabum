namespace Zarabum.interpreter;

enum TokenType
{
    LEFT_PAREN, RIGHT_PAREN,
    LEFT_BRACE, RIGHT_BRACE,
    COMMA, DOT, SEMICOLON,
    PLUS, MINUS, STAR,

    EQUAL, EQUAL_EQUAL,
    BANG, BANG_EQUAL,
    GREATER, GREATER_EQUAL,
    LESS, LESS_EQUAL, SLASH,

    NUMBER, STRING, IDENTIFIER,

    VAR, NAO, E, OU, XOU,
    SE, SENAO,
    ESCOLHA, CASO, OUTROCASO,
    PARA, ENQUANTO,
    FUNC,
    ESCREVA, LEIA,
    NULO,

    EOF
}

class Token
{
    public readonly TokenType type;
    public readonly string lexeme;
    public readonly object? literal;
    public readonly int position;

    public Token(TokenType type, string lexeme, object? literal, int position)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.position = position;
    }
}
