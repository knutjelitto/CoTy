namespace Pliant.Ebnf
{
    public class EbnfSettingIdentifier : EbnfNode
    {
        private readonly int _hashCode;
        public string Value { get; }

        public EbnfSettingIdentifier(string value)
        {
            Value = value;
            this._hashCode = Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfSettingIdentifier>(obj, out var other) &&
                   Value.Equals(other.Value);
        }

        public override int GetHashCode() => this._hashCode;
    }
}