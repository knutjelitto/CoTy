using System;
using System.Collections;
using System.Collections.Generic;

namespace CoTy.Inputs
{
    public class StringStream : ItemStream<char>
    {
        private readonly string characters;

        public StringStream(string characters)
        {
            this.characters = characters;
        }

        public override IEnumerator<char> GetEnumerator() => this.characters.GetEnumerator();
    }
}
