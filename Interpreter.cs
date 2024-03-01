namespace Zarabum.interpreter;

static class Interpreter
{
    static private Environment _environment = new Environment();

    public static void Interpret(this Statement statement)
    {
        if (statement is DeclarationStatement declaration)
            _environment.Define(declaration.identifier.name, declaration.expression.Evaluate());
        else if (statement is PrintStatement printStatement)
            Console.WriteLine(printStatement.expression.Evaluate());
        else if (statement is ExpressionStatement expressionStatement)
            expressionStatement.expression.Evaluate();
        else
            throw new Exception($"unexpected statement");
    }

    public static double Evaluate(this Expression expression) => expression switch
    {
        Literal literal => literal.value == null ? 0.0 : (double)literal.value,
        Binary binary => EvaluateBinary(binary),
        Unary unary => EvaluateUnary(unary),
        Grouping grouping => Evaluate(grouping.expression),
        Identifier identifier => (Double)_environment.Get(identifier.name),
        _ => throw new Exception("unexpected expression")
    };

    public static double EvaluateBinary(Binary expr)
    {
        var leftOperand = expr.leftOperand.Evaluate();
        var rightOperand = expr.rightOperand.Evaluate();

        return expr.operatorToken.type switch
        {
            TokenType.PLUS => leftOperand + rightOperand,
            TokenType.MINUS => leftOperand - rightOperand,
            TokenType.STAR => leftOperand * rightOperand,
            TokenType.SLASH => leftOperand / rightOperand,
            _ => throw new Exception($"unexpected operator <{expr.operatorToken.type}>")
        };
    }

    public static double EvaluateUnary(Unary expr)
    {
        if (expr.operatorToken.type == TokenType.MINUS)
            return -Evaluate(expr.rightOperand);
        else
            throw new Exception($"unexpected unary operator <{expr.operatorToken.type}>");
    }
}
