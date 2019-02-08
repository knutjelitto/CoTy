namespace Pliant.Ebnf
{
    public class EbnfLexerRuleTerm : EbnfNode
    {
        private readonly int _hashCode;
        public EbnfLexerRuleFactor Factor { get; }

        public EbnfLexerRuleTerm(EbnfLexerRuleFactor factor)
        {
            Factor = factor;
            this._hashCode = Factor.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfLexerRuleTerm>(obj, out var other) &&
                   Factor.Equals(other.Factor);
        }

        public override int GetHashCode() => this._hashCode;
    }

    public sealed class EbnfLexerRuleTermConcatenation : EbnfLexerRuleTerm
    {
        private readonly int _hashCode;
        public EbnfLexerRuleTerm Term { get; }

        public EbnfLexerRuleTermConcatenation(EbnfLexerRuleFactor factor, EbnfLexerRuleTerm term)
            : base(factor)
        {
            Term = term;
            this._hashCode = (Factor, Term).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfLexerRuleTermConcatenation>(obj, out var other) &&
                   (Factor, Term).Equals((other.Factor, other.Term));
        }

        public override int GetHashCode() => this._hashCode;
    }
}