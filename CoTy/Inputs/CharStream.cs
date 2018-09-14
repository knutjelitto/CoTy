using System;
using System.Collections;
using System.Collections.Generic;

namespace CoTy.Inputs
{
    public class CharStream : ItemStream<char>
    {
        private readonly string characters;

        public CharStream(string characters)
        {
            this.characters = characters;
        }

        public override IEnumerator<char> GetEnumerator() => this.characters.GetEnumerator();
    }
}
