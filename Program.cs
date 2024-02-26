namespace Zarabum.interpreter;

class Program
{
    static string? Source;

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

            Source = File.ReadAllText(filePath);

            _Run(Source);
        }
        else
        {
            while (true)
            {
                Console.Write("> ");
                string? Source = Console.ReadLine();

                if (string.IsNullOrEmpty(Source)) return;

                _Run(Source);
            }
        }
    }

    private static void _Run(string s)
    {
        Scanner scanner = new Scanner(s);
        scanner.getTokens();

        // scanner.tokens.ForEach(
             // (Token t) => Console.WriteLine(
                 // "[position " + t.position +
                 // "] " + t.type +
                 // " " + t.lexeme +
                 // " " + t.literal)
             // );

        scanner.Diagnostics.ForEach((d) => d.Print());
        if (scanner.Diagnostics.Any((d) => d is ErrorDiagnostic)) return;

        Parser parser = new Parser(scanner);
        parser.Parse();

        parser.Diagnostics.ForEach((d) => d.Print());

        foreach (Expression expr in parser.expressions)
        {
            Console.WriteLine(expr.Stringify());
            Console.WriteLine(expr.Evaluate());
        }
    }
}
