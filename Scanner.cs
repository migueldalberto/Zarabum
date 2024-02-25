namespace Zarabum.interpreter;

class Scanner
{
    private readonly string _source;
    private int _start;
    private int _position;
    private int _line;

    private List<Token> _tokens;
    public List<Token> tokens { get { return _tokens; } }

    private bool _hadError;
    public bool hadError { get { return _hadError; } }

    private Dictionary<string, TokenType> _keywords = 
        new Dictionary<string, TokenType>();

    public Scanner(string source)
    {
        _source = source;
        _tokens = new List<Token>();
        _hadError = false;

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
        _keywords.Add("escreva", TokenType.ESCREVA);
        _keywords.Add("leia", TokenType.LEIA);
        _keywords.Add("nulo", TokenType.NULO);
    }

    private char _currentChar
    {
        get { return _sourceAt(_position); }
    }

    private char _preview
    {
        get { return _sourceAt(_position + 1); }
    }

    private char _sourceAt(int i) => i < _source.Length ? _source[i] : '\0';

    private string _substr {
        get { return _source.Substring(_start, _position - _start); }
    }

    private bool _match (char e) {
        if (_preview == e) {
            ++_position;
            return true;
        } else 
            return false;
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
                case '{': _addToken(TokenType.LEFT_BRACE); break;
                case '}': _addToken(TokenType.RIGHT_BRACE); break;
                case ',': _addToken(TokenType.COMMA); break;
                case '.': _addToken(TokenType.DOT); break;
                case ';': _addToken(TokenType.SEMICOLON); break;
                case '+': _addToken(TokenType.PLUS); break;
                case '-': _addToken(TokenType.MINUS); break;
                case '*': _addToken(TokenType.STAR); break;
                case '=': _addToken(_match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '!': _addToken(_match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '>': _addToken(_match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER_EQUAL); break;
                case '<': _addToken(_match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '/':
                      if (_preview == '/') {
                          while (_currentChar != '\0' && _currentChar != '\n') 
                              ++_position;
                      } else {
                          _addToken(TokenType.SLASH);
                      }
                      break;
                case '"':
                      if (_currentChar == '"') {
                          ++_position;
                          while (_currentChar != '"') {
                              if (_currentChar == '\n' || _currentChar == '\0') {
                                  _hadError = true;
                                  Program.Error(_position, "open string reached end of line or end of file, missing \"");
                                  break;
                              }
                              ++_position;
                          }

                          if (_currentChar == '"')
                              _addToken(
                                      TokenType.STRING, 
                                      _source.Substring(_start, _position + 1 - _start), 
                                      _source.Substring(_start + 1, _position - _start - 1)
                                      );
                      }
                      break;
                default:
                    if (char.IsDigit(_currentChar))
                    {
                        while (char.IsDigit(_currentChar) || (_currentChar.Equals('.') && char.IsDigit(_preview)))
                            ++_position;

                        _addToken(TokenType.NUMBER, double.Parse(_substr));
                        --_position;
                    } else if (char.IsLetter(_currentChar) || _currentChar.Equals('_')) {
                        while (char.IsLetterOrDigit(_currentChar) || _currentChar.Equals('_'))
                            ++_position;

                        if (_keywords.ContainsKey(_substr))
                            _addToken(_keywords[_substr]);
                        else 
                            _addToken(TokenType.IDENTIFIER, _substr);

                        --_position;
                    } else {
                        _hadError = true;
                        Program.Error(_position, "invalid character '" + _currentChar + "'");
                    }
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
        _addToken(type, _substr, literal);
    private void _addToken(TokenType type, string lexeme, object? literal) =>
        _tokens.Add(new Token(type, lexeme, literal, _start));
}
