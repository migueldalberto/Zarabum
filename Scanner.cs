namespace Zarabum.interpreter;

class Scanner
{
    private readonly string _source;
    private int _start;
    private int _position;
    private int _line;

    private List<Token> _tokens;
    public List<Token> tokens { get { return _tokens; } }

    public bool hadError;

    public Scanner(string source)
    {
        _source = source;
        _tokens = new List<Token>();
        hadError = false;
    }

    private char _currentChar
    {
        get { return _sourceAt(_position); }
    }

    private char _preview
    {
        get
        {
            return _sourceAt(_position + 1);
        }
    }

    private char _sourceAt(int i) => i < _source.Length ? _source[i] : '\0';

    private string _substr
    {
        get
        {
            return _source.Substring(_start, _position - _start);
        }
    }

    public List<Token> getTokens()
    {
        while (_currentChar != '\0')
        {
            _start = _position;
            switch (_currentChar)
            {
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    ++_line;
                    break;
                case '(': _addToken(TokenType.LEFT_PAREN); break;
                case ')': _addToken(TokenType.RIGHT_PAREN); break;
                case '+': _addToken(TokenType.PLUS); break;
                case '-': _addToken(TokenType.MINUS); break;
                case '*': _addToken(TokenType.STAR); break;
                default:
                    if (char.IsDigit(_currentChar))
                    {
                        while (char.IsDigit(_currentChar) || (_currentChar.Equals('.') && char.IsDigit(_preview)))
                            ++_position;

                        _addToken(TokenType.NUMBER, double.Parse(_substr));
                        --_position;
                    } else if (char.IsLetter(_currentChar)) {
                    } else {
                        hadError = true;
                        Program.Error(_position, "invalid character '" + _currentChar + "'");
                    }
                    break;
            }

            ++_position;
        }

        return _tokens;
    }

    private void _addToken(TokenType type) =>
        _addToken(type, null);

    private void _addToken(TokenType type, object? literal) =>
        _tokens.Add(new Token(type, _substr, literal, _position));

}
