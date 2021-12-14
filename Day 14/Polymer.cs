using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_14 {
    public class Polymer : IObjectParser<string> {

        public LinkedList<char> Elements = new();

        public void Parse(string input) {
            foreach(char c in input) {
                Elements.AddLast(c);
            }
        }
    }

}
