using System;
using System.Linq;
using CoTy.Errors;
using CoTy.Objects;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace CoTy.Modules
{
    public class OperatorModule : Module
    {
        public OperatorModule() : base("operator") { }

        public override Context Reflect(Context into)
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

            return into;
        }
    }
}
