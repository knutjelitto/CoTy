using System;
using System.Collections;
using System.Collections.Generic;

namespace CoTy.Inputs
{
    public class ConsoleInput : IEnumerable<char>
    {
        private readonly Action beforePrompt;

        public ConsoleInput(Action beforePrompt = null)
        {
            this.beforePrompt = beforePrompt ?? (() => { });
        }

        public IEnumerator<char> GetEnumerator()
        {
            string line;
            while ((line = GetLine()) != null)
            {
                foreach (var character in line)
                {
                    yield return character;
                }
                yield return CharSource.NL;
            }
        }

        private string GetLine()
        {
            if (!Console.IsInputRedirected)
            {
                this.beforePrompt();
                Console.Write("->");
            }
            return Console.ReadLine();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
