// RegExp -> NFA -> DFA -> Graph in Generic C#
// This program requires .Net version 2.0.
// Peter Sestoft (sestoft@itu.dk) 
// Java 2000-10-07, GC# 2001-10-23, 2003-09-03, 2004-07-26, 2006-03-05

// This file contains, in order:
//   * A class Nfa for representing an NFA (a nondeterministic finite 
//     automaton), and for converting it to a DFA (a deterministic 
//     finite automaton).  Most complexity is in this class.
//   * A class Dfa for representing a DFA, a deterministic finite 
//     automaton, and for writing a dot input file representing the DFA.
//   * Classes for representing regular expressions, and for building an 
//     NFA from a regular expression
//   * A test class that creates an NFA, a DFA, and a dot input file 
//     for a number of small regular expressions.  The DFAs are 
//     not minimized.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NuEarl.Regular
{
// A set represented as the collection of keys of a Dictionary

    public class Set<T> : ICollection<T>
    {
        // Only the keys matter; the type bool used for the value is arbitrary
        private readonly Dictionary<T, bool> dict;
        public Set()
        {
            this.dict = new Dictionary<T, bool>();
        }

        public Set(T x) : this()
        {
            Add(x);
        }

        public Set(IEnumerable<T> coll) : this()
        {
            foreach (var x in coll)
                Add(x);
        }

        public bool Contains(T x)
        {
            return this.dict.ContainsKey(x);
        }

        public void Add(T x)
        {
            if (!Contains(x)) this.dict.Add(x, false);
        }

        public bool Remove(T x)
        {
            return this.dict.Remove(x);
        }

        public void Clear()
        {
            this.dict.Clear();
        }

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator()
        {
            return this.dict.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => this.dict.Count;

        public void CopyTo(T[] arr, int i)
        {
            this.dict.Keys.CopyTo(arr, i);
        }

        // Is this set a subset of that?
        private bool Subset(Set<T> that)
        {
            foreach (var x in this)
                if (!that.Contains(x))
                    return false;
            return true;
        }

        // Create new set as intersection of this and that
        public Set<T> Intersection(Set<T> that)
        {
            var res = new Set<T>();
            foreach (var x in this)
                if (that.Contains(x))
                    res.Add(x);
            return res;
        }

        // Create new set as union of this and that
        public Set<T> Union(Set<T> that)
        {
            var res = new Set<T>(this);
            foreach (var x in that)
                res.Add(x);
            return res;
        }

        // Compute hash code -- should be cached for efficiency
        public override int GetHashCode()
        {
            var res = 0;
            foreach (var x in this)
                res ^= x.GetHashCode();
            return res;
        }

        public override bool Equals(object obj)
        {
            if (obj is Set<T> other)
            {
                return other.Count == Count &&
                       other.Subset(this) && Subset(other);
            }

            return false;
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.Append("{ ");
            var first = true;
            foreach (var x in this)
            {
                if (!first)
                    res.Append(", ");
                res.Append(x);
                first = false;
            }
            res.Append(" }");
            return res.ToString();
        }
    }

// ----------------------------------------------------------------------

// Regular expressions, NFAs, DFAs, and dot graphs
// sestoft@itu.dk 
// Java 2001-07-10 * C# 2001-10-22 * Gen C# 2001-10-23, 2003-09-03

// In the Generic C# 2.0 version we 
//  use Queue<int> and Queue<Set<int>> for worklists
//  use Set<int> for pre-DFA states
//  use List<Transition> for NFA transition relations
//  use Dictionary<Set<int>, Dictionary<String, Set<int>>>
//  and Dictionary<int, Dictionary<String, int>> for DFA transition relations

/* Class Nfa and conversion from NFA to DFA ---------------------------

  A nondeterministic finite automaton (NFA) is represented as a
  Map from state number (int) to a List of Transitions, a
  Transition being a pair of a label lab (a String, null meaning
  epsilon) and a target state (an int).

  A DFA is created from an NFA in two steps:

    (1) Construct a DFA whose each of whose states is composite,
        namely a set of NFA states (Set of int).  This is done by
        methods CompositeDfaTrans and EpsilonClose.

    (2) Replace composite states (Set of int) by simple states
        (int).  This is done by methods Rename and MkRenamer.

  Method CompositeDfaTrans works as follows: 

    Create the epsilon-closure S0 (a Set of ints) of the start
    state s0, and put it in a worklist (a Queue).  Create an
    empty DFA transition relation, which is a Map from a
    composite state (an epsilon-closed Set of ints) to a Map
    from a label (a non-null String) to a composite state.

    Repeatedly choose a composite state S from the worklist.  If it is
    not already in the keyset of the DFA transition relation, compute
    for every non-epsilon label lab the set T of states reachable by
    that label from some state s in S.  Compute the epsilon-closure
    Tclose of every such state T and put it on the worklist.  Then add
    the transition S -lab-> Tclose to the DFA transition relation, for
    every lab.

  Method EpsilonClose works as follows: 

    Given a set S of states.  Put the states of S in a worklist.
    Repeatedly choose a state s from the worklist, and consider all
    epsilon-transitions s -eps-> s' from s.  If s' is in S already,
    then do nothing; otherwise add s' to S and the worklist.  When the
    worklist is empty, S is epsilon-closed; return S.

  Method MkRenamer works as follows: 

    Given a Map from Set of int to something, create an
    injective Map from Set of int to int, by choosing a fresh
    int for every value of the map.

  Method Rename works as follows:

    Given a Map from Set of int to Map from String to Set of
    int, use the result of MkRenamer to replace all Sets of ints
    by ints.

*/


    public class Nfa
    {
        private readonly int startState;
        private readonly int exitState;    // This is the unique accept state
        private readonly IDictionary<int, List<Transition>> trans;

        public Nfa(int startState, int exitState)
        {
            this.startState = startState; this.exitState = exitState;
            this.trans = new Dictionary<int, List<Transition>>();
            if (!startState.Equals(exitState)) this.trans.Add(exitState, new List<Transition>());
        }

        public int Start => this.startState;

        public int Exit => this.exitState;

        public IDictionary<int, List<Transition>> Trans => this.trans;

        public void AddTrans(int s1, string lab, int s2)
        {
            List<Transition> s1Trans;
            if (this.trans.ContainsKey(s1))
                s1Trans = this.trans[s1];
            else
            {
                s1Trans = new List<Transition>();
                this.trans.Add(s1, s1Trans);
            }
            s1Trans.Add(new Transition(lab, s2));
        }

        public void AddTrans(KeyValuePair<int, List<Transition>> tr)
        {
            // Assumption: if tr is in trans, it maps to an empty list (end state)
            this.trans.Remove(tr.Key);
            this.trans.Add(tr.Key, tr.Value);
        }

        public override string ToString()
        {
            return "NFA start=" + this.startState + " exit=" + this.exitState;
        }

        // Construct the transition relation of a composite-state DFA
        // from an NFA with start state s0 and transition relation
        // trans (a Map from int to List of Transition).  The start
        // state of the constructed DFA is the epsilon closure of s0,
        // and its transition relation is a Map from a composite state
        // (a Set of ints) to a Map from label (a String) to a
        // composite state (a Set of ints).

        private static IDictionary<Set<int>, IDictionary<string, Set<int>>>
            CompositeDfaTrans(int s0, IDictionary<int, List<Transition>> trans)
        {
            var S0 = EpsilonClose(new Set<int>(s0), trans);
            var worklist = new Queue<Set<int>>();
            worklist.Enqueue(S0);
            // The transition relation of the DFA
            IDictionary<Set<int>, IDictionary<string, Set<int>>> res =
                new Dictionary<Set<int>, IDictionary<string, Set<int>>>();
            while (worklist.Count != 0)
            {
                var S = worklist.Dequeue();
                if (!res.ContainsKey(S))
                {
                    // The S -lab-> T transition relation being constructed for a given S
                    IDictionary<string, Set<int>> STrans =
                        new Dictionary<string, Set<int>>();
                    // For all s in S, consider all transitions s -lab-> t
                    foreach (var s in S)
                    {
                        // For all non-epsilon transitions s -lab-> t, add t to T
                        foreach (var tr in trans[s])
                        {
                            if (tr.lab != null)
                            {       // Already a transition on lab
                                Set<int> toState;
                                if (STrans.ContainsKey(tr.lab))
                                    toState = STrans[tr.lab];
                                else
                                {                    // No transitions on lab yet
                                    toState = new Set<int>();
                                    STrans.Add(tr.lab, toState);
                                }
                                toState.Add(tr.target);
                            }
                        }
                    }
                    // Epsilon-close all T such that S -lab-> T, and put on worklist
                    var STransClosed =
                        new Dictionary<string, Set<int>>();
                    foreach (var entry in STrans)
                    {
                        var Tclose = EpsilonClose(entry.Value, trans);
                        STransClosed.Add(entry.Key, Tclose);
                        worklist.Enqueue(Tclose);
                    }
                    res.Add(S, STransClosed);
                }
            }
            return res;
        }

        // Compute epsilon-closure of state set S in transition relation trans.  

        private static Set<int>
            EpsilonClose(Set<int> S, IDictionary<int, List<Transition>> trans)
        {
            // The worklist initially contains all S members
            var worklist = new Queue<int>(S);
            var res = new Set<int>(S);
            while (worklist.Count != 0)
            {
                var s = worklist.Dequeue();
                foreach (var tr in trans[s])
                {
                    if (tr.lab == null && !res.Contains(tr.target))
                    {
                        res.Add(tr.target);
                        worklist.Enqueue(tr.target);
                    }
                }
            }
            return res;
        }

        // Compute a renamer, which is a Map from Set of int to int

        private static IDictionary<Set<int>, int> MkRenamer(ICollection<Set<int>> states)
        {
            IDictionary<Set<int>, int> renamer = new Dictionary<Set<int>, int>();
            var count = 0;
            foreach (var k in states)
                renamer.Add(k, count++);
            return renamer;
        }

        // Using a renamer (a Map from Set of int to int), replace
        // composite (Set of int) states with simple (int) states in
        // the transition relation trans, which is assumed to be a Map
        // from Set of int to Map from String to Set of int.  The
        // result is a Map from int to Map from String to int.

        private static IDictionary<int, IDictionary<string, int>>
            Rename(IDictionary<Set<int>, int> renamer,
                   IDictionary<Set<int>, IDictionary<string, Set<int>>> trans)
        {
            IDictionary<int, IDictionary<string, int>> newtrans =
                new Dictionary<int, IDictionary<string, int>>();
            foreach (var entry
                in trans)
            {
                var k = entry.Key;
                IDictionary<string, int> newktrans = new Dictionary<string, int>();
                foreach (var tr in entry.Value)
                    newktrans.Add(tr.Key, renamer[tr.Value]);
                newtrans.Add(renamer[k], newktrans);
            }
            return newtrans;
        }

        private static Set<int> AcceptStates(ICollection<Set<int>> states,
                                             IDictionary<Set<int>, int> renamer,
                                             int exit)
        {
            var acceptStates = new Set<int>();
            foreach (var state in states)
                if (state.Contains(exit))
                    acceptStates.Add(renamer[state]);
            return acceptStates;
        }

        public Dfa ToDfa()
        {
            var
                cDfaTrans = CompositeDfaTrans(this.startState, this.trans);
            var cDfaStart = EpsilonClose(new Set<int>(this.startState), this.trans);
            var cDfaStates = cDfaTrans.Keys;
            var renamer = MkRenamer(cDfaStates);
            var simpleDfaTrans =
                Rename(renamer, cDfaTrans);
            var simpleDfaStart = renamer[cDfaStart];
            var simpleDfaAccept = AcceptStates(cDfaStates, renamer, this.exitState);
            return new Dfa(simpleDfaStart, simpleDfaAccept, simpleDfaTrans);
        }

        // Nested class for creating distinctly named states when constructing NFAs

        public class NameSource
        {
            private static int nextName;

            public int next()
            {
                return nextName++;
            }
        }
    }

// Class Transition, a transition from one state to another ----------

    public class Transition
    {
        public readonly string lab;
        public readonly int target;

        public Transition(string lab, int target)
        {
            this.lab = lab; this.target = target;
        }

        public override string ToString()
        {
            return "-" + this.lab + "-> " + this.target;
        }
    }

// Class Dfa, deterministic finite automata --------------------------

/*
  A deterministic finite automaton (DFA) is represented as a Map
  from state number (int) to a Map from label (a String,
  non-null) to a target state (an int).  
*/

    public class Dfa
    {
        public Dfa(int startState, Set<int> acceptStates,
                   IDictionary<int, IDictionary<string, int>> trans)
        {
            Start = startState;
            Accept = acceptStates;
            Trans = trans;
        }

        public int Start { get; }

        public Set<int> Accept { get; }

        public IDictionary<int, IDictionary<string, int>> Trans { get; }

        public override string ToString()
        {
            return "DFA start=" + Start + "\naccept=" + Accept;
        }
    }

    // Regular expressions ----------------------------------------------
    //
    // Abstract syntax of regular expressions
    //    r ::= A | r1 r2 | (r1|r2) | r*
    //

    public abstract class Regex
    {
        public abstract Nfa MkNfa(Nfa.NameSource names);
    }

    internal class Eps : Regex
    {
        // The resulting nfa0 has form s0s -eps-> s0e

        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var s0s = names.next();
            var s0e = names.next();
            var nfa0 = new Nfa(s0s, s0e);
            nfa0.AddTrans(s0s, null, s0e);
            return nfa0;
        }
    }

    internal class Sym : Regex
    {
        private readonly string sym;

        public Sym(string sym)
        {
            this.sym = sym;
        }

        // The resulting nfa0 has form s0s -sym-> s0e

        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var s0s = names.next();
            var s0e = names.next();
            var nfa0 = new Nfa(s0s, s0e);
            nfa0.AddTrans(s0s, this.sym, s0e);
            return nfa0;
        }
    }

    internal class Seq : Regex
    {
        private readonly Regex r1;
        private readonly Regex r2;

        public Seq(Regex r1, Regex r2)
        {
            this.r1 = r1; this.r2 = r2;
        }

        // If   nfa1 has form s1s ----> s1e 
        // and  nfa2 has form s2s ----> s2e 
        // then nfa0 has form s1s ----> s1e -eps-> s2s ----> s2e

        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var nfa1 = this.r1.MkNfa(names);
            var nfa2 = this.r2.MkNfa(names);
            var nfa0 = new Nfa(nfa1.Start, nfa2.Exit);
            foreach (var entry in nfa1.Trans)
                nfa0.AddTrans(entry);
            foreach (var entry in nfa2.Trans)
                nfa0.AddTrans(entry);
            nfa0.AddTrans(nfa1.Exit, null, nfa2.Start);
            return nfa0;
        }
    }

    internal class Alt : Regex
    {
        private readonly Regex r1;
        private readonly Regex r2;

        public Alt(Regex r1, Regex r2)
        {
            this.r1 = r1; this.r2 = r2;
        }

        // If   nfa1 has form s1s ----> s1e 
        // and  nfa2 has form s2s ----> s2e 
        // then nfa0 has form s0s -eps-> s1s ----> s1e -eps-> s0e
        //                    s0s -eps-> s2s ----> s2e -eps-> s0e

        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var nfa1 = this.r1.MkNfa(names);
            var nfa2 = this.r2.MkNfa(names);
            var s0s = names.next();
            var s0e = names.next();
            var nfa0 = new Nfa(s0s, s0e);
            foreach (var entry in nfa1.Trans)
                nfa0.AddTrans(entry);
            foreach (var entry in nfa2.Trans)
                nfa0.AddTrans(entry);
            nfa0.AddTrans(s0s, null, nfa1.Start);
            nfa0.AddTrans(s0s, null, nfa2.Start);
            nfa0.AddTrans(nfa1.Exit, null, s0e);
            nfa0.AddTrans(nfa2.Exit, null, s0e);
            return nfa0;
        }
    }

    internal class Star : Regex
    {
        private readonly Regex r;

        public Star(Regex r)
        {
            this.r = r;
        }

        // If   nfa1 has form s1s ----> s1e 
        // then nfa0 has form s0s ----> s0s
        //                    s0s -eps-> s1s
        //                    s1e -eps-> s0s

        public override Nfa MkNfa(Nfa.NameSource names)
        {
            var nfa1 = this.r.MkNfa(names);
            var s0s = names.next();
            var nfa0 = new Nfa(s0s, s0s);
            foreach (var entry in nfa1.Trans)
                nfa0.AddTrans(entry);
            nfa0.AddTrans(s0s, null, nfa1.Start);
            nfa0.AddTrans(nfa1.Exit, null, s0s);
            return nfa0;
        }
    }

// Trying the RE->NFA->DFA translation on three regular expressions

    public class TestNFA
    {
        public void Test()
        {
            Regex a = new Sym("A");
            Regex b = new Sym("B");
            Regex abStar = new Star(new Alt(a, b));
            Regex bb = new Seq(b, b);
            Regex r = new Seq(abStar, new Seq(a, b));
            // The regular expression (a|b)*ab
            BuildAndShow(r);
            // The regular expression ((a|b)*ab)*
            BuildAndShow(new Star(r));
            // The regular expression ((a|b)*ab)((a|b)*ab)
            BuildAndShow(new Seq(r, r));
            // The regular expression (a|b)*abb, from ASU 1986 p 136
            BuildAndShow(new Seq(abStar, new Seq(a, bb)));
            // SML reals: sign?((digit+(\.digit+)?))([eE]sign?digit+)?
            Regex d = new Sym("digit");
            Regex dPlus = new Seq(d, new Star(d));
            Regex s = new Sym("sign");
            Regex sOpt = new Alt(s, new Eps());
            Regex dot = new Sym(".");
            Regex dotDigOpt = new Alt(new Eps(), new Seq(dot, dPlus));
            Regex mant = new Seq(sOpt, new Seq(dPlus, dotDigOpt));
            Regex e = new Sym("e");
            Regex exp = new Alt(new Eps(), new Seq(e, new Seq(sOpt, dPlus)));
            Regex smlReal = new Seq(mant, exp);
            BuildAndShow(smlReal);
        }

        private void BuildAndShow(Regex r)
        {
            var nfa = r.MkNfa(new Nfa.NameSource());
            Console.WriteLine(nfa);
            Console.WriteLine("---");
            var dfa = nfa.ToDfa();
            Console.WriteLine(dfa);
            Console.WriteLine();
        }
    }
}