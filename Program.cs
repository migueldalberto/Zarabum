namespace Zarabum.interpreter;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 1)
        {
            string? filePath = Path.GetFullPath(args[0]);

            if (!File.Exists(filePath))
            {
                Console.WriteLine("No such file: " + filePath);
                System.Environment.Exit(1);
            }

            string source = File.ReadAllText(filePath);

            _Run(source);
        }
        else
        {
            while (true)
            {
                Console.Write("> ");
                string? source = Console.ReadLine();

                if (string.IsNullOrEmpty(source)) return;

                _Run(source);
            }
        }
    }

    private static void _Run(string s)
    {
        Lexer lexer = new Lexer(s);
        lexer.getTokens();

        lexer.Diagnostics.ForEach((d) => d.Print());
        if (lexer.Diagnostics.Any((d) => d is ErrorDiagnostic)) return;

        Parser parser = new Parser(lexer);
        parser.Parse();

        parser.Diagnostics.ForEach((d) => d.Print());

        foreach (Expression expr in parser.expressions)
        {
            Console.WriteLine(expr.Stringify());
            Console.WriteLine(expr.Evaluate());
        }
    }
}
