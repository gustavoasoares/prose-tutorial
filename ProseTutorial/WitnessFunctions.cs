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
    public static class WitnessFunctions
    {
        [WitnessFunction("Substring", 1)]
        public static ExampleSpec WitnessStartPosition(GrammarRule rule, int parameter, ExampleSpec spec)
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

        [WitnessFunction("Substring", 2)]
        public static ExampleSpec WitnessEndPosition(GrammarRule rule, int parameter, ExampleSpec spec)
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

        [WitnessFunction("AbsPos", 0)]
        public static ExampleSpec WitnessK(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var result = new Dictionary<State, object>();
            foreach (var example in spec.Examples)
            {
                State inputState = example.Key;
                var output = (int) example.Value;
                result[inputState] = output;
            }
            return new ExampleSpec(result);
        }
    }
}
