using System;
using System.Collections.Generic;
using System.Linq;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public class OperatorDefiner : Definer
    {
        public OperatorDefiner() : base("operator") { }

        public override Context Define(Context into)
        {
            Define(into, "==", (dynamic value1, dynamic value2) => Equals(value1, value2));
            Define(into, "!=", (dynamic value1, dynamic value2) => !Equals(value1, value2));

            Define(into, "<", (dynamic value1, dynamic value2) => value1.CompareTo(value2) < 0);
            Define(into, "<=", (dynamic value1, dynamic value2) => value1.CompareTo(value2) <= 0);
            Define(into, ">", (dynamic value1, dynamic value2) => value1.CompareTo(value2) > 0);
            Define(into, ">=", (dynamic value1, dynamic value2) => value1.CompareTo(value2) >= 0);

            Define(into, "+", (dynamic value1, dynamic value2) => value1 + value2);
            Define(into, "-", (dynamic value1, dynamic value2) => value1 - value2);
            Define(into, "*", (dynamic value1, dynamic value2) => value1 * value2);
            Define(into, "/", (dynamic value1, dynamic value2) => value1 / value2);
            Define(into, "%", (dynamic value1, dynamic value2) => value1 % value2);

            Define(into, "++", (dynamic value) => ++value);
            Define(into, "--", (dynamic value) => --value);

            Define(into, "!", (dynamic value) => !value);
            Define(into, "&", (dynamic value1, dynamic value2) => value1 & value2);
            Define(into, "|", (dynamic value1, dynamic value2) => value1 | value2);
            Define(into, "^", (dynamic value1, dynamic value2) => value1 ^ value2);

            Define(into, "bool?", (value) => value is bool);
            Define(into, "true", true);
            Define(into, "false", false);

            Define(into, "<>", (value1, value2) => Sequence.From(Enumerate(value1).Concat(Enumerate(value2))));
            Define(into, "><", (context, stack, values) =>
            {
                foreach (var value in Enumerate(values))
                {
                    stack.Push(value);
                }
            });

            return base.Define(into);
        }
    }
}
