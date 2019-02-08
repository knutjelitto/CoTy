namespace Pliant.Ebnf
{
    public abstract class EbnfBlock : EbnfNode
    {
    }

    public sealed class EbnfBlockRule : EbnfBlock
    {
        private readonly int _hashCode;

        public EbnfRule Rule { get; }

        public EbnfBlockRule(EbnfRule rule)
        {
            Rule = rule;
            this._hashCode = Rule.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfBlockRule>(obj, out var other) &&
                   Rule.Equals(other.Rule);
        }
                
        public override int GetHashCode() => this._hashCode;
    }

    public sealed class EbnfBlockSetting : EbnfBlock
    {
        private readonly int _hashCode;
        public EbnfSetting Setting { get; }

        public EbnfBlockSetting(EbnfSetting setting)
        {
            Setting = setting;
            this._hashCode = Setting.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfBlockSetting>(obj, out var other) && 
                   Setting.Equals(other.Setting);
        }

        public override int GetHashCode() => this._hashCode;
    }

    public sealed class EbnfBlockLexerRule : EbnfBlock
    {
        private readonly int _hashCode;
        public EbnfLexerRule LexerRule { get; }

        public EbnfBlockLexerRule(EbnfLexerRule lexerRule)
        {
            LexerRule = lexerRule;
            this._hashCode = LexerRule.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return IsOfType<EbnfBlockLexerRule>(obj, out var other) &&
                   LexerRule.Equals(other.LexerRule);
        }

        public override int GetHashCode() => this._hashCode;
    }
}
