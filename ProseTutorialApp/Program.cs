using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
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

        static Grammar grammar = DSLCompiler.
            ParseGrammarFromFile("../../../ProseTutorial/grammar/substring.grammar").Value;
        static SynthesisEngine prose;



        static void Main(string[] args) {
            prose = ConfigureSynthesis(grammar);
            
            string menu = @"Select one of the options: 
1 - provide new example
2 - run top synthesized program on a new input
3 - exit";
            int option = 0; 
            while(option != 3)
            {
                Console.Out.WriteLine(menu);
                try
                {
                    option = Int16.Parse(Console.ReadLine());
                } catch(Exception)
                {
                    Console.Out.WriteLine("Invalid option. Try again.");
                    continue;
                }
                switch (option)
                {
                    case 1: LearnFromNewExample();
                        break;
                    case 2: RunOnNewInput();
                        break;
                    default:
                        Console.Out.WriteLine("Invalid option. Try again.");
                        break;
                }
            }           
        }

        private static Dictionary<State, object> examples = new Dictionary<State, object>();
        private static ProgramNode topProgram;

        private static void LearnFromNewExample()
        {
            Console.Out.Write("Provide a new input-output example (e.g., \"(Gustavo Soares)\",\"Gustavo Soares\"): ");
            try
            {
                string input = Console.ReadLine();
                var startFirstExample = input.IndexOf("\"") + 1;
                var endFirstExample = input.IndexOf("\"", startFirstExample + 1) + 1;
                var startSecondExample = input.IndexOf("\"", endFirstExample + 1) + 1;
                var endSecondExample = input.IndexOf("\"", startSecondExample + 1) + 1;

                if ((startFirstExample >= endFirstExample) || (startSecondExample >= endSecondExample))
                {
                    Console.Out.WriteLine("Invalid example format. Please try again. input and out should be between quotes");
                    return;
                }
                var inputExample = input.Substring(startFirstExample, endFirstExample - startFirstExample - 1);
                var outputExample = input.Substring(startSecondExample, endSecondExample - startSecondExample - 1);

                var inputState = State.CreateForExecution(grammar.InputSymbol, inputExample);
                examples.Add(inputState, outputExample);
            } catch(Exception)
            {
                Console.Out.WriteLine("Invalid example format. Please try again. input and out should be between quotes");
                return;
            }
            var spec = new ExampleSpec(examples);
            Console.Out.WriteLine("Learning a program for examples:");
            foreach (var example in examples)
            {
                Console.WriteLine("\"" + example.Key.Bindings.First().Value + "\" -> \"" + example.Value + "\"");
            }

            var scoreFeature = new RankingScore(grammar);
            var topPrograms = prose.LearnGrammarTopK(spec, scoreFeature, 4, null);
            if (topPrograms.IsEmpty())
            {
                Console.Out.WriteLine("No program was found for this specification.");
                return; 
            }

            topProgram = topPrograms.First();
            Console.Out.WriteLine("Top 4 learned programs:");
            var counter = 1;
            foreach (var program in topPrograms)
            {
                if (counter > 4) break;
                Console.Out.WriteLine("==========================");
                Console.Out.WriteLine("Program " + counter + ": ");
                Console.Out.WriteLine(program.PrintAST(Microsoft.ProgramSynthesis.AST.ASTSerializationFormat.HumanReadable));
                counter++;
            }
        }

        private static void RunOnNewInput()
        {
            if (topProgram == null)
            {
                Console.Out.WriteLine("No program was synthesized. Try to provide new examples first.");
                return; 
            }
            Console.Out.WriteLine("Top program: " + topProgram);

            try
            {
                Console.Out.Write("Insert a new input: ");
                var newInput = Console.ReadLine();
                var startFirstExample = newInput.IndexOf("\"") + 1;
                var endFirstExample = newInput.IndexOf("\"", startFirstExample + 1) + 1;
                newInput = newInput.Substring(startFirstExample, endFirstExample - startFirstExample - 1);
                var newInputState = State.CreateForExecution(grammar.InputSymbol, newInput);
                Console.Out.WriteLine("RESULT: \"" + newInput + "\" -> \"" + topProgram.Invoke(newInputState) + "\"");
            }
            catch (Exception)
            {
                Console.Out.WriteLine("The execution of the program on this input thrown an exception");
            }
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

   
}
