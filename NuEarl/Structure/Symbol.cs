using System;

namespace NuEarl.Structure
{
    public abstract class Symbol : IEquatable<Symbol>
    {
        public Symbol(Grammar grammar, string name)
        {
            Grammar = grammar;
            Name = name;
        }

        public Grammar Grammar { get; }
        public string Name { get; }

        public bool Equals(Symbol other)
        {
            return other != null && other.Name == this.Name;
        }

        public override bool Equals(object obj)
        {
            return Name.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
