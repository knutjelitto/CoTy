using System;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
namespace CoTy.Objects
{
    public partial class Cobject
    {
        public void Print(object value)
        {
            Console.Write($"{value}");
        }

        public void Println(object value)
        {
            Print(value);
            Console.WriteLine();
        }
    }
}
