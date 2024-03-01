namespace Zarabum.interpreter;

class Lexer
{
    private readonly string _source;
    private int _start;
    private int _position;

    private List<Token> _tokens;
    public List<Token> Tokens { get { return _tokens; } }

    private List<Diagnostic> _diagnostics = new List<Diagnostic>();
    public List<Diagnostic> Diagnostics { get { return _diagnostics; } }

    private Dictionary<string, TokenType> _keywords =
        new Dictionary<string, TokenType>();

    private char _CurrentChar
    {
        get { return _SourceAt(_position); }
    }

    private char _Preview
    {
        get { return _SourceAt(_position + 1); }
    }

    public Lexer(string source)
    {
        _source = source;
        _tokens = new List<Token>();

        _keywords.Add("var", TokenType.VAR);
        _keywords.Add("nao", TokenType.NAO);
        _keywords.Add("e", TokenType.E);
        _keywords.Add("ou", TokenType.OU);
        _keywords.Add("xou", TokenType.XOU);
        _keywords.Add("se", TokenType.SE);
        _keywords.Add("senao", TokenType.SENAO);
        _keywords.Add("escolha", TokenType.ESCOLHA);
        _keywords.Add("caso", TokenType.CASO);
        _keywords.Add("outrocaso", TokenType.OUTROCASO);
        _keywords.Add("para", TokenType.PARA);
        _keywords.Add("enquanto", TokenType.ENQUANTO);
        _keywords.Add("func", TokenType.FUNC);
        _keywords.Add("escrever", TokenType.ESCREVER);
        _keywords.Add("ler", TokenType.LER);
        _keywords.Add("nulo", TokenType.NULO);
    }


    private char _SourceAt(int i) => i < _source.Length ? _source[i] : '\0';

    private string _GetLexemeSubstring() =>
            _source.Substring(_start, _position - _start + 1);

    private bool _Match(char e)
    {
        if (_Preview == e)
        {
            ++_position;
            return true;
        }
        else
            return false;
    }

    public List<Token> getTokens()
    {
        while (_CurrentChar != '\0')
        {
            _start = _position;
            switch (_CurrentChar)
            {
                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    break;
                case '(': _addToken(TokenType.LEFT_PAREN); break;
                case ')': _addToken(TokenType.RIGHT_PAREN); break;
                case '{': _addToken(TokenType.LEFT_BRACE); break;
                case '}': _addToken(TokenType.RIGHT_BRACE); break;
                case ',': _addToken(TokenType.COMMA); break;
                case '.': _addToken(TokenType.DOT); break;
                case ';': _addToken(TokenType.SEMICOLON); break;
                case '+': _addToken(TokenType.PLUS); break;
                case '-': _addToken(TokenType.MINUS); break;
                case '*': _addToken(TokenType.STAR); break;
                case '=': _addToken(_Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '!': _addToken(_Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '>': _addToken(_Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER_EQUAL); break;
                case '<': _addToken(_Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '/':
                    if (_Preview == '/')
                        while (_CurrentChar != '\0' && _CurrentChar != '\n')
                            ++_position;
                    else
                        _addToken(TokenType.SLASH);
                    break;
                case '"':
                    ++_position;
                    while (_CurrentChar != '"')
                    {
                        if (_CurrentChar == '\n' || _CurrentChar == '\0')
                        {
                            _diagnostics.Add(new ErrorDiagnostic("open string reached end of line or end of file, missing \"", _position));
                            break;
                        }
                        ++_position;
                    }

                    if (_CurrentChar == '"')
                        _addToken(
                                TokenType.STRING,
                                _source.Substring(_start + 1, _position - _start - 1)
                                );
                    break;
                default:
                    if (char.IsDigit(_CurrentChar))
                    {
                        while (
                                char.IsDigit(_Preview) ||
                                (_Preview.Equals('.') && char.IsDigit(_SourceAt(_position + 2)))
                            )
                            ++_position;

                        _addToken(TokenType.NUMBER, double.Parse(_GetLexemeSubstring()));
                    }
                    else if (char.IsLetter(_CurrentChar) || _CurrentChar.Equals('_'))
                    {
                        while (char.IsLetterOrDigit(_Preview) || _Preview.Equals('_'))
                            ++_position;

                        if (_keywords.ContainsKey(_GetLexemeSubstring()))
                            _addToken(_keywords[_GetLexemeSubstring()]);
                        else
                            _addToken(TokenType.IDENTIFIER, _GetLexemeSubstring());
                    }
                    else
                        _diagnostics.Add(new ErrorDiagnostic($"invalid character '{_CurrentChar}'", _position));

                    break;
            }

            ++_position;
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, _position));

        return _tokens;
    }

    private void _addToken(TokenType type) =>
        _addToken(type, null);
    private void _addToken(TokenType type, object? literal) =>
        _addToken(type, _GetLexemeSubstring(), literal);
    private void _addToken(TokenType type, string lexeme, object? literal) =>
        _tokens.Add(new Token(type, lexeme, literal, _start));
}
