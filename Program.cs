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

        scanner.tokens.ForEach(
                (Token t) => Console.WriteLine(
                    "[position " + t.position +
                    "] " + t.type +
                    " " + t.lexeme +
                    " " + t.literal)
                );

        if (scanner.hadError) return;
    }

    public static void Error(int position, string msg) {
        Console.Error.WriteLine("error at l" + _CountLines(position) + ", " + msg);
    }

    private static int _CountLines(int position) => 
        string.IsNullOrEmpty(Source) ? 0 : Source.Substring(0, position).Where((c) => c == '\n').Count();
}
