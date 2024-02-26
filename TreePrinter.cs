using System.Diagnostics;

namespace Zarabum.interpreter;

static class TreePrinter
{
    public static string Stringify(this Expression expr) => expr switch
    {
        Binary binary => _Parenthesize(binary.operatorToken.lexeme, binary.leftOperand, binary.rightOperand),
        Grouping grouping => _Parenthesize("group", grouping.expression),
        Literal literal => _LiteralToString(literal),
        Unary unary => _Parenthesize(unary.operatorToken.lexeme, unary.rightOperand),
        _ => throw new UnreachableException()
    };

    private static String _Parenthesize(string name, params Expression[] exprs)
    {
        string str = $"({name}";

        foreach (Expression expr in exprs)
            str += $" {expr.Stringify()}";

        str += ")";

        return str;
    }

    private static string _LiteralToString(Literal literal)
    {
        string? value = literal.value?.ToString();

        if (value == null)
        {
            return "nulo";
        }
        else
        {
            return value;
        }
    }
}
