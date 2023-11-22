using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lox_Interpreter1
{
    public class LoxFunction : ILoxCallable
    {
        private readonly Stmt.Function declaration;
        private readonly EnvironmentLox closure;
        public LoxFunction(Stmt.Function declaration, EnvironmentLox closure)
        {
            this.closure = closure;
            this.declaration = declaration;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            EnvironmentLox environment = new EnvironmentLox(closure);

            for (int i = 0; i < declaration.Params.Count; i++)
            {
                environment.Define(declaration.Params[i].Lexeme, arguments[i]);
            }

            try
            {
                interpreter.ExecuteBlock(declaration.Body, environment);
            }
            catch (Return returnValue)
            {
                return returnValue.Value;
            }

            return null;
        }
        public int Arity => declaration.Params.Count;
        public override string ToString()
        {
            return $"<fn {declaration.Name.Lexeme}>";

        }

    }
}
