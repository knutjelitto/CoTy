// ReSharper disable UnusedMember.Global
using System;

namespace CoTy.Objects
{
    public partial class Cobject
    {
        public void Print()
        {
            Console.Write($"{this}");
        }

        public void Println()
        {
            Print();
            Console.WriteLine();
        }
    }
}
