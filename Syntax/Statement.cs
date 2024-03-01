namespace Zarabum.Syntax;

interface Statement;

class ExpressionStatement : Statement
{
    public readonly Expression expression;

    public ExpressionStatement(Expression expression)
    {
        this.expression = expression;
    }
}

class PrintStatement : Statement
{
    public readonly Expression expression;

    public PrintStatement(Expression expression)
    {
        this.expression = expression;
    }
}

class DeclarationStatement : Statement
{
    public readonly Identifier identifier;
    public readonly Expression expression;

    public DeclarationStatement(Identifier identifier, Expression expression)
    {
        this.identifier = identifier;
        this.expression = expression;
    }
}
