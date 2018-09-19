#if false
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoTy.Objects
{
    public class Characters : Cobject<string>, IComparable<Characters>, IEnumerable<char>
    {
        private static readonly Characters Empty = new Characters(string.Empty);

        private Characters(string value) : base(value)
        {
        }

        public static Characters From(string chars)
        {
            if (string.IsNullOrEmpty(chars))
            {
                return Empty;
            }

            return new Characters(chars);
        }

        public static Characters From(StringBuilder chars)
        {
            return From(chars.ToString());
        }

        public int CompareTo(Characters other)
        {
            return string.Compare(Value, other.Value, StringComparison.Ordinal);
        }

        public static implicit operator Characters(string characters)
        {
            return new Characters(characters);
        }

        public static implicit operator string(Characters characters)
        {
            return characters.Value;
        }

        public static Characters operator +(Characters chars1, Characters chars2)
        {
            return From(chars1.Value + chars2.Value);
        }

        public static Characters operator *(Characters chars, Integer count)
        {
            return Multiply(chars, count);
        }

        public static Characters operator *(Integer count, Characters chars)
        {
            return Multiply(chars, count);
        }

        public IEnumerator<char> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            return obj is Characters other && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return "\"" + Value.Replace("\"", "\\\"") + "\"";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static Characters Multiply(Characters str, Integer count)
        {
            var bigLength = str.Value.Length * count;

            if (bigLength <= 0 || Equals(str, Empty))
            {
                return Empty;
            }

            if (bigLength > 0x1000000) // arbitrary max length
            {
                return From("... to hefty a string ...");
            }

            var length = (int)bigLength;

            var builder = new StringBuilder(length);

            for (var i = (int)count; i > 0; --i)
            {
                builder.Append(str.Value);
            }

            return From(builder);
        }
    }
}
#endif