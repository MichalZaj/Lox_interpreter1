using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public class GenerateAst
    {
/*        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }
            string outputDir = args[0];

            DefineAst(outputDir, "Expr", new List<string>
            {
                "Assign   : Token name, Expr value",
                "Binary   : Expr left, Token operator, Expr right",
                "Grouping : Expr expression",
                "Literal  : object value",
                "Unary    : Token operator, Expr right",
                "Variable : Token name"
            });
            DefineAst(outputDir, "Stmt", new List<string>
            {
                "Block      : List<Stmt> statements",
                "Expression : Expr expression",
                "Print      : Expr expression",
                "Var        : Token name, Expr initializer"
            });
        }*/

        private static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            string path = Path.Combine(outputDir, baseName + ".cs");
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("namespace Lox_Interpreter1.Lox");
                writer.WriteLine("{");
                writer.WriteLine("    using System.Collections.Generic;");
                writer.WriteLine();
                writer.WriteLine($"    abstract class {baseName}");
                writer.WriteLine("    {");
                DefineVisitor(writer, baseName, types);
                foreach (string type in types)
                {
                    string[] typeSplit = type.Split(':');
                    string className = typeSplit[0].Trim();
                    string fields = typeSplit[1].Trim();
                    DefineType(writer, baseName, className, fields);
                }
                writer.WriteLine();
                writer.WriteLine("  public abstract R Accept<R>(IVisitor<R> visitor);");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }
        private static void DefineType(
            StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine($"  static class {className} : {baseName} {{");

            // Constructor.
            writer.WriteLine($"    {className}({fieldList}) {{");

            // Store parameters in fields.
            string[] fields = fieldList.Split(new string[] { ", " }, StringSplitOptions.None);
            foreach (string field in fields)
            {
                string name = field.Split(' ')[1];
                writer.WriteLine($"      this.{name} = {name};");
            }

            writer.WriteLine("    }");
            writer.WriteLine();
            writer.WriteLine("    public override R Accept<R>(IVisitor<R> visitor)");
            writer.WriteLine("    {");
            writer.WriteLine($"      return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("    }");
            // Fields.
            writer.WriteLine();
            foreach (string field in fields)
            {
                writer.WriteLine($"    public readonly {field};");
            }

            writer.WriteLine("  }");
        }

        private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine("  public interface IVisitor<R>");
            writer.WriteLine("  {");

            foreach (string type in types)
            {
                string typeName = type.Split(':')[0].Trim();
                writer.WriteLine($"    R Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
            }

            writer.WriteLine("  }");
            writer.WriteLine();
        }

        /*public interface IPastryVisitor
        {
            void VisitBeignet(Beignet beignet);
            void VisitCruller(Cruller cruller);
        }

        public abstract class Pastry
        {
            public abstract void Accept(IPastryVisitor visitor);
        }

        public class Beignet : Pastry
        {
            public override void Accept(IPastryVisitor visitor)
            {
                visitor.VisitBeignet(this);
            }
        }

        public class Cruller : Pastry
        {
            public override void Accept(IPastryVisitor visitor)
            {
                visitor.VisitCruller(this);
            }
        }*/
    }
}
