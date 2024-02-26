namespace Zarabum.interpreter;

class Parser
{
    Scanner scanner;
    private int _tokenListPosition = 0;

    private List<Expression> _expressions = new List<Expression>();
    public List<Expression> expressions { get { return _expressions; } }

    private List<Diagnostic> _diagnostics = new List<Diagnostic>();
    public List<Diagnostic> Diagnostics { get { return _diagnostics; } }

    public Token Current
    {
        get { return scanner.tokens[_tokenListPosition]; }
    }

    public Parser(string source)
    {
        scanner = new Scanner(source);
        scanner.getTokens();
    }

    public Parser(Scanner scanner)
    {
        this.scanner = scanner;

        if (scanner.tokens.Count() > 0)
        {
            scanner.getTokens();
        }
    }

    public List<Expression> Parse()
    {
        while (Current.type != TokenType.EOF)
        {
            var expr = _ParseExpression();
            _expressions.Add(expr);
            ++_tokenListPosition;
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
                ++_tokenListPosition;
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
                ++_tokenListPosition;
                var rightOperand = _ParsePrimaryExpression();
                leftOperand = new Binary(operatorToken, leftOperand, rightOperand);
            }

            return leftOperand;

    }

    private Expression _ParsePrimaryExpression()
    {
        if (Current.type == TokenType.LEFT_PAREN) {
            ++_tokenListPosition;
            var expression = _ParseExpression();
            _Match(TokenType.RIGHT_PAREN);
            return new Grouping(expression);
        }
        var numberToken = _Match(TokenType.NUMBER);
        return new Literal(numberToken.literal);
    }

    private Token _Match(TokenType type)
    {
        if (Current.type == type)
        {
            ++_tokenListPosition;
            return scanner.tokens[_tokenListPosition - 1];
        }

        _diagnostics.Add(new ErrorDiagnostic($"unexpected token <{Current.type}>, expected <{type}>", Current.position));
        return new Token(type, "", null, Current.position);
    }
}
