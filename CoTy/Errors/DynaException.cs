using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoTy.Errors
{
    public class DynaException : CotyException
    {
        public DynaException(string name, Type result, params Type[] arguments)
            : this($"can't dynamic bind `{name}: {string.Join(" x ", arguments.Select(arg => arg.Name))} -> {result.Name}´")
        {
        }
        private DynaException(string message) : base(message)
        {
        }
    }
}
