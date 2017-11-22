using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Wrangling.Constraints;
using Newtonsoft.Json;
using ProseTutorial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProseTutorialApp {
    class Program {
        static void Main(string[] args) {
            Console.Out.WriteLine("Provide the input output examples as a JSON (e.g.: {\"Examples\": [{\"Input\": \"(Gustavo Soares)\", \"Output\": \"Gustavo Soares\"}, {\"Input\": \"(Titus Barik)\", \"Output\": \"Titus Barik\"}]}");
            string input = Console.ReadLine();
            var userInput = JsonConvert.DeserializeObject<UserInput>(input);

            var grammar = DSLCompiler.
                ParseGrammarFromFile("../../../ProseTutorial/grammar/substring.grammar");
            var prose = ConfigureSynthesis(grammar.Value);

            var examples = new Dictionary<State, object>();
            foreach(var example in userInput.Examples) {
                var inputState = State.CreateForExecution(grammar.Value.InputSymbol, example.Input);
                examples.Add(inputState, example.Output);
            }
            var spec = new ExampleSpec(examples);
            Console.Out.WriteLine("Learning a program...");
            var scoreFeature = new RankingScore(grammar.Value);
            var topPrograms = prose.LearnGrammarTopK(spec, scoreFeature, 4, null);
            Console.Out.WriteLine("Top 4 learned programs:");
            var counter = 1; 
            foreach (var program in topPrograms) {
                if (counter > 4) break;
                Console.Out.WriteLine("==========================");
                Console.Out.WriteLine("Program " + counter + ": "); 
                Console.Out.WriteLine(program.PrintAST(Microsoft.ProgramSynthesis.AST.ASTSerializationFormat.HumanReadable));
                counter++;
            }
            var topProgram = topPrograms.First();
            Console.Out.WriteLine("Insert a new input:");
            var newInput = Console.ReadLine();
            Console.Out.WriteLine("Output of the top program on the new input: ");
            var newInputState = State.CreateForExecution(grammar.Value.InputSymbol, newInput);
            Console.Out.WriteLine(topProgram.Invoke(newInputState));
            Console.ReadKey();
        }

        public static SynthesisEngine ConfigureSynthesis(Grammar grammar) {
            var witnessFunctions = new WitnessFunctions(grammar);
            var deductiveSynthesis = new DeductiveSynthesis(witnessFunctions);
            var synthesisExtrategies = new ISynthesisStrategy[] { deductiveSynthesis };
            var synthesisConfig = new SynthesisEngine.Config { Strategies = synthesisExtrategies };
            var prose = new SynthesisEngine(grammar, synthesisConfig);
            return prose;
        }
    }

    class UserInput {
        public Example<string, string>[] Examples { get; set; }
    }
}
