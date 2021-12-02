using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_02 {
    internal class Program : ProgramStructure<IEnumerable<SubInstruction>> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Filter(x => x.Split())
            .FilterCreate<SubInstruction>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(IEnumerable<SubInstruction> input) {
            Point position = new();

            foreach(SubInstruction instruction in input) {
                position += instruction;
            }

            return position.X * position.Y;
        }

        protected override object SolvePart2(IEnumerable<SubInstruction> input) {
            return null;
        }

    }
}
