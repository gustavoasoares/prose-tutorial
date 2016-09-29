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
            //a result of a witness function is a refined example-based specication 
            //Each example is a map from an input state (State) to an output value (object)
            var result = new Dictionary<State, object>();

            //iterate over the input-output examples for Substring operator and refine them to the pos operator
            foreach (var example in spec.Examples)
            {
                State inputState = example.Key;
                //Get the input string value from the input state. 
                var input = inputState[rule.Body[0]] as string;  
                //Get the output value
                var output = example.Value as string;
                //TODO refine the example for the Substring operator into an example for the pos parameter
                //var refinedExample = ...
                //result[inputState] = refinedExample; 
            }
            return new ExampleSpec(result);
        }

        [WitnessFunction("Substring", 2)]
        public static ExampleSpec WitnessEndPosition(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            throw  new NotImplementedException();
        }

        [WitnessFunction("AbsPos", 0)]
        public static ExampleSpec WitnessK(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            //The example for Abs pos has the form:
            // (string) -> int
            // It maps an input string to an int value that represents an index in that string
            var result = new Dictionary<State, object>();
            foreach (var example in spec.Examples)
            {
                //Get the input state
                State inputState = example.Key;
                //get the output example
                var output = (int) example.Value;
                //The example for Abs(k) is the sample example for k 
                //Therefore, we just return the output value
                result[inputState] = output;
            }
            return new ExampleSpec(result);
        }
    }
}
