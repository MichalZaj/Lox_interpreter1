using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public class EnvironmentLox
    {
        public readonly EnvironmentLox enclosing;
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();
        public EnvironmentLox()
        {
            enclosing = null;
        }

        public EnvironmentLox(EnvironmentLox enclosing)
        {
            this.enclosing = enclosing;
        }
        public void Define(string name, object value)
        {
            values[name] = value;
        }
        public object Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }
            if (enclosing != null)
            {
                return enclosing.Get(name);
            }

            throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }
        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
                return;
            }
            if (enclosing != null)
            {
                enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name,
                "Undefined variable '" + name.Lexeme + "'.");
        }

    }

}
