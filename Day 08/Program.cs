using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_08 {
    internal class Program : ProgramStructure<IEnumerable<Tuple<InputData, OutputData>>> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ForEach(
                new Parser<string>() // For each line
                .Filter(new SeparatedParser(" | "))
                .ToArray()
                .Accumulate(
                    new Parser<string[]>() // Parse input data
                    .Parse(x => x[0])
                    .Filter(new SeparatedParser()) // split string into the individual inputs
                    .Create<InputData>()
                    ,
                    new Parser<string[]>() // Parse output data
                    .Parse(x =>x [1])
                    .Filter(new SeparatedParser()) // Split string into the individual outputs
                    .Create<OutputData>()
                )
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        Dictionary<int, int[]> possibleNumbersFromLength = new() {
            { 2, new int[] { 1 } },
            { 3, new int[] { 7 } },
            { 4, new int[] { 4 } },
            { 5, new int[] { 2, 3, 5 } },
            { 6, new int[] { 0, 6, 9 } },
            { 7, new int[] { 8 } }
        };

        protected override object SolvePart1(IEnumerable<Tuple<InputData, OutputData>> data) {
            // First look for the easy numbers, 1, 4, 7, 8
            int easyDigitCount = 0;
            foreach (var line in data) {
                foreach (string output in line.Item2) {
                    if (possibleNumbersFromLength[output.Length].Length == 1) {
                        easyDigitCount++;
                    }
                }
            }

            return easyDigitCount;
        }

        protected override object SolvePart2(IEnumerable<Tuple<InputData, OutputData>> data) {

            
            return null;
        }

    }
}
