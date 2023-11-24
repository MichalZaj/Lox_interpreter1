using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lox_Interpreter1
{
    public class Return : Exception
    {
        public readonly object Value;

        public Return(object value)
        {
            this.Value = value;
        }
    }
}

