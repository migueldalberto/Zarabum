namespace Zarabum.interpreter;

class Parser
{
    public readonly Lexer lexer;
    private int _tokenPosition = 0;

    private List<Expression> _expressions = new List<Expression>();
    public List<Expression> expressions { get { return _expressions; } }

    private List<Diagnostic> _diagnostics = new List<Diagnostic>();
    public List<Diagnostic> Diagnostics { get { return _diagnostics; } }

    public List<Token> Tokens { get { return lexer.Tokens; } }

    public Token Current
    {
        get { return Tokens[_tokenPosition]; }
    }

    public Parser(string source)
    {
        lexer = new Lexer(source);
        lexer.getTokens();
    }

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;

        if (Tokens.Count() > 0)
        {
            lexer.getTokens();
        }
    }

    public List<Expression> Parse()
    {
        while (Current.type != TokenType.EOF)
        {
            var expr = _ParseExpression();
            _expressions.Add(expr);
            ++_tokenPosition;
        }

        return _expressions;
    }

    private Expression _ParseExpression()
    {
        return _ParseTerm();
    }

    private Expression _ParseTerm()
    {
        var leftOperand = _ParseFactor();

        while (
                Current.type == TokenType.PLUS ||
                Current.type == TokenType.MINUS
            )
        {
            var operatorToken = Current;
            ++_tokenPosition;
            var rightOperand = _ParseFactor();
            leftOperand = new Binary(operatorToken, leftOperand, rightOperand);
        }

        return leftOperand;
    }

    private Expression _ParseFactor()
    {
        var leftOperand = _ParsePrimaryExpression();

        while (
                Current.type == TokenType.STAR ||
                Current.type == TokenType.SLASH
            )
        {
            var operatorToken = Current;
            ++_tokenPosition;
            var rightOperand = _ParsePrimaryExpression();
            leftOperand = new Binary(operatorToken, leftOperand, rightOperand);
        }

        return leftOperand;

    }

    private Expression _ParsePrimaryExpression()
    {
        if (Current.type == TokenType.LEFT_PAREN)
        {
            ++_tokenPosition;
            var expression = _ParseExpression();
            _Match(TokenType.RIGHT_PAREN);
            return new Grouping(expression);
        }
        else if (Current.type == TokenType.MINUS)
        {
            var operatorToken = Current;
            ++_tokenPosition;
            var expression = _ParseExpression();
            return new Unary(operatorToken, expression);
        }
        var numberToken = _Match(TokenType.NUMBER);
        return new Literal(numberToken.literal);
    }

    private Token _Match(TokenType type)
    {
        if (Current.type == type)
        {
            ++_tokenPosition;
            return Tokens[_tokenPosition - 1];
        }

        _diagnostics.Add(new ErrorDiagnostic($"unexpected token <{Current.type}>, expected <{type}>", Current.position));
        return new Token(type, "", null, Current.position);
    }
}
