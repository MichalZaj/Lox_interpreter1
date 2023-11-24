using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public interface ILoxCallable
    {
        int Arity { get; }
        object Call(Interpreter interpreter, List<object> arguments);
    }
    internal abstract class Globals : ILoxCallable
    {
        public abstract int Arity { get; }

        public abstract object Call(Interpreter interpreter, List<object> arguments);

        internal class Clock : Globals
        {
            public override int Arity { get; } = 0;

            public override object Call(Interpreter interpreter, List<object> arguments)
            {
                return (double)DateTime.Now.Millisecond / 1000.0;
            }
        }
    }

}
