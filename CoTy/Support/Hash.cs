using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoTy.Support
{
    public class Hash
    {
        public static int Up(params object[] args)
        {
            return Up(args.AsEnumerable());
        }

        public static int Up(IEnumerable<object> args)
        {
            var hash = 0;

            foreach (var arg in args)
            {
                hash = ((hash << 5) + hash) ^ arg.GetHashCode();
            }

            return hash;
        }
    }
}
