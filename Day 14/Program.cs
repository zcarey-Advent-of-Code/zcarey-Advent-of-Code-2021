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

            Dictionary<char, int> elementCount = new();
            foreach(PolymerElement element in input.Item1.Elements) {
                if (!elementCount.ContainsKey(element.Element)) {
                    elementCount[element.Element] = 1;
                } else {
                    elementCount[element.Element]++;
                }
            }

            // Find the most and least common element
            int mostCommonCount = int.MinValue;
            int leastCommonCount = int.MaxValue;
            foreach(var pair in elementCount) {
                mostCommonCount = Math.Max(mostCommonCount, pair.Value);
                leastCommonCount = Math.Min(leastCommonCount, pair.Value);
            }

            return mostCommonCount - leastCommonCount;
        }

        protected override object SolvePart2(Tuple<Polymer, InsertionRule[]> input) {
            return null;
        }

        private void ApplyStep(Polymer polymer, InsertionRule[] rules) {
            LinkedListNode<PolymerElement> node;
            for(node = polymer.Elements.First; node.Next != null; node = node.Next) {
                foreach(InsertionRule rule in rules) {
                    if (rule.Match(node)) {
                        node.Value.AddBuffer.Add(rule.InsertionElement);
                    }
                }
            }

            // Now actually add the inserted elements into the list
            for(node = polymer.Elements.First; node.Next != null; node = node.Next) {
                foreach(char insertion in node.Value.AddBuffer) {
                    polymer.Elements.AddAfter(node, new PolymerElement(insertion));
                }
                node.Value.AddBuffer.Clear();
            }
        }

    }
}
