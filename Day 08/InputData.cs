using AdventOfCode.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_08 {
    public class InputData : IObjectParser<IEnumerable<string>>, IEnumerable<string> {

        public List<string> Data = new();

        public void Parse(IEnumerable<string> input) {
            Data = input.ToList();
        }

        public IEnumerator<string> GetEnumerator() {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Data.GetEnumerator();
        }
    }
}
