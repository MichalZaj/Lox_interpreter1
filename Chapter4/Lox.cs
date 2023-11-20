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
        static bool HadError = false;
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

            // For now, just print the tokens.
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
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


    }

}
