using Pliant.Tokens;

namespace Pliant.Grammars
{
    public abstract class BaseLexerRule : ILexerRule
    {
        protected BaseLexerRule(LexerRuleType lexerRuleType, TokenType tokenType)
        {
            LexerRuleType = lexerRuleType;
            TokenType = tokenType;
        }

        public LexerRuleType LexerRuleType { get; }
        public TokenType TokenType { get; }
        public SymbolType SymbolType => SymbolType.LexerRule;

        public abstract bool CanApply(char c);
    }
}