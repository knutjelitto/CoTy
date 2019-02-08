using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pliant.Diagnostics;
using Pliant.Utilities;

namespace Pliant.Grammars
{
    public sealed class Production : IProduction
    {
        public INonTerminal LeftHandSide { get; }

        public IReadOnlyList<ISymbol> RightHandSide { get; }

        public bool IsEmpty => RightHandSide.Count == 0;

        public Production(INonTerminal leftHandSide, List<ISymbol> rightHandSide)
        {
            Assert.IsNotNull(leftHandSide, nameof(leftHandSide));
            Assert.IsNotNull(rightHandSide, nameof(rightHandSide));
            LeftHandSide = leftHandSide;
            RightHandSide = new List<ISymbol>(new List<ISymbol>(rightHandSide));
            this._hashCode = ComputeHashCode();
        }

        public Production(INonTerminal leftHandSide, params ISymbol[] rightHandSide)
        {
            Assert.IsNotNull(leftHandSide, nameof(leftHandSide));
            Assert.IsNotNull(rightHandSide, nameof(rightHandSide));
            LeftHandSide = leftHandSide;
            RightHandSide = new List<ISymbol>(new List<ISymbol>(rightHandSide));
            this._hashCode = ComputeHashCode();
        }
                
        public override bool Equals(object obj)
        {
            if (obj is Production that)
            {
                if (!LeftHandSide.Equals(that.LeftHandSide))
                {
                    return false;
                }

                var rightHandSideCount = RightHandSide.Count;
                if (rightHandSideCount != that.RightHandSide.Count)
                {
                    return false;
                }

                for (var i = 0; i < rightHandSideCount; i++)
                {
                    if (!RightHandSide[i].Equals(that.RightHandSide[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        // PERF: Cache Costly Hash Code Computation
        private readonly int _hashCode;

        public override int GetHashCode()
        {
            return this._hashCode;
        }

        private int ComputeHashCode()
        {
            var hash = HashCode.ComputeIncrementalHash(LeftHandSide.GetHashCode(), HashCode.InitIncrementalHash());

            return RightHandSide.Aggregate(hash, (accumulator, symbol) => HashCode.ComputeIncrementalHash(symbol.GetHashCode(), accumulator));
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0} ->", LeftHandSide.Value);

            for (int p = 0; p < RightHandSide.Count; p++)
            {
                var symbol = RightHandSide[p];
                stringBuilder.AppendFormat(" {0}", symbol);
            }
            return stringBuilder.ToString();
        }
    }
}