using System.Collections.Generic;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public class OperatorDefiner : Definer
    {
        public OperatorDefiner() : base("operator") { }

        public override void Define(IScope into)
        {
            Define(into, "eq?", (dynamic value1, dynamic value2) => Equals(value1, value2));
            Define(into, "ne?", (dynamic value1, dynamic value2) => !Equals(value1, value2));

            Define(into, "<", (dynamic value1, dynamic value2) => value1.CompareTo(value2) < 0);
            Define(into, "<=", (dynamic value1, dynamic value2) => value1.CompareTo(value2) <= 0);
            Define(into, ">", (dynamic value1, dynamic value2) => value1.CompareTo(value2) > 0);
            Define(into, ">=", (dynamic value1, dynamic value2) => value1.CompareTo(value2) >= 0);

            Define(into, "add", (dynamic value1, dynamic value2) => value1 + value2);
            Define(into, "sub", (dynamic value1, dynamic value2) => value1 - value2);
            Define(into, "mul", (dynamic value1, dynamic value2) => value1 * value2);
            Define(into, "div", (dynamic value1, dynamic value2) => value1 / value2);
            Define(into, "mod", (dynamic value1, dynamic value2) => value1 % value2);

            Define(into, "succ", (dynamic value) => ++value);
            Define(into, "pred", (dynamic value) => --value);

            Define(into, "not", (dynamic value) => !value);
            Define(into, "and", (dynamic value1, dynamic value2) => value1 & value2);
            Define(into, "or", (dynamic value1, dynamic value2) => value1 | value2);
            Define(into, "xor", (dynamic value1, dynamic value2) => value1 ^ value2);

            Define(into, "neutral", () => Neutrum.Neutral);

            Define(into, "bool?", value => value is bool);
            Define(into, "true", () => true);
            Define(into, "false", () => false);

            Define(into, "integer", (dynamic value) => (Integer)value);
            Define(into, "integer?", value => value is Integer);

            Define(into, "sequence?", value => value is Sequence);
        }
    }
}
