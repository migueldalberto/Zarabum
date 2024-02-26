namespace Zarabum.interpreter;

static class Evaluator
{
    public static double Evaluate(this Expression expression) => expression switch
    {
        Literal literal => literal.value == null ? 0.0 : (double)literal.value,
        Binary binary => EvaluateBinary(binary),
        _ => throw new Exception("unexpected expression")
    };

    public static double EvaluateBinary(Binary expr)
    {
        var leftOperand = expr.leftOperand.Evaluate();
        var rightOperand = expr.rightOperand.Evaluate();

        if (expr.operatorToken.type == TokenType.PLUS)
        {
            return leftOperand + rightOperand;
        }
        else if (expr.operatorToken.type == TokenType.MINUS)
        {
            return leftOperand - rightOperand;
        }
        else if (expr.operatorToken.type == TokenType.STAR)
        {
            return leftOperand * rightOperand;
        }
        else if (expr.operatorToken.type == TokenType.SLASH)
        {
            return leftOperand / rightOperand;
        }
        else
        {
            throw new Exception($"unexpected operator <{expr.operatorToken.type}>");
        }
    }
}
