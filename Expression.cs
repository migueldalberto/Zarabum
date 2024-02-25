namespace Zarabum.interpreter;

interface Expression;

class Binary : Expression
{
    Token operatorToken;
    Expression left;
    Expression right;

    Binary (Token operatorToken, Expression left, Expression right)
    {
        this.operatorToken = operatorToken;
        this.left = left;
        this.right = right;
    }
}

class Unary : Expression
{
    Token operatorToken;
    Expression right;

    Unary (Token operatorToken, Expression right)
    {
        this.operatorToken = operatorToken;
        this.right = right;
    }
}

class Literal : Expression
{
    object value;

    Literal(object value)
    {
        this.value = value;
    }
}

class Grouping : Expression
{
    Expression expression;

    Grouping(Expression expression)
    {
        this.expression = expression;
    }
}

