using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_21 {
    internal class Program : ProgramStructure<int[]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Filter(x => x.Substring("Player 1 starting position: ".Length))
            .Filter(int.Parse)
            .Filter(x => x - 1) // Internally is 0-9 while the actual board is 1-10
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(int[] input) {
            int pos1 = input[0];
            int score1 = 0;

            int pos2 = input[1];
            int score2 = 0;

            Dice dice = new DeterministicDice();
            return PlayGame(dice, ref pos1, ref score1, ref pos2, ref score2) * dice.Rolls;
        }

        protected override object SolvePart2(int[] input) {
            return null;
        }

        // Returns the loser's score
        private static int PlayGame(Dice dice, ref int pos1, ref int score1, ref int pos2, ref int score2) {
            while (Simulate(dice, ref pos1, ref score1, ref pos2, ref score2)) {
            }

            if(score1 >= 1000) {
                return score2;
            } else {
                return score1;
            }
        }

        // Returns true of the game should continue.
        private static bool Simulate(Dice dice, ref int pos1, ref int score1, ref int pos2, ref int score2) {
            // On each player's turn, the player rolls the die three times and adds up the results.
            int move1 = dice.Roll() + dice.Roll() + dice.Roll();
            // Then, the player moves their pawn that many times forward around the track
            pos1 = (pos1 + move1) % 10;
            // After each player moves, they increase their score by the value of the space their pawn stopped on.
            // NOTE: our board is 0-9, so we add one to get the actual board's 1-10
            score1 += (pos1 + 1);
            if (score1 >= 1000) return false;

            // Do the same for player 2
            int move2 = dice.Roll() + dice.Roll() + dice.Roll();
            pos2 = (pos2 + move2) % 10;
            score2 += (pos2 + 1);
            if (score2 >= 1000) return false;

            return true;
        }

    }
}
