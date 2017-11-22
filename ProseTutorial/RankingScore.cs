﻿using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using System.Text.RegularExpressions;
using Microsoft.ProgramSynthesis.Features;

namespace ProseTutorial
{
    public class RankingScore : Feature<double>
    {
        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

        protected override double GetFeatureValueForVariable(VariableNode variable) => 0;

        [FeatureCalculator(nameof(Semantics.Substring))]
        public static double Substring(double v, double pos1, double pos2) => pos1 * pos2;

        [FeatureCalculator(nameof(Semantics.AbsPos))]
        public static double AbsPos(double v, double k) => k;

        [FeatureCalculator("k", Method = CalculationMethod.FromLiteral)]
        //TODO update the following ranking function 
        //This ranking function should produce higher values for small absolute values of k
        public static double K(int k) => 1.0;
    }
}