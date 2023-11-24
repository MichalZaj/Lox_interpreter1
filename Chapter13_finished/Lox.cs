using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public class Lox
    {
        private static readonly Interpreter Interpreter = new Interpreter();
        static bool HadError = false;
        static bool HadRuntimeError = false;
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length > 1)
                {
                    Console.WriteLine("Usage: clox [script]");
                    Environment.Exit(64);
                }
                else if (args.Length == 1)
                {
                    RunFile(args[0]);
                }
                else
                {
                    RunPrompt();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                Environment.Exit(74);
            }
        }

        private static void RunFile(string path)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                string content = Encoding.Default.GetString(bytes);
                Run(content);

                if (HadError)
                {
                    Environment.Exit(65);
                }
                if (HadRuntimeError)
                {
                    Environment.Exit(70);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file '{path}': {ex.Message}");
                Environment.Exit(74); // Exit code for IO error
            }
        }

        private static void RunPrompt()
        {
            try
            {
                for (;;)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (line == null) break;
                    Run(line);
                    HadError = false;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading input: {ex.Message}");
                Environment.Exit(74);
            }
        }
        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.Parse();

            // Stop if there was a syntax error.
            if (HadError) return;

            Resolver resolver = new Resolver(Interpreter);
            resolver.Resolve(statements);
            //Stop if there was a resoultion error
            if (HadError) return;

            Interpreter.Interpret(statements);
            //Console.WriteLine(new AstPrinter().Print(expression));
        }
        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
            HadError = true;
        }
        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }
        public static void RuntimeError(RuntimeError error)
        {
            Console.Error.WriteLine($"{error.Message}\n[line {error.Token.Line}]");
            HadRuntimeError = true;
        }

    }

}
