﻿using System;
using System.Collections.Generic;

namespace CoTy.Inputs
{
    public class ConsoleStream : ItemStream<char>
    {
        private readonly Action beforePrompt;
        private int nesting = 0;

        public ConsoleStream(Action beforePrompt = null)
        {
            this.beforePrompt = beforePrompt ?? (() => { });
        }

        protected override void OpenLevel()
        {
            this.nesting = this.nesting + 1;
        }

        protected override void CloseLevel()
        {
            this.nesting = Math.Max(0, this.nesting - 1);
        }

        public override IEnumerator<char> GetEnumerator()
        {
            string line;
            while ((line = GetLine()) != null)
            {
                foreach (var character in line)
                {
                    yield return character;
                }
                yield return '\n';
            }
        }

        private string GetLine()
        {
            if (!Console.IsInputRedirected)
            {
                this.beforePrompt();
                Console.Write($"{new string('-', this.nesting + 1)}>");
            }
            return Console.ReadLine();
        }
    }
}
