using System;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
namespace CoTy.Objects.Impl
{
    public partial class CobjectImpl
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
