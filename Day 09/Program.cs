using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_09 {
    internal class Program : ProgramStructure<int[][]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ForEach(
                new Parser<string>()
                .ForEach<string, string, char>()
                .Filter(x => x.ToString())
                .Filter(int.Parse)
                .ToArray()
            ).ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(int[][] input) {
            IEnumerable<int> LowPoints = FindLowPoints(input);
            IEnumerable<int> RiskLevel = LowPoints.Select(x => 1 + x);
            return RiskLevel.Sum();
        }

        protected override object SolvePart2(int[][] input) {
            return null;
        }

        private static IEnumerable<int> FindLowPoints(int[][] map) {
            for(int y = 0; y < map.Length; y++) {
                for(int x = 0; x < map[y].Length; x++) {
                    int height = map[y][x];
                    bool foundLower = false;

                    if (y >= 1) foundLower |= map[y - 1][x] <= height;
                    if (y < map.Length - 1) foundLower |= map[y + 1][x] <= height;
                    if(x >= 1) foundLower |= map[y][x - 1] <= height;
                    if(x < map[y].Length - 1) foundLower |= map[y][x + 1] <= height;

                    if (!foundLower) {
                        yield return height;
                    }
                }
            }
        }

    }
}
