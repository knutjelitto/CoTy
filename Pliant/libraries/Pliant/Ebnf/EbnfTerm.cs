namespace Pliant.Ebnf
{
    public class EbnfTerm : EbnfNode
    {
        private readonly int _hashCode;
        public EbnfFactor Factor { get; }
        
        public EbnfTerm(EbnfFactor factor)
        {
            Factor = factor;
            this._hashCode = Factor.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfTerm>(obj, out var other) &&
                   Factor.Equals(other.Factor);
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return Factor.ToString();
        }
    }

    public class EbnfTermConcatenation : EbnfTerm
    {
        private readonly int _hashCode;
        public EbnfTerm Term { get; }

        public EbnfTermConcatenation(EbnfFactor factor, EbnfTerm term)
            : base(factor)
        {
            Term = term;
            this._hashCode = (Factor, Term).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfTermConcatenation>(obj, out var other) &&
                   (Factor, Term).Equals((other.Factor, other.Term));
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return $"{Factor} {Term}";
        }
    }
}