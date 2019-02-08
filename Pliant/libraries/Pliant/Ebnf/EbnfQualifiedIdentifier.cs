namespace Pliant.Ebnf
{
    public class EbnfQualifiedIdentifier : EbnfNode
    {
        private readonly int _hashCode;
        public string Identifier { get; }

        public EbnfQualifiedIdentifier(string identifier)
        {
            Identifier = identifier;
            this._hashCode = Identifier.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfQualifiedIdentifier>(obj, out var other) &&
                   Identifier.Equals(other.Identifier);
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return Identifier;
        }
    }

    public class EbnfQualifiedIdentifierConcatenation : EbnfQualifiedIdentifier
    {
        private readonly int _hashCode;
        public EbnfQualifiedIdentifier QualifiedIdentifier { get; }

        public EbnfQualifiedIdentifierConcatenation(
            string identifier,
            EbnfQualifiedIdentifier qualifiedIdentifier)
            : base(identifier)
        {
            QualifiedIdentifier = qualifiedIdentifier;
            this._hashCode = (Identifier, QualifiedIdentifier).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfQualifiedIdentifierConcatenation>(obj, out var other) &&
                   (Identifier, QualifiedIdentifier).Equals((other.Identifier, other.QualifiedIdentifier));
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return $"{QualifiedIdentifier}.{Identifier}";
        }
    }
}