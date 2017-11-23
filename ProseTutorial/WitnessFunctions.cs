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
        public ExampleSpec WitnessPositionPair(GrammarRule rule, ExampleSpec spec) {
            var result = new Dictionary<State, object>();

            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var input = inputState[rule.Body[0]] as string;
                var output = example.Value as string;
                var start = input.IndexOf(output);
                var end = input.IndexOf(output) + output.Length;
                result[inputState] = Tuple.Create(start, end);
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.PositionPair), 0)]
        public ExampleSpec WitnessPositionPairStartPosition(GrammarRule rule, ExampleSpec spec) {
            var result = new Dictionary<State, object>();
            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var output = example.Value as Tuple<int, int>;
                result[inputState] = output.Item1;
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.PositionPair), 1)]
        public ExampleSpec WitnessPositionPairEndPosition(GrammarRule rule, ExampleSpec spec) {
            var result = new Dictionary<State, object>();
            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var output = example.Value as Tuple<int, int>;
                result[inputState] = output.Item2;
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.AbsPos), 1)]
        public DisjunctiveExamplesSpec WitnessK(GrammarRule rule, ExampleSpec spec) {
            var result = new Dictionary<State, object>();
            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var v = inputState[rule.Body[0]] as string;
                var pos = (int)example.Value;
                result[inputState] = pos + 1;
            }
            return new ExampleSpec(result);
        }
    }
}