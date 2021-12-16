using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_16 {
    internal class Program : ProgramStructure<Packet> {

        Program() : base(new Parser()
            .Parse(new StringReader())
            .Filter(s => Enumerable.Range(0, s.Length).Where(x => x % 2 == 0).Select(x => s.Substring(x, 2))) // Return groups of 2 hex characters
            .Filter(x => Convert.ToByte(x, 16)) // Convert 2 hex characters into a byte
            .Create<BitBlitz>()
            .Create<Packet>()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
            //new Program().Run(args, "Example2.txt");
        }

        protected override object SolvePart1(Packet input) {
            return SumVersionNumbers(input);
        }

        protected override object SolvePart2(Packet input) {
            return null;
        }

        private static long SumVersionNumbers(Packet packet) {
            return packet.Version + packet.SubPackets.Select(x => SumVersionNumbers(x)).Sum();
        }

    }
}
