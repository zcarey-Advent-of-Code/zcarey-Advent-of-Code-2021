using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_21 {
    public abstract class Dice {

        public int Rolls { get; private set; } = 0;

        public int Roll() {
            Rolls++;
            return GetNextRoll();
        }

        protected abstract int GetNextRoll();

    }

    public class DeterministicDice : Dice {

        private int current = -1;

        protected override int GetNextRoll() {
            current = (current + 1) % 100;
            return current + 1; // The variable goes from 0-99, but the dice rolls from 1-100
        }
    }
}
