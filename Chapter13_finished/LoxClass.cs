using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public class LoxClass : ILoxCallable
    {
        public readonly string Name;
        public readonly LoxClass Superclass;
        private readonly Dictionary<string, LoxFunction> Methods;

        public LoxClass(string name, LoxClass superclass, Dictionary<string, LoxFunction> methods)
        {
            this.Name = name;
            this.Superclass = superclass;
            this.Methods = methods;
        }
        public LoxFunction FindMethod(string name)
        {
            if (Methods.ContainsKey(name))
            {
                return Methods[name];
            }
            if (Superclass != null)
            {
                return Superclass.FindMethod(name);
            }
            return null;
        }
        public override string ToString()
        {
            return Name;
        }
        public object Call(Interpreter interpreter, List<object> arguments)
        {
            LoxInstance instance = new LoxInstance(this);
            LoxFunction initializer = FindMethod("init");
            if (initializer != null)
            {
                initializer.Bind(instance).Call(interpreter, arguments);
            }
            return instance;
        }

        public int Arity
        {
            get
            {
                if (Methods.TryGetValue("init", out var init))
                {
                    return init.Arity;
                }
                return 0;
            }
        }


    }

}
