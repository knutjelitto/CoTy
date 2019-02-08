using System.Collections;
using NuEarl.Structure;

namespace NuEarl.Sample
{
    public class SampleGrammar : Grammar
    {
        public SampleGrammar()
        {
            /*
            expression  := expression "+" term
                         | expression "-" term
                         | term
            term        := term "*" factor
                         | term "/" factor
                         | factor
            factor      := "+"
                         | "-"
                         | "(" expression ")"
                         | number
            number      := digit+
            digit       := "0" .. "9"
            */

            var expression = Nonterminal("E");
            var term = Nonterminal("T");
            var factor = Nonterminal("F");
            var number = Nonterminal("number");

            var addOp = Terminal("+");
            var subOp = Terminal("-");
            var mulOp = Terminal("*");
            var divOp = Terminal("/");
            var lParent = Terminal("(");
            var rParent = Terminal(")");

            var x = 00000;

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
