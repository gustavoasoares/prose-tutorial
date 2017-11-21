using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Learning.Logging;

namespace ProseTutorial
{
    [TestClass]
    public class SubstringTest
    {
        [TestMethod]
        public void TestLearnSubstringSingleExample()
        {
            //set up the grammar 
            var grammar = DSLCompiler.
                ParseGrammarFromFile("../../grammar/substring.grammar");
            var prose = ConfigureSynthesis(grammar.Value);

            //create the example
            var input = State.CreateForExecution(grammar.Value.InputSymbol, "Gustavo Soares");
            var examples = new Dictionary<State, object> { { input, "Soares" } };
            var spec = new ExampleSpec(examples);

            //learn the set of programs that satisfy the spec 
            var learnedSet = prose.LearnGrammar(spec);

            //run the first synthesized program in the same input and check if 
            //the output is correct
            var programs = learnedSet.RealizedPrograms;
            var output = programs.First().Invoke(input) as string;
            Assert.AreEqual("Soares", output);
        }

        [TestMethod]
        public void TestLearnSubstringTwoExamples()
        {
            var grammar = DSLCompiler.
                ParseGrammarFromFile("../../grammar/substring.grammar");
            var prose = ConfigureSynthesis(grammar.Value);

            var firstInput = State.CreateForExecution(grammar.Value.InputSymbol, "Gustavo Soares");
            var secondInput = State.CreateForExecution(grammar.Value.InputSymbol, "Sumit Gulwani");
            var examples = new Dictionary<State, object> { { firstInput, "Soares" }, { secondInput, "Gulwani" } };
            var spec = new ExampleSpec(examples);

            var learnedSet = prose.LearnGrammar(spec);
            var programs = learnedSet.RealizedPrograms;
            var output = programs.First().Invoke(firstInput) as string;
            Assert.AreEqual("Soares", output);
            var output2 = programs.First().Invoke(secondInput) as string;
            Assert.AreEqual("Gulwani", output2);
        }

        [TestMethod]
        public void TestLearnSubstringOneExample()
        {
            var grammar = DSLCompiler.
                ParseGrammarFromFile("../../grammar/substring.grammar");
            var prose = ConfigureSynthesis(grammar.Value);

            var input = State.CreateForExecution(grammar.Value.InputSymbol, "Gustavo Soares");

            var examples = new Dictionary<State, object> { { input, "Soares" }};
            var spec = new ExampleSpec(examples);

            var scoreFeature = new RankingScore(grammar.Value);
            var topPrograms = prose.LearnGrammarTopK(spec, scoreFeature, 1, null);
            var topProgram = topPrograms.First();
            var output = topProgram.Invoke(input) as string;
            Assert.AreEqual("Soares", output);

            var input2 = State.CreateForExecution(grammar.Value.InputSymbol, "Sumit Gulwani");
            var output2 = topProgram.Invoke(input2) as string;
            Assert.AreEqual("Gulwani", output2);
        }

        public static SynthesisEngine ConfigureSynthesis(Grammar grammar)
        {
            var witnessFunctions = new WitnessFunctions(grammar);
            var deductiveSynthesis = new DeductiveSynthesis(witnessFunctions);
            var synthesisExtrategies = new ISynthesisStrategy[] { deductiveSynthesis };
            var synthesisConfig = new SynthesisEngine.Config { Strategies = synthesisExtrategies };
            var prose = new SynthesisEngine(grammar, synthesisConfig);
            return prose;
        }
    }
}