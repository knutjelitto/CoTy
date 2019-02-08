using Pliant.Collections;
using Pliant.Grammars;
using System.Collections.Generic;
using System;

namespace Pliant.Charts
{
    public class EarleySet : IEarleySet
    {
        private static readonly INormalState[] EmptyNormalStates = { };
        private static readonly ITransitionState[] EmptyTransitionStates = { };
        private UniqueList<INormalState> predictions;
        private UniqueList<INormalState> scans;
        private UniqueList<INormalState> completions;
        private UniqueList<ITransitionState> transitions;

        public IReadOnlyList<INormalState> Predictions
        {
            get
            {
                if (this.predictions == null)
                {
                    return EmptyNormalStates;
                }

                return this.predictions;
            } 
        }

        public IReadOnlyList<INormalState> Scans
        {
            get
            {
                if (this.scans == null)
                {
                    return EmptyNormalStates;
                }

                return this.scans;
            }
        }

        public IReadOnlyList<INormalState> Completions
        {
            get
            {
                if (this.completions == null)
                {
                    return EmptyNormalStates;
                }

                return this.completions;
            }
        }

        public IReadOnlyList<ITransitionState> Transitions
        {
            get
            {
                if (this.transitions == null)
                {
                    return EmptyTransitionStates;
                }

                return this.transitions;
            }
        }

        public int Location { get; }

        public EarleySet(int location)
        {
            Location = location;
        }

        public bool Contains(StateType stateType, IDottedRule dottedRule, int origin)
        {
            if (stateType != StateType.Normal)
            {
                return false;
            }

            var hashCode = NormalStateHashCodeAlgorithm.Compute(dottedRule, origin);
            if (dottedRule.IsComplete)
            {
                return CompletionsContainsHash(hashCode);
            }

            var currentSymbol = dottedRule.PostDotSymbol;
            if (currentSymbol.SymbolType == SymbolType.NonTerminal)
            {
                return PredictionsContainsHash(hashCode);
            }

            return ScansContainsHash(hashCode);
        }

        private bool CompletionsContainsHash(int hashCode)
        {
            if (this.completions == null)
            {
                return false;
            }

            return this.completions.ContainsHash(hashCode);
        }

        private bool PredictionsContainsHash(int hashCode)
        {
            if (this.predictions == null)
            {
                return false;
            }

            return this.predictions.ContainsHash(hashCode);
        }

        private bool ScansContainsHash(int hashCode)
        {
            if (this.scans == null)
            {
                return false;
            }

            return this.scans.ContainsHash(hashCode);
        }

        public bool Enqueue(IState state)
        {
            if (state.StateType == StateType.Transitive)
            {
                return EnqueueTransition(state as ITransitionState);
            }

            return EnqueueNormal(state, state as INormalState);
        }

        private bool EnqueueNormal(IState state, INormalState normalState)
        {
            var dottedRule = state.DottedRule;
            if (!dottedRule.IsComplete)
            {
                var currentSymbol = dottedRule.PostDotSymbol;
                if (currentSymbol.SymbolType == SymbolType.NonTerminal)
                {
                    return AddUniquePrediction(normalState);
                }

                return AddUniqueScan(normalState);
            }

            return AddUniqueCompletion(normalState);
        }

        private bool AddUniqueCompletion(INormalState normalState)
        {
            if (this.completions == null)
            {
                this.completions = new UniqueList<INormalState>();
            }

            return this.completions.AddUnique(normalState);
        }

        private bool AddUniqueScan(INormalState normalState)
        {
            if (this.scans == null)
            {
                this.scans = new UniqueList<INormalState>();
            }

            return this.scans.AddUnique(normalState);
        }

        private bool AddUniquePrediction(INormalState normalState)
        {
            if (this.predictions == null)
            {
                this.predictions = new UniqueList<INormalState>();
            }

            return this.predictions.AddUnique(normalState);
        }

        private bool EnqueueTransition(ITransitionState transitionState)
        {
            if (this.transitions == null)
            {
                this.transitions = new UniqueList<ITransitionState>();
            }

            return this.transitions.AddUnique(transitionState);
        }

        public ITransitionState FindTransitionState(ISymbol searchSymbol)
        {
            foreach (var transition in Transitions)
            {
                var transitionState = transition as TransitionState;
                if (transitionState.Recognized.Equals(searchSymbol))
                {
                    return transitionState;
                }
            }

            return null;
        }

        public INormalState FindSourceState(ISymbol searchSymbol)
        {
            var sourceItemCount = 0;
            INormalState sourceItem = null;

            for (int s = 0; s < Predictions.Count; s++)
            {
                var state = Predictions[s];
                if (state.IsSource(searchSymbol))
                {
                    var moreThanOneSourceItemExists = sourceItemCount > 0;
                    if (moreThanOneSourceItemExists)
                    {
                        return null;
                    }

                    sourceItemCount++;
                    sourceItem = state;
                }
            }
            return sourceItem;
        }        
    }
}