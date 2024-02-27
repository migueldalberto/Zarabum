namespace Zarabum.interpreter;

static class Evaluator
{
    public static double Evaluate(this Expression expression) => expression switch
    {
        Literal literal => literal.value == null ? 0.0 : (double)literal.value,
        Binary binary => EvaluateBinary(binary),
        Unary unary => EvaluateUnary(unary),
        Grouping grouping => Evaluate(grouping.expression),
        _ => throw new Exception("unexpected expression")
    };

    public static double EvaluateBinary(Binary expr)
    {
        var leftOperand = expr.leftOperand.Evaluate();
        var rightOperand = expr.rightOperand.Evaluate();

        return expr.operatorToken.type switch {
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
            return - Evaluate(expr.rightOperand);
        else
            throw new Exception($"unexpected unary operator <{expr.operatorToken.type}>");
    }
}
