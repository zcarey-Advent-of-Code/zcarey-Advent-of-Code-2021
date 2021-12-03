using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_03 {
    internal class Program : ProgramStructure<string[]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(string[] input) {
            int[] numberOfOnes = new int[input[0].Length];
            int half = input.Length / 2;

            foreach(string binary in input) {
                for (int i = 0; i < binary.Length; i++) {
                    if (binary[i] == '1') {
                        numberOfOnes[i]++;
                    }
                }
            }

            int gamma = 0;
            int epsilon = 0;
            for(int i = 0; i < numberOfOnes.Length; i++) {
                int bit = (numberOfOnes[i] > half) ? 1 : 0;
                gamma = (gamma << 1) | bit;
                epsilon = (epsilon << 1) | (1 - bit);
            }

            return gamma * epsilon;
        }

        protected override object SolvePart2(string[] input) {
            return null;
        }

    }
}
