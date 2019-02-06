using System;

namespace CoPeg
{
    public class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            var p = new Program();

            p.AnyKey();
        }

        private void AnyKey()
        {
            Console.WriteLine("any key ...");
            Console.ReadKey(true);
        }
    }
}
