using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_07 {
    internal class Program : ProgramStructure<int[]> {

        Program() : base(new Parser()
            .Parse(new StringReader())
            .Filter(new SeparatedParser(","))
            .Filter(int.Parse)
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(int[] input) {
            int skip = input.Length / 2;
            int median = 0;
            if (input.Length % 2 == 0) {
                median = input.OrderBy(x => x).Skip(skip - 1).Take(2).Sum() / 2;
            } else {
                median = input.OrderBy(x => x).Skip(skip).First();
            }

            // Calculate fuel usage
            return input.Select(x => Math.Abs(x - median)).Sum();
        }

        protected override object SolvePart2(int[] input) {
            return null;
        }

    }
}
