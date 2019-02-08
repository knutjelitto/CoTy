using Pliant.Diagnostics;

namespace Pliant.Ebnf
{
    public class EbnfDefinition : EbnfNode
    {
        private readonly int _hashCode;
        public EbnfBlock Block { get; }

        public EbnfDefinition(EbnfBlock block)
        {
            Assert.IsNotNull(block, nameof(block));

            Block = block;
            this._hashCode = Block.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            return IsOfType<EbnfDefinition>(obj, out var other) && Block.Equals(other.Block);
        }

        public override int GetHashCode() => this._hashCode;
    }

    public class EbnfDefinitionConcatenation : EbnfDefinition
    {
        private readonly int _hashCode;
        public EbnfDefinition Definition { get; }

        public EbnfDefinitionConcatenation(EbnfBlock block, EbnfDefinition definition)
            : base(block)
        {
            Definition = definition;
            this._hashCode = (Block, Definition).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfDefinitionConcatenation>(obj, out var other) &&
                   (Block, Definition).Equals((other.Block, other.Definition));
        }
        
        public override int GetHashCode() => this._hashCode;
    }
}