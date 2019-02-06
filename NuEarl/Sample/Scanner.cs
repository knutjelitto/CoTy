using System;
using System.Collections.Generic;
using System.Text;

namespace NuEarl.Sample
{
    public class Scanner
    {
        private readonly string text;
        private int index;

        public Scanner(string text)
        {
            this.text = text;
            this.index = 0;
        }

        private bool Have => this.index < this.text.Length;
        private char Current => this.text[this.index];

        public bool Next(out char token)
        {
            Skip();
            if (Have)
            {
                token = Current;
                Advance();
                return true;
            }

            token = '\0';
            return false;
        }

        private void Advance()
        {
            this.index++;
        }

        private void Skip()
        {
            while (Have && char.IsWhiteSpace(Current))
            {
                Advance();
            }
        }
    }
}
