using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.ProgramSynthesis.Utils;

namespace ProseTutorial
{
    public static class Semantics
    {
        public static string Substring(string v, Tuple<int, int> pos) => v.Substring(pos.Item1, pos.Item2 - pos.Item1);


        public static Tuple<int, int> PositionPair(int start, int end) {
            return Tuple.Create<int, int>(start, end);
        }

        public static int? AbsPos(string v, int k)
        {
            //TODO update the return statement to consider the case where k is a negative number
            //if k is positive, you should return k-1 but it k is negative, it represents the one-based index from the right to the left,
            //so you should return the length of v + k + 1. 
            return k - 1;
        }
    }
}
