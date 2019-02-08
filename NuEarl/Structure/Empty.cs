using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NuEarl.Structure
{
    public class Empty : Symbol
    {
        public static readonly Empty Instance = new Empty();

        public Empty() : base(null, "epsilon")
        {
        }
    }
}
