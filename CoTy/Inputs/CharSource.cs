using System.Collections.Generic;

namespace CoTy.Inputs
{
    public class CharSource : ItemSource<char>
    {
        private const char EOT = '\u0003';
        public const char NL = '\n';

        public CharSource(IEnumerable<char> characterSource)
            : base(characterSource)
        {
        }

        protected override char EndOfItems => EOT;
    }
}
