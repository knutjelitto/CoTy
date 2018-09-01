using System;
using System.Collections;
using System.Collections.Generic;

namespace CoTy.Inputs
{
    public class ConsoleSource : IEnumerable<char>
    {
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
