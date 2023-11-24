using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lox_Interpreter1.TokenType;

namespace Lox_Interpreter1
{
    public class Scanner
    {
        private readonly string source;
        private readonly List<Token> Tokens = new List<Token>();
        private int Start = 0;
        private int Current = 0;
        private int Line = 1;

        public Scanner(string source)
        {
            this.source = source;
        }
        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                // We are at the beginning of the next lexeme.
                Start = Current;
                ScanToken();
            }

            Tokens.Add(new Token(TokenType.EOF, "", null, Line));
            return Tokens;
        }
        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;

                case '\n':
                    Line++;
                    break;
                case '"':
                    String();
                    break;
                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        Lox.Error(Line, "Unexpected character.");
                    }

                    break;
            }
        }
        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();
            string text = source.Substring(Start, Current - Start);
            TokenType type;

            if (!keywords.TryGetValue(text, out type))
            {
                type = TokenType.IDENTIFIER;
            }
            AddToken(type);
        }
        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            // Look for a fractional part.
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Consume the "."
                Advance();

                while (IsDigit(Peek())) Advance();
            }

            AddToken(TokenType.NUMBER,
                double.Parse(source.Substring(Start, Current - Start)));
        }
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[Current] != expected) return false;

            Current++;
            return true;
        }
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[Current];
        }
        private char PeekNext()
        {
            if (Current + 1 >= source.Length) return '\0';
            return source[Current + 1];
        }
        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }
        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }
        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        private bool IsAtEnd()
        {
            return Current >= source.Length;
        }
        private char Advance()
        {
            return source[Current++];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            string text = source.Substring(Start, Current - Start);
            Tokens.Add(new Token(type, text, literal, Line));
        }
        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') Line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Lox.Error(Line, "Unterminated string.");
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            string value = source.Substring(Start + 1, Current - Start - 2);
            AddToken(TokenType.STRING, value);
        }
        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            {"and", TokenType.AND},
            {"class", TokenType.CLASS},
            {"else", TokenType.ELSE},
            {"false", TokenType.FALSE},
            {"for", TokenType.FOR},
            {"fun", TokenType.FUN},
            {"if", TokenType.IF},
            {"nil", TokenType.NIL},
            {"or", TokenType.OR},
            {"print", TokenType.PRINT},
            {"return", TokenType.RETURN},
            {"super", TokenType.SUPER},
            {"this", TokenType.THIS},
            {"true", TokenType.TRUE},
            {"var", TokenType.VAR},
            {"while", TokenType.WHILE}
        };

    }

}
