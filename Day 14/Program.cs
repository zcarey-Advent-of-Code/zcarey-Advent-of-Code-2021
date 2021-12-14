using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_14 {
    internal class Program : ProgramStructure<Tuple<Polymer, InsertionRule[]>> {

        Program() : base(new Parser()
            .Parse(new LineReader())
            .Accumulate(
                // Parse the first line
                new Parser<IEnumerable<string>>()
                .Parse(x => x.First())
                .Create<Polymer>()
                ,
                // Parse the rules
                new Parser<IEnumerable<string>>()
                .Filter(x => x.Skip(2)) // Skip the first 2 lines to get to the input
                .ForEach(
                    // Split the string into 2 strings for each rule
                    new Parser<string>()
                    .Parse(new SeparatedParser("->"))
                    .ForEach(x => x.Trim())
                    .ToArray()
                )
                .FilterCreate<InsertionRule>()
                .ToArray()
            )
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(Tuple<Polymer, InsertionRule[]> input) {
            for(int i = 0; i < 10; i++) {
                ApplyStep(input.Item1, input.Item2);
            }

            var ElementCount = CountElements(input.Item1);

            return ElementCount.MostCommon - ElementCount.LeastCommon;
        }

        protected override object SolvePart2(Tuple<Polymer, InsertionRule[]> input) {
            for (int i = 0; i < 40; i++) {
                ApplyStep(input.Item1, input.Item2);
                Console.WriteLine("Step {0} completed.", i); // Just so I know it's still running T_T
            }

            var ElementCount = CountElements(input.Item1);

            return ElementCount.MostCommon - ElementCount.LeastCommon;
        }

        private void ApplyStep(Polymer polymer, InsertionRule[] rules) {
            LinkedListNode<char> node;
            for(node = polymer.Elements.First; node.Next != null; node = node.Next) {
                foreach(InsertionRule rule in rules) {
                    if (rule.Match(node)) {
                        // Add the node between the two elements, then move the iteration to that node
                        // so the for loop will automatically move us past it
                        node = polymer.Elements.AddAfter(node, rule.InsertionElement);
                        break;
                    }
                }
            }
        }

        // Returns the count of the least common element followed by the count of the most common element.
        private (long LeastCommon, long MostCommon) CountElements(Polymer input) {

            // Count the number of each element
            Dictionary<char, int> elementCount = new();
            foreach (char element in input.Elements) {
                if (!elementCount.ContainsKey(element)) {
                    elementCount[element] = 1;
                } else {
                    elementCount[element]++;
                }
            }

            // Find the most and least common element
            int mostCommonCount = int.MinValue;
            int leastCommonCount = int.MaxValue;
            foreach (var pair in elementCount) {
                mostCommonCount = Math.Max(mostCommonCount, pair.Value);
                leastCommonCount = Math.Min(leastCommonCount, pair.Value);
            }

            return (leastCommonCount, mostCommonCount);
        }

    }
}
