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

        /// <summary>
        /// This witness function should deduce the first position for the Substring operator
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="spec"></param>
        /// <returns>Since there may be more than one occurrence of the output in the input string, 
        /// there may be more than one spec for the start position, which is specified using DisjunctiveExamplesSpec
        /// </returns>
        [WitnessFunction(nameof(Semantics.Substring), 1)]
        public DisjunctiveExamplesSpec WitnessStartPosition(GrammarRule rule, ExampleSpec spec) {
            //the spec on the first position for each input state will have type IEnumerable<object> since we may have 
            //more than one possible output
            var result = new Dictionary<State, IEnumerable<object>>();

            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var input = inputState[rule.Body[0]] as string;
                var output = example.Value as string;
                var occurrences = new List<int>();

                ///////////////////////////////////////////////////////////////////////
                //TODO replace the following line by the commented out for-loop bellow where we identify all start positions 
                //and add each one to the occurrences list. 
                ///////////////////////////////////////////////////////////////////////
                occurrences.Add(input.IndexOf(output));
                //for (int i = input.IndexOf(output);i >= 0; i = input.IndexOf(output, i + 1)) {
                //    occurrences.Add((int)i);
                //}

                if (occurrences.Count == 0) return null;
                result[inputState] = occurrences.Cast<object>();
            }
            return new DisjunctiveExamplesSpec(result);

        }

        /// <summary>
        /// This is a conditional witness function. In the presence of multiple occurrences of the output string in the input string,
        /// We can only write a witness function for an end position for each start position. Thus, our witness function for the end position
        /// is conditional on the start position: in addition to an outer spec, it takes an additional input – a spec on its prerequisite start position parameter.
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="spec"></param>
        /// <param name="startSpec">The specification of the start position parameter</param>
        /// <returns></returns>
        [WitnessFunction(nameof(Semantics.Substring), 2, DependsOnParameters = new []{1})]
        public ExampleSpec WitnessEndPosition(GrammarRule rule, ExampleSpec spec, ExampleSpec startSpec) {
            var result = new Dictionary<State, object>();
            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var output = example.Value as string;
                //get the spec on the start position 
                var start = (int)startSpec.Examples[inputState];
                //TODO given the spec on the start position, write a spec on the end position
                //result[inputState] = ...;
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.AbsPos), 1)]
        public DisjunctiveExamplesSpec WitnessK(GrammarRule rule, DisjunctiveExamplesSpec spec) {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.DisjunctiveExamples) {
                State inputState = example.Key;
                var v = inputState[rule.Body[0]] as string;

                var positions = new List<int>();
                foreach (int pos in example.Value) {
                    positions.Add(pos + 1);
                }
                if (positions.Count == 0) return null;
                kExamples[inputState] = positions.Cast<object>();
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

    }
}