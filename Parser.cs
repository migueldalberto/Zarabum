namespace Zarabum.interpreter;

class Parser
{
    Scanner scanner;
    private int _currentIndex = 0;

    private List<Expression> _expressions = new List<Expression>();
    public List<Expression> expressions { get { return _expressions; } }

    public Token Current
    {
        get { return scanner.tokens[_currentIndex]; }
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
        while (Current.type != TokenType.EOF) {
            var leftOperand = _ParsePrimaryExpression();

            while (
                    Current.type == TokenType.PLUS ||
                    Current.type == TokenType.MINUS
                )
            {
                var operatorToken = Current;
                ++_currentIndex;
                var rightOperand = _ParsePrimaryExpression();
                leftOperand = new Binary(operatorToken, leftOperand, rightOperand);
            }

            _expressions.Add(leftOperand);
            ++_currentIndex;
        }

        return _expressions;
    }

    private Expression _ParsePrimaryExpression()
    {
        var numberToken = _Match(TokenType.NUMBER);
        return new Literal(numberToken.literal);
    }

    private Token _Match(TokenType type)
    {
        if (Current.type == type)
        {
            ++_currentIndex;
            return scanner.tokens[_currentIndex - 1];
        }

        Program.Error(Current.position, $"unexpected token <{Current.type}>, expected <{type}>");
        return new Token(type, "", null, Current.position);
    }
}
