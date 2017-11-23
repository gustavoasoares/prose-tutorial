using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.ProgramSynthesis;
using System.Threading.Tasks;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Learning;

namespace ProseTutorial {
    public class WitnessFunctions : DomainLearningLogic {
        public WitnessFunctions(Grammar grammar) : base(grammar) { }

        [WitnessFunction(nameof(Semantics.Substring), 1)]
        public DisjunctiveExamplesSpec WitnessPositionPair(GrammarRule rule, ExampleSpec spec) {
            var result = new Dictionary<State, IEnumerable<object>>();

            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var input = inputState[rule.Body[0]] as string;
                var output = example.Value as string;
                var occurrences = new List<Tuple<int, int>>();

                for (int i = input.IndexOf(output); i >= 0; i = input.IndexOf(output, i + 1)) {
                    occurrences.Add(Tuple.Create(i, i + output.Length));
                }

                if (occurrences.Count == 0) return null;
                result[inputState] = occurrences;
            }
            return new DisjunctiveExamplesSpec(result);

        }

        [WitnessFunction(nameof(Semantics.PositionPair), 0)]
        public DisjunctiveExamplesSpec WitnessPositionPairStartPosition(GrammarRule rule, DisjunctiveExamplesSpec spec) {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.DisjunctiveExamples) {
                State inputState = example.Key;

                var positions = new List<int>();
                foreach (Tuple<int, int> pair in example.Value) {
                    positions.Add(pair.Item1);
                }
                if (positions.Count == 0) return null;
                result[inputState] = positions.Cast<object>();
            }
            return DisjunctiveExamplesSpec.From(result);
        }

        [WitnessFunction(nameof(Semantics.PositionPair), 1)]
        public DisjunctiveExamplesSpec WitnessPositionPairEndPosition(GrammarRule rule, DisjunctiveExamplesSpec spec) {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.DisjunctiveExamples) {
                State inputState = example.Key;

                var positions = new List<int>();
                foreach (Tuple<int, int> pair in example.Value) {
                    positions.Add(pair.Item2);
                }
                if (positions.Count == 0) return null;
                result[inputState] = positions.Cast<object>();
            }
            return DisjunctiveExamplesSpec.From(result);
        }

        /// <summary>
        /// This witness function should deduce the spec for k given the spec for AbsPos     
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="spec"></param>
        /// <returns>However, now we need to produce two possible specs for k (positive and negative)
        /// given a single spec for AbsPos. A disjunction of possible specs has its own 
        /// representative spec type in PROSE – DisjunctiveExamplesSpec.</returns>
        [WitnessFunction(nameof(Semantics.AbsPos), 1)]
        public DisjunctiveExamplesSpec WitnessK(GrammarRule rule, DisjunctiveExamplesSpec spec) {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.DisjunctiveExamples) {
                State inputState = example.Key;
                var v = inputState[rule.Body[0]] as string;

                var positions = new List<int>();
                foreach (int pos in example.Value) {
                    //the positive spec for k
                    positions.Add(pos + 1);
                    //TODO add the negative spec for k 
                    //uncomment the next statement and replace X by the expression that should return the negative spec
                    //positions.Add(X);
                }
                if (positions.Count == 0) return null;
                kExamples[inputState] = positions.Cast<object>();
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

    }
}