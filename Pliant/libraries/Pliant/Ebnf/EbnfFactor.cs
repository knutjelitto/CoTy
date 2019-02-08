namespace Pliant.Ebnf
{
    public abstract class EbnfFactor : EbnfNode
    {
    }

    public sealed class EbnfFactorIdentifier : EbnfFactor
    {
        private readonly int _hashCode;
        public EbnfQualifiedIdentifier QualifiedIdentifier { get; }

        public EbnfFactorIdentifier(EbnfQualifiedIdentifier qualifiedIdentifier)
        {
            QualifiedIdentifier = qualifiedIdentifier;
            this._hashCode = QualifiedIdentifier.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfFactorIdentifier>(obj, out var other) &&
                   QualifiedIdentifier.Equals(other.QualifiedIdentifier);
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return QualifiedIdentifier.ToString();
        }
    }

    public sealed class EbnfFactorLiteral : EbnfFactor
    {
        private readonly int _hashCode;
        public string Value { get; }

        public EbnfFactorLiteral(string value)
        {
            Value = value;
            this._hashCode = Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfFactorLiteral>(obj, out var other) && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return this._hashCode;
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public sealed class EbnfFactorRegex : EbnfFactor
    {
        private readonly int _hashCode;
        public RegularExpressions.Regex Regex { get; }

        public EbnfFactorRegex(RegularExpressions.Regex regex)
        {
            Regex = regex;
            this._hashCode = Regex.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfFactorRegex>(obj, out var other) && Regex.Equals(other.Regex);
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return $"/{Regex}/";
        }
    }

    public sealed class EbnfFactorRepetition : EbnfFactor
    {
        private readonly int _hashCode;
        public EbnfExpression Expression { get; }

        public EbnfFactorRepetition(EbnfExpression expression)
        {
            Expression = expression;
            this._hashCode = Expression.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfFactorRepetition>(obj, out var other) && Expression.Equals(other.Expression);
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return $"{{{Expression}}}";
        }
    }

    public sealed class EbnfFactorOptional : EbnfFactor
    {
        private readonly int _hashCode;
        public EbnfExpression Expression { get; }

        public EbnfFactorOptional(EbnfExpression expression)
        {
            Expression = expression;
            this._hashCode = Expression.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfFactorOptional>(obj, out var other) && Expression.Equals(other.Expression);
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return $"[{Expression}]";
        }
    }

    public sealed class EbnfFactorGrouping : EbnfFactor
    {
        private readonly int _hashCode;
        public EbnfExpression Expression { get; }

        public EbnfFactorGrouping(EbnfExpression expression)
        {
            Expression = expression;
            this._hashCode = Expression.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfFactorGrouping>(obj, out var other) && Expression.Equals(other.Expression);
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return $"({Expression})";
        }
    }
}