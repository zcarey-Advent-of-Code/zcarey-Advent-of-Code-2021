using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_08 {
    public class WireLinks {

        public static Dictionary<int, int[]> PossibleNumbersFromLength = new() {
            { 2, new int[] { 1 } },
            { 3, new int[] { 7 } },
            { 4, new int[] { 4 } },
            { 5, new int[] { 2, 3, 5 } },
            { 6, new int[] { 0, 6, 9 } },
            { 7, new int[] { 8 } }
        };

        private Dictionary<char, char> Linker = new();

        private WireLinks() {

        }

        public Wires Unscramble(Wires wires) {
            StringBuilder sb = new StringBuilder();
            foreach(char wire in wires) {
                sb.Append(Linker[wire]);
            }
            return new Wires(sb.ToString());
        }

        public static WireLinks Solve(Wires[] input) {
            if (input.Length != 10) throw new ArgumentException("Length of input must be 10.", nameof(input));

            WireLinks links = new();

            Dictionary<char, List<char>> possibleLinks = new() {
                { 'a', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'b', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'c', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'd', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'e', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'f', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
                { 'g', new() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } }
            };

            // First, search for the 4 easy numbers (8 does not help at all though, stupid 8)
            Dictionary<int, Wires> knownNumbers = new();
            foreach(Wires wires in input) {
                if (PossibleNumbersFromLength[wires.Count].Length == 1) {
                    int digit = PossibleNumbersFromLength[wires.Count][0];
                    knownNumbers[digit] = wires;
                    foreach(char c in wires) {
                        IsThisDigit(possibleLinks[c], digit);
                    }
                }
            }
            if (knownNumbers.Count != 4) 
                throw new Exception("Unable to find all the easy numbers");

            // Seven is special, we can identify it because it shares all but 1 segment with the digit 1
            foreach(char c in knownNumbers[7]) {
                if (!knownNumbers[1].Contains(c)) {
                    CompleteLink(possibleLinks, c, 'a');
                    possibleLinks.Remove(c);
                    links.Linker[c] = 'a';
                    break;
                }
            }


            // Find wire pairs
            List<KeyValuePair<char, List<char>>> pairCF = new();
            List<KeyValuePair<char, List<char>>> pairBD = new();
            List<KeyValuePair<char, List<char>>> pairEG = new();

            bool pairFound = true;
            while (pairFound) {
                pairFound = false;

                List<char> removeLinks = new();
                foreach (var pair in possibleLinks) {
                    if (pair.Value.Count == 2) {
                        if (pair.Value[0] == 'c' || pair.Value[0] == 'f') {
                            pairCF.Add(pair);
                            removeLinks.Add(pair.Key);
                            pairFound = true;
                        } else if (pair.Value[0] == 'b' || pair.Value[0] == 'd') {
                            pairBD.Add(pair);
                            removeLinks.Add(pair.Key);
                            pairFound = true;
                        } else if (pair.Value[0] == 'e' || pair.Value[0] == 'g') {
                            pairEG.Add(pair);
                            removeLinks.Add(pair.Key);
                            pairFound = true;
                        }
                    }
                }
                foreach (char key in removeLinks) {
                    possibleLinks.Remove(key);
                }

                // remove characters for known links
                foreach (var pair in possibleLinks) {
                    if (pairCF.Count == 2) {
                        pair.Value.Remove('c');
                        pair.Value.Remove('f');
                    }
                    if (pairBD.Count == 2) {
                        pair.Value.Remove('b');
                        pair.Value.Remove('d');
                    }
                    if (pairEG.Count == 2) {
                        pair.Value.Remove('e');
                        pair.Value.Remove('g');
                    }
                }
            }
            if (pairCF.Count != 2 || pairBD.Count != 2 || pairEG.Count != 2) throw new Exception("Could not find all wire pairs.");

            // TODO possibleLinks no longer needed past here

            // Find sigits 0, 6, and 9
            Wires[] digitSet069 = input.Where(x => x.Count == 6).ToArray();

            // For pair BD, the wire that's missing from this group corresponds to output 'd'
            List<char> possibleBD = new() { pairBD[0].Key, pairBD[1].Key };
            bool foundBD = false;
            foreach(Wires wire in digitSet069) {
                if (!wire.Contains(possibleBD[0])){
                    links.Linker[possibleBD[0]] = 'd';
                    links.Linker[possibleBD[1]] = 'b';
                    foundBD = true;
                    break;
                }else if (!wire.Contains(possibleBD[1])) {
                    links.Linker[possibleBD[1]] = 'd';
                    links.Linker[possibleBD[0]] = 'b';
                    foundBD = true;
                    break;
                }
            }
            if (foundBD == false) throw new Exception("Could not find BD");


            // Same thing, but for the EG pair corresponding to 'e'
            List<char> possibleEG = new() { pairEG[0].Key, pairEG[1].Key };
            bool foundEG = false;
            foreach (Wires wire in digitSet069) {
                if (!wire.Contains(possibleEG[0])) {
                    links.Linker[possibleEG[0]] = 'e';
                    links.Linker[possibleEG[1]] = 'g';
                    foundEG = true;
                    break;
                } else if (!wire.Contains(possibleEG[1])) {
                    links.Linker[possibleEG[1]] = 'e';
                    links.Linker[possibleEG[0]] = 'g';
                    foundEG = true;
                    break;
                }
            }
            if (foundEG == false) throw new Exception("Could not find EG");


            // Same thing, but for the CF pair corresponding to 'c'
            List<char> possibleCF = new() { pairCF[0].Key, pairCF[1].Key };
            bool foundCF = false;
            foreach (Wires wire in digitSet069) {
                if (!wire.Contains(possibleCF[0])) {
                    links.Linker[possibleCF[0]] = 'c';
                    links.Linker[possibleCF[1]] = 'f';
                    foundCF = true;
                    break;
                } else if (!wire.Contains(possibleCF[1])) {
                    links.Linker[possibleCF[1]] = 'c';
                    links.Linker[possibleCF[0]] = 'f';
                    foundCF = true;
                    break;
                }
            }
            if (foundCF == false) throw new Exception("Could not find CF");

            // One final sanity check
            for(char c = 'a'; c <= 'g'; c++) {
                if (!links.Linker.ContainsKey(c)) {
                    throw new Exception("Bad link");
                }
            }

            return links;
        }

        private static bool PairMatch(List<char> pair1, List<char> pair2) {
            if(pair1[0] == pair2[0]) {
                return pair1[1] == pair2[1];
            } else {
                return pair1[0] == pair2[1] && pair1[1] == pair2[0];
            }
        }

        // if we know that wire 'a' is a part of the number '1', remove any other connections that aren't apart of '1'
        private static void IsThisDigit(List<char> possibilities, int digit) {
            foreach(char c in Segment.InverseSegments[digit]) {
                possibilities.Remove(c);
            }
        }

        // Yay, you found that "input" matched to "output"!
        // Now we can remove that wire from all the other possible links
        private static void CompleteLink(Dictionary<char, List<char>> links, char input, char output) {
            links[input] = new() { output };
            foreach (var pair in links) {
                if (pair.Key != input) {
                    pair.Value.Remove(output);
                }
            }
        }

    }
}
