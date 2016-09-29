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
      public static string Substring(string x, int? start, int? end)
        {
            return x.Substring((int)start, (int)end - (int)start);
        }

        public static int? AbsPos(int k)
        {
            return k;
        }

        public static int? RelPos(string x, Tuple<Regex, Regex> rr)
        {
            Regex left = rr.Item1;
            Regex right = rr.Item2;
            var rightMatches = right.Matches(x);

            foreach (Match leftMatch in left.Matches(x))
            {
                foreach (Match rightMatch in rightMatches)
                {
                    if (rightMatch.Index == leftMatch.Index + leftMatch.Length)
                        return leftMatch.Index + leftMatch.Length;
                }
            }
            return null;
        }
    }
}
