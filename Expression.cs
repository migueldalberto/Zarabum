namespace Zarabum.interpreter;

interface Expression;

class Binary : Expression
{
    public Token operatorToken;
    public Expression leftOperand;
    public Expression rightOperand;

    public Binary(Token operatorToken, Expression leftOperand, Expression rightOperand)
    {
        this.operatorToken = operatorToken;
        this.leftOperand = leftOperand;
        this.rightOperand = rightOperand;
    }
}

class Unary : Expression
{
    public Token operatorToken;
    public Expression rightOperand;

    public Unary(Token operatorToken, Expression rightOperand)
    {
        this.operatorToken = operatorToken;
        this.rightOperand = rightOperand;
    }
}

class Literal : Expression
{
    public object? value;

    public Literal(object? value)
    {
        this.value = value;
    }
}

class Grouping : Expression
{
    public Expression expression;

    public Grouping(Expression expression)
    {
        this.expression = expression;
    }
}

class Identifier : Expression
{
    public readonly string name;

    public Identifier (Token token)
    {
        this.name = "" + token.literal?.ToString();
    }
}
