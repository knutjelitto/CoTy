namespace Pliant.Ebnf
{
    public class EbnfExpression : EbnfNode
    {
        private readonly int _hashCode;
        public EbnfTerm Term { get; }

        public EbnfExpression(EbnfTerm term)
        {
            Term = term;
            this._hashCode = Term.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfExpression>(obj, out var other) && Term.Equals(other.Term);
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return Term.ToString();
        }
    }

    public sealed class EbnfExpressionAlteration : EbnfExpression
    {
        private readonly int _hashCode;
        public EbnfExpression Expression { get; }

        public EbnfExpressionAlteration(
            EbnfTerm term,
            EbnfExpression expression)
            : base(term)
        {
            Expression = expression;
            this._hashCode = (Term, Expression).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfExpressionAlteration>(obj, out var other) &&
                   (Term, Expression).Equals((other.Term, other.Expression));
        }

        public override int GetHashCode()
        {
            return this._hashCode;
        }

        public override string ToString()
        {
            return $"{Term} | {Expression}";
        }
    }
}