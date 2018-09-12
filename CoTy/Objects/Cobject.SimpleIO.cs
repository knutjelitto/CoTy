// ReSharper disable UnusedMember.Global
using System;

namespace CoTy.Objects
{
    public partial class Cobject
    {
        public void Print(Cobject value)
        {
            Console.Write($"{value}");
        }

        public void Println(Cobject value)
        {
            Print(value);
            Console.WriteLine();
        }
    }
}
