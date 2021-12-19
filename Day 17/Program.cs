using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_17 {
    internal class Program : ProgramStructure<Rectangle> {

        Program() : base(new Parser()
            .Parse(new StringReader()) // "target area: x=257..286, y=-101..-57"
            .Parse(x => x.Substring("target area: ".Length)) // "x=257..286, y=-101..-57"
            .Filter(new SeparatedParser(", ")) // "x=257..286", "y=-101..-57"
            .Filter(x => x.Substring(2)) // "257..286", "-101..-57"
            .ForEach(
                new Parser<string>() // "257..286"
                .Filter(new SeparatedParser("..")) // "257", "287"
                .Filter(int.Parse) // 257, 287
            ) // [[257, 287], [-101, -57]]
            .Combine() // 257, 287, -101, -57
            .ToArray()
            .Parse(x => new Rectangle(x[0], x[1] - x[0], x[2], x[3] - x[2]))
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(Rectangle input) {
            // Let t = number of steps (discrete steps)
            // For now, assume positive x only
            // velX(t) = -t + V0x + 1, where t <= V0x
            // X(t) = -0.5t^2 + V0xt + 1/2t where t<= V0x
            // The step that has the furthest x distance is t=V0x + 1
            // Therefore, the furthest distance is x=0.5V0x^2 + 0.5V0x
            // Using quadratic we can find
            // V0x = (-1 + sqrt(8x + 1)) / 2

            // OK, enough math! Lets do the rest with programming because we CAN!
            int V0x = (-1 + (int)Math.Sqrt(8 * input.Left + 1)) / 2;
            int t0 = V0x + 1;
            int topY = int.MinValue;

            for (; true; V0x++) {
                int x = (V0x * V0x + V0x) / 2;
                if (x < input.Left) continue;
                if (x > input.Right) break;

                for (int t = V0x + 1; true; t++) {
                    // Find the maximum possible y we can get
                    int V0y = (-1 + (int)Math.Sqrt(8 * input.Top + 1)) / 2;
                    for(; true; V0y++) {
                        int y = (-t * t + 2 * V0y * t + t) / 2;
                        if (y > input.Top) continue;
                          if (y < input.Bottom) break;
                        topY = Math.Max(topY, y);
                    }
                }

            }


            return topY;
        }

        protected override object SolvePart2(Rectangle input) {
            return null;
        }

    }
}
