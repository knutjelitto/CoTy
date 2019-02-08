using Pliant.Automata;
using Pliant.Builders;
using Pliant.Grammars;
using Pliant.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System;
using Pliant.Tokens;

namespace Pliant.Ebnf
{
    public class EbnfGrammarGenerator
    {
        private readonly INfaToDfa _nfaToDfaAlgorithm;
        private readonly IRegexToNfa _regexToNfaAlgorithm;

        public EbnfGrammarGenerator()
        {
            this._regexToNfaAlgorithm = new ThompsonConstructionAlgorithm();
            this._nfaToDfaAlgorithm = new SubsetConstructionAlgorithm();
        }

        public IGrammar Generate(EbnfDefinition ebnf)
        {
            var grammarModel = new GrammarModel();
            Definition(grammarModel, ebnf);
            return grammarModel.ToGrammar();
        }

        private void Definition(GrammarModel grammarModel, EbnfDefinition definition)
        {
            Block(definition.Block, grammarModel);

            if (definition is EbnfDefinitionConcatenation definitionConcatenation)
            {
                Definition(grammarModel, definitionConcatenation.Definition);
            }
        }

        private void Block(EbnfBlock block, GrammarModel grammarModel)
        {
            if (block is EbnfBlockLexerRule lexerRule)
            {
                grammarModel.LexerRules.Add(LexerRule(lexerRule));
            }
            else if (block is EbnfBlockRule rule)
            {
                foreach (var production in Rule(rule.Rule))
                {
                    grammarModel.Productions.Add(production);
                }
            }
            else if (block is EbnfBlockSetting setting)
            {
                switch (setting.Setting.SettingIdentifier.Value)
                {
                    case StartProductionSettingModel.SettingKey:
                        grammarModel.StartSetting = StartSetting(setting);
                        break;

                    case IgnoreSettingModel.SettingKey:
                        var ignoreSettings = IgnoreSettings(setting);
                        foreach (var ignore in ignoreSettings)
                        {
                            grammarModel.IgnoreSettings.Add(ignore);
                        }

                        break;

                    case TriviaSettingModel.SettingKey:
                        var triviaSettings = TriviaSettings(setting);
                        foreach (var trivia in triviaSettings)
                        {
                            grammarModel.TriviaSettings.Add(trivia);
                        }

                        break;
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"Invalid EbnfBlock node type detected. Found {block.GetType().Name}, expected EbnfBlockLexerRule or EbnfBlockRule or EbnfBlockSetting");
            }
        }

        private LexerRuleModel LexerRule(EbnfBlockLexerRule blockLexerRule)
        {
            var ebnfLexerRule = blockLexerRule.LexerRule;

            var fullyQualifiedName = GetFullyQualifiedNameFromQualifiedIdentifier(
                ebnfLexerRule.QualifiedIdentifier);

            var lexerRule = LexerRuleExpression(
                fullyQualifiedName,
                ebnfLexerRule.Expression);

            return new LexerRuleModel(lexerRule);
        }

        private ILexerRule LexerRuleExpression(
            FullyQualifiedName fullyQualifiedName, 
            EbnfLexerRuleExpression ebnfLexerRule)
        {
            if (TryRecognizeSimpleLiteralExpression(fullyQualifiedName, ebnfLexerRule, out var lexerRule))
            {
                return lexerRule;
            }

            var nfa = LexerRuleExpression(ebnfLexerRule);
            var dfa = this._nfaToDfaAlgorithm.Transform(nfa);

            return new DfaLexerRule(dfa, fullyQualifiedName.FullName);
        }

        private bool TryRecognizeSimpleLiteralExpression(
            FullyQualifiedName fullyQualifiedName,
            EbnfLexerRuleExpression ebnfLexerRule, 
            out ILexerRule lexerRule)
        {
            lexerRule = null;

            if (ebnfLexerRule.GetType() == typeof(EbnfLexerRuleExpression))
            {
                var term = ebnfLexerRule.Term;
                if (term.GetType() == typeof(EbnfLexerRuleTerm))
                {
                    var factor = term.Factor;
                    if (factor is EbnfLexerRuleFactorLiteral literal)
                    {
                        lexerRule = new StringLiteralLexerRule(
                            literal.Value,
                            new TokenType(fullyQualifiedName.FullName));

                        return true;
                    }
                }
            }

            return false;
        }

        private INfa LexerRuleExpression(EbnfLexerRuleExpression expression)
        {
            var nfa = LexerRuleTerm(expression.Term);
            if (expression is EbnfLexerRuleExpressionAlteration alteration)
            {
                var alterationNfa = LexerRuleExpression(alteration);
                nfa = nfa.Union(alterationNfa);
            }
            return nfa;
        }

        private INfa LexerRuleTerm(EbnfLexerRuleTerm term)
        {
            var nfa = LexerRuleFactor(term.Factor);
            if (term is EbnfLexerRuleTermConcatenation concatenation)
            {
                var concatNfa = LexerRuleTerm(concatenation.Term);
                nfa = nfa.Concatenation(concatNfa);
            }
            return nfa;
        }

        private INfa LexerRuleFactor(EbnfLexerRuleFactor factor)
        {
            if (factor is EbnfLexerRuleFactorLiteral literal)
            {
                return LexerRuleFactorLiteral(literal);
            }

            if (factor is EbnfLexerRuleFactorRegex regex)
            {
                return LexerRuleFactorRegex(regex);
            }

            throw new InvalidOperationException(
                $"Invalid {nameof(EbnfLexerRuleFactor)} node type detected. Found {factor.GetType().Name}, expected {nameof(EbnfLexerRuleFactorLiteral)} or {nameof(EbnfLexerRuleFactorRegex)}");
        }

        private INfa LexerRuleFactorLiteral(EbnfLexerRuleFactorLiteral ebnfLexerRuleFactorLiteral)
        {
            var literal = ebnfLexerRuleFactorLiteral.Value;
            var states = new NfaState[literal.Length + 1];
            for (var i = 0; i < states.Length; i++)
            {
                var current = new NfaState();
                states[i] = current;

                if (i == 0)
                {
                    continue;
                }

                var previous = states[i - 1];
                previous.AddTransistion(
                    new TerminalNfaTransition(
                        new CharacterTerminal(literal[i - 1]), current));
            }
            return new Nfa(states[0], states[states.Length - 1]);
        }
        
        private INfa LexerRuleFactorRegex(EbnfLexerRuleFactorRegex ebnfLexerRuleFactorRegex)
        {
            var regex = ebnfLexerRuleFactorRegex.Regex;
            return this._regexToNfaAlgorithm.Transform(regex);
        }

        private StartProductionSettingModel StartSetting(EbnfBlockSetting blockSetting)
        {
            var productionName =  GetFullyQualifiedNameFromQualifiedIdentifier(
                blockSetting.Setting.QualifiedIdentifier);
            return new StartProductionSettingModel(productionName);
        }

        private IReadOnlyList<TriviaSettingModel> TriviaSettings(EbnfBlockSetting blockSetting)
        {
            var fullyQualifiedName = GetFullyQualifiedNameFromQualifiedIdentifier(blockSetting.Setting.QualifiedIdentifier);
            var triviaSettingModel = new TriviaSettingModel(fullyQualifiedName);
            return new[] { triviaSettingModel};
        }

        private IReadOnlyList<IgnoreSettingModel> IgnoreSettings(EbnfBlockSetting blockSetting)
        {
            var fullyQualifiedName = GetFullyQualifiedNameFromQualifiedIdentifier(blockSetting.Setting.QualifiedIdentifier);
            var ignoreSettingModel = new IgnoreSettingModel(fullyQualifiedName);
            return new[] { ignoreSettingModel };
        }

        private IEnumerable<ProductionModel> Rule(EbnfRule rule)
        {
            var nonTerminal = GetFullyQualifiedNameFromQualifiedIdentifier(rule.QualifiedIdentifier);
            var productionModel = new ProductionModel(nonTerminal);
            foreach(var production in Expression(rule.Expression, productionModel))
            {
                yield return production;
            }

            yield return productionModel;           
        }

        private IEnumerable<ProductionModel> Expression(EbnfExpression expression, ProductionModel currentProduction)
        {
            foreach (var production in Term(expression.Term, currentProduction))
            {
                yield return production;
            }

            if (expression is EbnfExpressionAlteration expressionAlteration)
            {
                currentProduction.Lambda();

                foreach (var production in Expression(expressionAlteration.Expression, currentProduction))
                {
                    yield return production;
                }
            }
        }

        private IEnumerable<ProductionModel> Grouping(EbnfFactorGrouping grouping, ProductionModel currentProduction)
        {
            var name = grouping.ToString();
            var nonTerminal = new NonTerminal(name);
            var groupingProduction = new ProductionModel(nonTerminal);

            currentProduction.AddWithAnd(new NonTerminalModel(nonTerminal));

            var expression = grouping.Expression;           
            foreach (var production in Expression(expression, groupingProduction))
            {
                yield return production;
            }

            yield return groupingProduction;
        }

        private IEnumerable<ProductionModel> Optional(EbnfFactorOptional optional, ProductionModel currentProduction)
        {
            var name = optional.ToString();
            var nonTerminal = new NonTerminal(name);
            var optionalProduction = new ProductionModel(nonTerminal);

            currentProduction.AddWithAnd(new NonTerminalModel(nonTerminal));

            var expression = optional.Expression;
            foreach (var production in Expression(expression, optionalProduction))
            {
                yield return production;
            }

            optionalProduction.Lambda();
            yield return optionalProduction;
        }

        private IEnumerable<ProductionModel> Repetition(EbnfFactorRepetition repetition, ProductionModel currentProduction)
        {
            var name = repetition.ToString();
            var nonTerminal = new NonTerminal(name);
            var repetitionProduction = new ProductionModel(nonTerminal);

            currentProduction.AddWithAnd(new NonTerminalModel(nonTerminal));

            var expression = repetition.Expression;
            foreach (var production in Expression(expression, repetitionProduction))
            {
                yield return production;
            }

            repetitionProduction.AddWithAnd(new NonTerminalModel(nonTerminal));
            repetitionProduction.Lambda();

            yield return repetitionProduction;
        }

        private IEnumerable<ProductionModel> Term(EbnfTerm term, ProductionModel currentProduction)
        {
            foreach (var production in Factor(term.Factor, currentProduction))
            {
                yield return production;
            }

            if (term is EbnfTermConcatenation concatenation)
            {
                foreach (var production in Term(concatenation.Term, currentProduction))
                {
                    yield return production;
                }
            }
        }

        private IEnumerable<ProductionModel> Factor(EbnfFactor factor, ProductionModel currentProduction)
        {
            switch (factor)
            {
                case EbnfFactorGrouping grouping:
                    foreach (var production in Grouping(grouping, currentProduction))
                    {
                        yield return production;
                    }

                    break;
                case EbnfFactorOptional optional:
                    foreach (var production in Optional(optional, currentProduction))
                    {
                        yield return production;
                    }

                    break;
                case EbnfFactorRepetition repetition:
                    foreach (var production in Repetition(repetition, currentProduction))
                    {
                        yield return production;
                    }

                    break;
                case EbnfFactorIdentifier identifier:
                    var nonTerminal = GetFullyQualifiedNameFromQualifiedIdentifier(identifier.QualifiedIdentifier);
                    currentProduction.AddWithAnd(new NonTerminalModel(nonTerminal));
                    break;
                case EbnfFactorLiteral literal:
                    var stringLiteralRule = new StringLiteralLexerRule(literal.Value);
                    currentProduction.AddWithAnd(new LexerRuleModel(stringLiteralRule));
                    break;
                case EbnfFactorRegex regex:
                    var nfa = this._regexToNfaAlgorithm.Transform(regex.Regex);
                    var dfa = this._nfaToDfaAlgorithm.Transform(nfa);
                    var dfaLexerRule = new DfaLexerRule(dfa, regex.Regex.ToString());
                    currentProduction.AddWithAnd(new LexerRuleModel(dfaLexerRule));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
                        
        private static FullyQualifiedName GetFullyQualifiedNameFromQualifiedIdentifier(EbnfQualifiedIdentifier qualifiedIdentifier)
        {
            var @namespace = new StringBuilder();
            var currentQualifiedIdentifier = qualifiedIdentifier;
            while (currentQualifiedIdentifier is EbnfQualifiedIdentifierConcatenation)
            {
                if (@namespace.Length > 0)
                {
                    @namespace.Append(".");
                }

                @namespace.Append(currentQualifiedIdentifier.Identifier);
                currentQualifiedIdentifier = (currentQualifiedIdentifier as EbnfQualifiedIdentifierConcatenation).QualifiedIdentifier;
            }

            return new FullyQualifiedName(@namespace.ToString(), currentQualifiedIdentifier.Identifier);
        }   
    }
}
