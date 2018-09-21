using System.Collections.Generic;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public class CoreOperator : Core
    {
        public CoreOperator() : base("operator") { }

        public override void Define(Maker into)
        {
            into.Define("eq?", (dynamic value1, dynamic value2) => Equals(value1, value2));
            into.Define("ne?", (dynamic value1, dynamic value2) => !Equals(value1, value2));

            into.Define("compare", (dynamic value1, dynamic value2) => (Integer)value1.CompareTo(value2));

            into.Define("lt?", (dynamic value1, dynamic value2) => value1.CompareTo(value2) < 0);
            into.Define("le?", (dynamic value1, dynamic value2) => value1.CompareTo(value2) <= 0);
            into.Define("gt?", (dynamic value1, dynamic value2) => value1.CompareTo(value2) > 0);
            into.Define("ge?", (dynamic value1, dynamic value2) => value1.CompareTo(value2) >= 0);

            into.Define("add", (dynamic value1, dynamic value2) => value1 + value2);
            into.Define("sub", (dynamic value1, dynamic value2) => value1 - value2);
            into.Define("mul", (dynamic value1, dynamic value2) => value1 * value2);
            into.Define("div", (dynamic value1, dynamic value2) => value1 / value2);
            into.Define("mod", (dynamic value1, dynamic value2) => value1 % value2);

            into.Define("succ", (dynamic value) => ++value);
            into.Define("pred", (dynamic value) => --value);

            into.Define("not", (dynamic value) => !value);
            into.Define("and", (dynamic value1, dynamic value2) => value1 & value2);
            into.Define("or", (dynamic value1, dynamic value2) => value1 | value2);
            into.Define("xor", (dynamic value1, dynamic value2) => value1 ^ value2);

            into.Define("neutral", () => Neutrum.Neutral);

            into.Define("bool?", value => value is bool);
            into.Define("true", () => true);
            into.Define("false", () => false);

            into.Define("integer", (dynamic value) => (Integer)value);
            into.Define("integer?", value => value is Integer);

            into.Define("sequence?", value => value is IEnumerable<object>);
        }
    }
}
