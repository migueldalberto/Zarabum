class Diagnostic
{
    public string Message { get; }
    public int? Position { get; }

    public Diagnostic(string Message)
    {
        this.Message = Message;
        this.Position = null;
    }

    public Diagnostic(string Message, int Position)
    {
        this.Message = Message;
        this.Position = Position;
    }
}

class ErrorDiagnostic : Diagnostic
{
    public ErrorDiagnostic(string Message, int Position) : base(Message, Position) { }
}

class LogDiagnostic : Diagnostic
{
    public LogDiagnostic(string Message, int Position) : base(Message, Position) { }
}

class DebugDiagnostic : Diagnostic
{
    public DebugDiagnostic(string Message, int Position) : base(Message, Position) { }
}

class WarnDiagnostic : Diagnostic
{
    public WarnDiagnostic(string Message, int Position) : base(Message, Position) { }
}

static class DiagnosticPrinter
{
    public static void Print(this Diagnostic diagnostic)
    {
        if (diagnostic is ErrorDiagnostic)
            _Print(diagnostic, ConsoleColor.Red, "ERROR");
        else if (diagnostic is DebugDiagnostic)
            _Print(diagnostic, ConsoleColor.Blue, "DEBUG");
        else if (diagnostic is WarnDiagnostic)
            _Print(diagnostic, ConsoleColor.Yellow, "DEBUG");
        else
            _Print(diagnostic, Console.ForegroundColor, "LOG");
    }

    private static void _Print(Diagnostic diagnostic, ConsoleColor color, string prefix)
    {
        Console.ForegroundColor = color;
        Console.Write($"[{prefix}] ");
        Console.ResetColor();
        if (diagnostic.Position != null)
            Console.Write("[at " + diagnostic.Position.ToString() + "] ");
        Console.Write($"- {diagnostic.Message}\n");
    }

    
}
