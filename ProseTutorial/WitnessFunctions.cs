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
        public static Regex[] UsefulRegexes = {
    new Regex(@"\w+"),  // Word
	new Regex(@"\d+"),  // Number
    new Regex(@"\s+"),  // Number
    new Regex(@".+"),  // Anything
    new Regex(@"$")  // Anything
};

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

        [WitnessFunction("RelPos", 1)]
        public static DisjunctiveExamplesSpec WitnessRegexPair(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (var example in spec.Examples)
            {
                State inputState = example.Key;
                var input = inputState[rule.Body[0]] as string;
                var output = (int)example.Value;

                List<Tuple<Match, Regex>>[] leftMatches, rightMatches;
                BuildStringMatches(input, out leftMatches, out rightMatches);

                var leftRegex = leftMatches[output];
                var rightRegex = rightMatches[output];
                if (leftRegex.Count == 0 || rightRegex.Count == 0)
                    return null;
                var regexes = new List<Tuple<Regex, Regex>>();
                regexes.AddRange(from l in leftRegex
                                 from r in rightRegex
                                 select Tuple.Create(l.Item2, r.Item2));
                result[inputState] = regexes;
            }
            return DisjunctiveExamplesSpec.From(result);
        }

        static void BuildStringMatches(string inp, out List<Tuple<Match, Regex>>[] leftMatches,
                                       out List<Tuple<Match, Regex>>[] rightMatches)
        {
            leftMatches = new List<Tuple<Match, Regex>>[inp.Length + 1];
            rightMatches = new List<Tuple<Match, Regex>>[inp.Length + 1];
            for (int p = 0; p <= inp.Length; ++p)
            {
                leftMatches[p] = new List<Tuple<Match, Regex>>();
                rightMatches[p] = new List<Tuple<Match, Regex>>();
            }
            foreach (Regex r in UsefulRegexes)
            {
                foreach (Match m in r.Matches(inp))
                {
                    leftMatches[m.Index + m.Length].Add(Tuple.Create(m, r));
                    rightMatches[m.Index].Add(Tuple.Create(m, r));
                }
            }
        }

    }
}
