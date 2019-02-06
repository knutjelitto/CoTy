using System;
using System.Linq;
using NuEarl.Sample;

namespace NuEarl
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            var p = new Program();

            p.Sample();

            p.AnyKey();
        }

        public void Sample()
        {
            var sample = new SampleGrammar();

            foreach (var nonterminal in sample.Nonterminals)
            {
                Console.WriteLine($"{nonterminal} = {string.Join(" | ", nonterminal.Rules.Select(r => r.Symbols))}");
            }
        }

        public void AnyKey()
        {
            Console.Write("any key ...");
            Console.ReadKey(true);
        }
    }
}
