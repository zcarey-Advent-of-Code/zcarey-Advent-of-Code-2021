using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_10 {
    internal class Program : ProgramStructure<string[]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        private static Dictionary<char, char> ClosingBrackets = new() {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' }
        };

        private static Dictionary<char, int> ScoreTable = new() {
            { ')', 3 },
            { ']', 57},
            { '}', 1197},
            { '>', 25137 }
        };

        protected override object SolvePart1(string[] lines) {
            int errorScore = 0;

            foreach(string line in lines) {
                Stack<char> chunks = new Stack<char>();
                foreach(char c in line) {
                    if (ClosingBrackets.ContainsKey(c)) {
                        // Starting a new chunk!
                        chunks.Push(c);
                    } else {
                        // We are expecting the correct closing bracket
                        char closingBracket = ClosingBrackets[chunks.Pop()];
                        if(c != closingBracket) {
                            // Corrupted line!
                            errorScore += ScoreTable[c];
                            break;
                        }
                    }
                }
            }

            return errorScore;
        }

        protected override object SolvePart2(string[] lines) {
            return null;
        }

    }
}
