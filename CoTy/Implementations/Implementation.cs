using System;
using System.Reflection;

namespace CoTy.Implementations
{
    public class Implementation
    {
        public MethodInfo Get(string name, params Type[] parameterTypes)
        {
            return GetType().GetMethod(name, parameterTypes);
        }
    }
}
