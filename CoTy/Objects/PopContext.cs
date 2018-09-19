using System;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public class PopContext : IScope
    {
        public string Name => throw new NotImplementedException();

        public IScope Parent => throw new NotImplementedException();

        public IEnumerable<Symbol> Symbols => throw new NotImplementedException();

        public void Define(string symbol, object value, bool isSealed = false, bool isOpaque = false)
        {
            throw new NotImplementedException();
        }

        public void Define(Symbol symbol, object value, bool isSealed = false, bool isOpaque = false)
        {
            throw new NotImplementedException();
        }

        public Binding Find(Symbol symbol)
        {
            throw new NotImplementedException();
        }

        public void Get(Symbol symbol, out object value)
        {
            throw new NotImplementedException();
        }

        public bool IsDefined(Symbol symbol)
        {
            throw new NotImplementedException();
        }

        public IScope Pop()
        {
            throw new NotImplementedException();
        }

        public IScope Push(string name)
        {
            throw new NotImplementedException();
        }

        public bool TryFind(Symbol symbol, out Binding binding)
        {
            throw new NotImplementedException();
        }

        public void Undefine(Symbol symbol)
        {
            throw new NotImplementedException();
        }

        public void Update(Symbol symbol, object value)
        {
            throw new NotImplementedException();
        }
    }
}
