using Pliant.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pliant.Ebnf
{
    public class EbnfLexerRuleExpression : EbnfNode
    {
        private readonly int _hashCode;
        public EbnfLexerRuleTerm Term { get; }

        public EbnfLexerRuleExpression(EbnfLexerRuleTerm term)
        {
            Term = term;
            this._hashCode = Term.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            return IsOfType<EbnfLexerRuleExpression>(obj, out var other) && 
                   Term.Equals(other.Term);
        }

        public override int GetHashCode() => this._hashCode;
    }

    public sealed class EbnfLexerRuleExpressionAlteration : EbnfLexerRuleExpression
    {
        private readonly int _hashCode;
        public EbnfLexerRuleExpression Expression { get; }

        public EbnfLexerRuleExpressionAlteration(EbnfLexerRuleTerm term, EbnfLexerRuleExpression expression)
            : base(term)
        {
            Expression = expression;
            this._hashCode = (Term, Expression).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfLexerRuleExpressionAlteration>(obj, out var other) &&
                   (Term, Expression).Equals((other.Term, other.Expression));
        }

        public override int GetHashCode() => this._hashCode;
    }
}
