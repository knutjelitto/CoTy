using System.Collections.Generic;

namespace CoTy.Inputs
{
    public class CharSource : ItemSource<char>
    {
        public static readonly char EOT = '\u0003';
        public static readonly char NL = '\n';

        public CharSource(IEnumerable<char> characterSource)
            : base(characterSource)
        {
        }

        protected override char EndOfItems => EOT;
    }
}
