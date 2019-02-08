using System;
using System.Text;

namespace NuEarl.Structure
{
    public class Earley
    {
        public Earley(Rule rule, int dot, int parent)
        {
            Rule = rule;
            Parent = parent;
            Dot = dot;
        }

        public Rule Rule { get; }
        public int Parent { get; }
        public int Dot { get; }

        public Earley Advance()
        {
            if (Dot == Rule.Symbols.Count)
            {
                throw new Exception($"early item {this} already completed");
            }

            return new Earley(Rule, Dot+1, Parent);
        }

        public override bool Equals(object obj)
        {
            return obj is Earley other && ToString() == other.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("[");
            builder.Append(Rule.Name);
            builder.Append(" =");
            for (var i = 0; i < Rule.Symbols.Count; i++)
            {
                builder.Append(" ");
                if (i == Dot)
                {
                    builder.Append("• ");
                }

                builder.Append(Rule.Symbols[i].Name);
            }

            if (Dot == Rule.Symbols.Count)
            {
                builder.Append(" •");
            }

            builder.Append(",");
            builder.Append(Parent);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
