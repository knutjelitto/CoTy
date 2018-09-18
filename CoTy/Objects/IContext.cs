using System.Collections.Generic;

namespace CoTy.Objects
{
    public interface IContext
    {
        string Name { get; }
        IContext Parent { get; }
        IContext Scope { get; }
        IEnumerable<Symbol> Symbols { get; }

        void Define(string symbol, object value, bool isSealed = false, bool isOpaque = false);
        void Define(Symbol symbol, object value, bool isSealed = false, bool isOpaque = false);
        Binding Find(Symbol symbol);
        void Get(Symbol symbol, out object value);
        bool IsDefined(Symbol symbol);
        IContext Pop();
        IContext Push(string name);
        bool TryFind(Symbol symbol, out Binding binding);
        void Undefine(Symbol symbol);
        void Update(Symbol symbol, object value);
    }
}