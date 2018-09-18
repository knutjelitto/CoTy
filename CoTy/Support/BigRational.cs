using System;
using System.Numerics;

namespace CoTy.Support
{
    public sealed class BigRational : IComparable, IComparable<BigRational>, IEquatable<BigRational>
    {
        public static readonly BigRational Zero = new BigRational(0, 1);
        public static readonly BigRational One = new BigRational(1, 1);
        public static readonly BigRational MinusOne = new BigRational(-1, 1);

        private readonly BigInteger Numerator;
        private readonly BigInteger Denominator;

        public BigRational(BigInteger numerator, BigInteger denominator)
        {
            Canonicalize(ref numerator, ref denominator);
            this.Numerator = numerator;
            this.Denominator = denominator;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is BigRational other))
            {
                throw new ArgumentException();
            }

            return CompareTo(other);
        }

        public int CompareTo(BigRational other)
        {
            return (this.Numerator * other.Denominator).CompareTo(this.Denominator * other.Numerator);
        }

        public bool Equals(BigRational other)
        {
            return this.Numerator == other.Numerator && this.Denominator == other.Denominator;
        }

        public override bool Equals(object obj)
        {
            return obj is BigRational other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Hash.Up(this.Numerator, this.Denominator);
        }

        public static bool operator ==(BigRational one, BigRational two)
        {
            return Equals(one, two);
        }

        public static bool operator !=(BigRational one, BigRational two)
        {
            return !Equals(one, two);
        }

        public static bool operator <(BigRational one, BigRational two)
        {
            return one.CompareTo(two) < 0;
        }

        public static bool operator >(BigRational one, BigRational two)
        {
            return one.CompareTo(two) > 0;
        }

        public static bool operator <=(BigRational one, BigRational two)
        {
            return one.CompareTo(two) <= 0;
        }

        public static bool operator >=(BigRational one, BigRational two)
        {
            return one.CompareTo(two) >= 0;
        }

        public static BigRational operator +(BigRational one, BigRational two)
        {
            return new BigRational(one.Numerator * two.Denominator + one.Denominator * two.Numerator, one.Denominator * two.Denominator);
        }

        public static BigRational operator -(BigRational one, BigRational two)
        {
            return new BigRational(one.Numerator * two.Denominator - one.Denominator * two.Numerator, one.Denominator * two.Denominator);
        }

        public static BigRational operator *(BigRational one, BigRational two)
        {
            return new BigRational(one.Numerator * two.Numerator, one.Denominator * two.Denominator);
        }

        public static BigRational operator /(BigRational one, BigRational two)
        {
            return new BigRational(one.Numerator * two.Denominator, one.Denominator * two.Numerator);
        }

        private void Canonicalize(ref BigInteger numerator, ref BigInteger denominator)
        {
            var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);

            if (denominator.CompareTo(BigInteger.Zero) < 0)
            {
                if (gcd != 1)
                {
                    numerator = -numerator / gcd;
                    denominator = -denominator / gcd;
                }
                else
                {
                    numerator = -numerator;
                    denominator = -denominator;
                }
            }
            else if (gcd != 1)
            {
                numerator = numerator / gcd;
                denominator = denominator / gcd;
            }
        }

        public override string ToString()
        {
            return this.Numerator + "/" + this.Denominator;
        }
    }
}
