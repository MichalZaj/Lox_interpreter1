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
        private readonly bool isInitializer;
        public LoxFunction(Stmt.Function declaration, EnvironmentLox closure, bool isInitializer)
        {
            this.isInitializer = isInitializer;
            this.closure = closure;
            this.declaration = declaration;
        }
        public LoxFunction Bind(LoxInstance instance)
        {
            EnvironmentLox environment = new EnvironmentLox(closure);
            environment.Define("this", instance);
            return new LoxFunction(declaration, environment,isInitializer);

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
                if (isInitializer) return closure.GetAt(0, "this");
                return returnValue.Value;
            }
            if (isInitializer)
            {
                return closure.GetAt(0, "this");
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
