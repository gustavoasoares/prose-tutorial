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
        public static string Substring(string v, int? start, int? end) => v.Substring((int)start, (int)end - (int)start);

        public static int? AbsPos(string v, int k)
        {
            if (Math.Abs(k) >= v.Length + 1) return null;

            return k - 1;
        }
    }
}
