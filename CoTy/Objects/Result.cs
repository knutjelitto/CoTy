using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoTy.Objects
{
    public sealed class Result : Cobject<List<object>>
    {
        private Result(List<object> value) : base(value)
        {
        }

        public static Result From(IEnumerable<object> values)
        {
            return new Result(values.ToList());
        }

        public static Result From(params object[] values)
        {
            return From(values.AsEnumerable());
        }

    }
}
