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
        public static double Substring(double x, double pos1, double pos2) => pos1 + pos2;

        [FeatureCalculator(nameof(Semantics.AbsPos))]
        public static double AbsPos(double k) => k;

        [FeatureCalculator("k", Method = CalculationMethod.FromLiteral)]
        public static double K(int k) => 0;


        [FeatureCalculator(nameof(Semantics.RelPos))]
        public static double RelPos(double x, double rr) => 1;

        [FeatureCalculator("rr", Method = CalculationMethod.FromLiteral)]
        public static double RR(Tuple<Regex, Regex> tuple) => 0;    
    }
}