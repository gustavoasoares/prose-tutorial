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

namespace ProseTutorial
{
    public class WitnessFunctions : DomainLearningLogic
    {
        public WitnessFunctions(Grammar grammar) : base(grammar) { }

        [WitnessFunction(nameof(Semantics.Substring), 1)]
        public ExampleSpec WitnessStartPosition(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, object>();

            foreach (var example in spec.Examples)
            {
                State inputState = example.Key;
                var input = inputState[rule.Body[0]] as string;  
                var output = example.Value as string;
                var refinedExample = input.IndexOf(output);
                result[inputState] = refinedExample; 
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.Substring), 2)]
        public ExampleSpec WitnessEndPosition(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, object>();
            foreach (var example in spec.Examples)
            {
                State inputState = example.Key;
                var input = inputState[rule.Body[0]] as string;
                var output = example.Value as string;
                var refinedExample = input.IndexOf(output) + output.Length;
                result[inputState] = refinedExample;
            }
            return new ExampleSpec(result);
        }

        /// <summary>
        /// This witness function should deduce the spec for k given the spect for AbsPos     
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="spec"></param>
        /// <returns>However, now we need to produce two possible specs for k (positive and negative)
        /// given a single spec for AbsPos. A disjunction of possible specs has its own 
        /// representative spec type in PROSE – DisjunctiveExamplesSpec.</returns>
        [WitnessFunction(nameof(Semantics.AbsPos), 1)]
        public DisjunctiveExamplesSpec WitnessK(GrammarRule rule, ExampleSpec spec) {

            //the spec on k for each input state will have type IEnumerable<object> since we will have 
            //more than one possible output
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var v = inputState[rule.Body[0]] as string;
                var pos = (int)example.Value;

                var positions = new List<object>();
                //the positive spec for k
                positions.Add((int)pos + 1);
                //TODO add the negative spec for k 
                //uncomment the next statement and replace X by the expression that should return the negative spec
                //positions.Add((int)pos - X);
                kExamples[inputState] = positions;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

    }
}