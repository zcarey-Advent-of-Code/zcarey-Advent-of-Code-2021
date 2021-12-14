﻿using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_14 {
    public class InsertionRule : IObjectParser<string[]> {

        public char InsertionElement { get; private set; }
        public string Rule { get; private set; }

        public void Parse(string[] input) {
            if (input[1].Length != 1) throw new ArgumentException("Insertion element must be 1 char");
            InsertionElement = input[1][0];

            if (input[0].Length != 2) throw new ArgumentException("Rule must be 2 chars");
            Rule = input[0];
        }

        public bool Match(LinkedListNode<char> node) {
            if (node.Next == null) return false;

            return node.Value == Rule[0] && node.Next.Value == Rule[1];
        }
    }
}
