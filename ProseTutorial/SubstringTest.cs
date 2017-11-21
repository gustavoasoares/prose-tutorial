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
        public void TestSubstringProgram()
        {
            var grammar = DSLCompiler.
                ParseGrammarFromFile("../../grammar/substring.grammar");
            var program = ProgramNode.Parse("Substring(x, AbsPos(7), AbsPos(15))",
                grammar.Value,
                ASTSerializationFormat.XML);

            var input = State.CreateForExecution(grammar.Value.InputSymbol, "Bjoern Hartmann");
            var output = program.Invoke(input) as string;
            Assert.AreEqual("Hartmann", output);
        }

        [TestMethod]
        public void TestLearnSubstringSingleExample()
        {
            var grammar = DSLCompiler.
                ParseGrammarFromFile("../../grammar/substring.grammar");

            var prose = ConfigureSynthesis(grammar.Value);
            var input = State.CreateForExecution(grammar.Value.InputSymbol, "Bjoern Hartmann");
            var examples = new Dictionary<State, object> { { input, "Hartmann" } };
            var spec = new ExampleSpec(examples);
            var learnedSet = prose.LearnGrammar(spec);
            var output = learnedSet.RealizedPrograms.First().Invoke(input) as string;
            Assert.AreEqual("Hartmann", output);
        }

        [TestMethod]
        public void TestLearnSubstring()
        {
            var grammar = DSLCompiler.
                ParseGrammarFromFile("../../grammar/substring.grammar");

            var prose = ConfigureSynthesis(grammar.Value);

            var input = State.CreateForExecution(grammar.Value.InputSymbol, "Bjoern Hartmann");
            var input2 = State.CreateForExecution(grammar.Value.InputSymbol, "Andrew Head");
            var examples = new Dictionary<State, object> { { input, "Hartmann" }, { input2, "Head" } };
            var spec = new ExampleSpec(examples);
            var learnedSet = prose.LearnGrammar(spec);
            var output = learnedSet.RealizedPrograms.First().Invoke(input) as string;
            Assert.AreEqual("Hartmann", output);
            var output2 = learnedSet.RealizedPrograms.First().Invoke(input2) as string;
            Assert.AreEqual("Head", output2);
        }


        [TestMethod]
        public void TestLearnStringTransformation()
        {
            var grammar = DSLCompiler.
                ParseGrammarFromFile("../../grammar/substring.grammar");

            var prose = ConfigureSynthesis(grammar.Value);

            var input = State.CreateForExecution(grammar.Value.InputSymbol, "Bjoern Hartmann");
            var examples = new Dictionary<State, object> { { input, "Hartmann" } };
            var spec = new ExampleSpec(examples);
            var learnedSet = prose.LearnGrammar(spec);
            var output = learnedSet.RealizedPrograms.First().Invoke(input) as string;
            Assert.AreEqual("Hartmann, B.", output);
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