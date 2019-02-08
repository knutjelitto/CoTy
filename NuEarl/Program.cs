using System;
using System.Linq;
using NuEarl.Regular;
using NuEarl.Sample;

namespace NuEarl
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            var p = new Program();

            //p.Sample();
            p.Nfa2Dfa();

            p.AnyKey();
        }

        // ReSharper disable once UnusedMember.Local
        private void Sample()
        {
            var sample = new SampleGrammar();

            foreach (var nonterminal in sample.Nonterminals)
            {
                Console.WriteLine($"{nonterminal} = {string.Join(" | ", nonterminal.Rules.Select(r => r.Symbols))}");
            }
        }

        private void Nfa2Dfa()
        {
            var t = new TestNFA();

            t.Test();
        }

        private void AnyKey()
        {
            Console.Write("any key ...");
            Console.ReadKey(true);
        }
    }
}
