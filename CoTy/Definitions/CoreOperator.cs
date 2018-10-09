using System.Collections.Generic;
using CoTy.Objects;

namespace CoTy.Definitions
{
    public class CoreOperator : Core
    {
        public CoreOperator() : base("operator") { }

        public override void Define(Maker into)
        {
            into.Define("==", (Cobject value1, Cobject value2) => Bool.From(Equals(value1, value2)));
            into.Define("!=", (Cobject value1, Cobject value2) => Bool.From(!Equals(value1, value2)));

            into.Define("compare", (Cobject value1, Cobject value2) => (Integer)((dynamic)value1).CompareTo((dynamic)value2));

            into.Define("<", (Cobject value1, Cobject value2) => Bool.From(((dynamic)value1).CompareTo((dynamic)value2) < 0));
            into.Define("<=", (Cobject value1, Cobject value2) => Bool.From(((dynamic)value1).CompareTo((dynamic)value2) <= 0));
            into.Define(">", (Cobject value1, Cobject value2) => Bool.From(((dynamic)value1).CompareTo((dynamic)value2) > 0));
            into.Define(">=", (Cobject value1, Cobject value2) => Bool.From(((dynamic)value1).CompareTo((dynamic)value2) >= 0));

            into.Define("+", (Cobject value1, Cobject value2) => (dynamic)value1 + (dynamic)value2);
            into.Define("-", (Cobject value1, Cobject value2) => (dynamic)value1 - (dynamic)value2);
            into.Define("*", (Cobject value1, Cobject value2) => (dynamic)value1 * (dynamic)value2);
            into.Define("/", (Cobject value1, Cobject value2) => (dynamic)value1 / (dynamic)value2);
            into.Define("%", (Cobject value1, Cobject value2) => (dynamic)value1 % (dynamic)value2);

            into.Define("not", (Cobject value) => !(dynamic)value);
            into.Define("and", (Cobject value1, Cobject value2) => (dynamic)value1 & (dynamic)value2);
            into.Define("or", (Cobject value1, Cobject value2) => (dynamic)value1 | (dynamic)value2);
            into.Define("xor", (Cobject value1, Cobject value2) => (dynamic)value1 ^ (dynamic)value2);

            into.Define("neutral", () => Neutrum.Neutral);

            into.Define("bool?", (Cobject value) => Bool.From(value is Bool));
            into.Define("true", () => Bool.True);
            into.Define("false", () => Bool.False);

            into.Define("integer", (Cobject value) => (Integer)(dynamic)value);
            into.Define("integer?", (Cobject value) => Bool.From(value is Integer));

            into.Define("sequence?", (Cobject value) => Bool.From(value is IEnumerable<Cobject>));
        }
    }
}
