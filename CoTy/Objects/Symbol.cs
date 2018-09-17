﻿using System.Collections.Generic;

namespace CoTy.Objects
{
    public class Symbol : Cobject<string>
    {
        private static readonly Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public static readonly Symbol True = Get("true");
        public static readonly Symbol False = Get("false");
        public static readonly Symbol Quoter = Get("'");
        public static readonly Symbol BindTo = Get(":");
        public static readonly Symbol LeftParent = Get("(");
        public static readonly Symbol RightParent = Get(")");
        public static readonly Symbol Define = Get("def");
        public static readonly Symbol Set = Get("set");
        public static readonly Symbol ApplySym = Get("apply");

        private readonly int hashCode;

        private Symbol(string value) : base(value)
        {
            this.hashCode = value.GetHashCode();
        }

        public static Symbol Get(string name)
        {
            if (!symbols.TryGetValue(name, out var symbol))
            {
                symbol = new Symbol(name);
                symbols.Add(name, symbol);
            }

            return symbol;
        }

        public override void Close(Context context, Stack stack)
        {
            context.Get(this, out var value);

            value.Apply(context, stack);
        }

        public override int GetHashCode() => this.hashCode;

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Symbol other && Value == other.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
