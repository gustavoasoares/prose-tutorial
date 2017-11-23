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
        
        [WitnessFunction(nameof(Semantics.AbsPos), 1)]
        public DisjunctiveExamplesSpec WitnessK(GrammarRule rule, DisjunctiveExamplesSpec spec) {

            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.DisjunctiveExamples) {
                State inputState = example.Key;
                var v = inputState[rule.Body[0]] as string;

                var positions = new List<int>();
                foreach (int pos in example.Value) {
                    positions.Add((int)pos + 1);
                    positions.Add((int)pos - v.Length -1);
                }
                if (positions.Count == 0) return null;
                kExamples[inputState] = positions.Cast<object>();
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

    }
}