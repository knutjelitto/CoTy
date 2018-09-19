using System.Linq;

using CoTy.Objects;

namespace CoTy.Definitions
{
    public class OperatorDefiner : Definer
    {
        public OperatorDefiner() : base("operator") { }

        public override void Define(IScope into)
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

            Define(into, "succ", (dynamic value) => ++value);
            Define(into, "pred", (dynamic value) => --value);

            Define(into, "not", (dynamic value) => !value);
            Define(into, "and", (dynamic value1, dynamic value2) => value1 & value2);
            Define(into, "or", (dynamic value1, dynamic value2) => value1 | value2);
            Define(into, "xor", (dynamic value1, dynamic value2) => value1 ^ value2);

            Define(into, "bool?", value => value is bool);
            Define(into, "true", () => true);
            Define(into, "false", () => false);

            Define(into, "<>", (value1, value2) => Sequence.From(Enumerate(value1).Concat(Enumerate(value2))));
            Define(into, "><", (scope, stack, values) =>
            {
                foreach (var value in Enumerate(values))
                {
                    stack.Push(value);
                }
            });
        }
    }
}
