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

        /// <summary>
        /// This witness function  deduces a specification on the second parameter (index 1) of the Substring operator 
        /// given a specification on the entire Substring operator
        /// </summary>
        /// <param name="rule">The Substring operator's rule</param>
        /// <param name="spec">The specification for the Substring operator</param>
        /// <returns>The specification for the first position on the Substring operator</returns>
        [WitnessFunction(nameof(Semantics.Substring), 1)]
        public ExampleSpec WitnessStartPosition(GrammarRule rule, ExampleSpec spec)
        {
            //a result of a witness function is a refined example-based specication 
            //Each example is a map from an input state (State) to an output value (object)
            var result = new Dictionary<State, object>();

            //iterate over the input-output examples for the Substring operator 
            foreach (var example in spec.Examples)
            {
                //get the input state of the current example
                State inputState = example.Key;
                // the first parameter of Substring is the input variable 'v'
                // we extract its current bound value from the given input state
                // (e.g., "(Gustavo Soares)")
                var input = inputState[rule.Body[0]] as string;  
                //Get the output value (e.g., "Gustavo Soares")
                var output = example.Value as string;
                //now we deduce a spec on the first position of the substring
                //In this tutorial, for simplification, we consider just the first ocurrence of the substring
                ////////////////////////////////////////////////////////////////////////////////
                //TODO uncomment the following code fragment to complete this witness function//
                ////////////////////////////////////////////////////////////////////////////////
                //var refinedExample = input.IndexOf(output);
                //result[inputState] = refinedExample;
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
                //similar to the previous witness fnction, we deduce a spec on the second position of the substring
                ////////////////////////////////////////////////////////////////////////////////
                //TODO uncomment the following code fragment to complete this witness function//
                ////////////////////////////////////////////////////////////////////////////////
                //var refinedExample = input.IndexOf(output) + output.Length;
                //result[inputState] = refinedExample;
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction(nameof(Semantics.AbsPos), 1)]
        public DisjunctiveExamplesSpec WitnessK(GrammarRule rule, ExampleSpec spec)
        {
            var result = new Dictionary<State, object>();
            foreach (var example in spec.Examples) {
                State inputState = example.Key;
                var v = inputState[rule.Body[0]] as string;
                var pos = (int)example.Value;
            }
            return new ExampleSpec(result);
        }
    }
}