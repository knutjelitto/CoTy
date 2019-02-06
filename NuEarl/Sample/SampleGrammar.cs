using NuEarl.Structure;

namespace NuEarl.Sample
{
    public class SampleGrammar : Grammar
    {
        public SampleGrammar()
        {
            /*
            <expression> ::= <expression> + <term>
                           | <expression> - <term>
                           | <term>
            <term>       ::= <term> * <factor>
                           | <term> / <factor>
                           | <factor>
            <factor>     ::= ( <expression> ) 
                           | <number>
            */

            var expression = Nonterminal("expression");
            var term = Nonterminal("term");
            var factor = Nonterminal("factor");
            var number = Terminal("number");

            Start = expression;

            expression |= expression + "+" + term;
            expression |= expression + "-" + term;
            expression |= term;

            term |= term + "*" + factor;
            term |= term + "/" + factor;
            term |= factor;

            factor |= number;
            factor |= "-" + factor;
            factor |= "+" + factor;
            factor |= "(" + expression + ")";
        }
    }
}
