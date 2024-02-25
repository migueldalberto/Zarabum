namespace Zarabum.interpreter;

enum TokenType
{
    LEFT_PAREN, RIGHT_PAREN,
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
    ESCREVA, LEIA
}

class Token
{
    public TokenType type;
    public string lexeme;
    public object? literal;
    public int position;

    public Token(TokenType type, string lexeme, object? literal, int position)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.position = position;
    }
}
