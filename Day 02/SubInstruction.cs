using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_02 {
    public enum SubDirection {
        forward,
        down,
        up
    }

    public struct SubInstruction : IObjectParser<string[]> {
        public SubDirection Direction;
        public int Units;

        public int dx {
            get {
                if(Direction == SubDirection.forward) {
                    return Units;
                } else {
                    return 0;
                }
            }
        }

        public int dy {
            get {
                if (Direction == SubDirection.up) {
                    return -Units;
                } else if (Direction == SubDirection.down) {
                    return Units;
                } else {
                    return 0;
                }
            }
        }

        public SubInstruction(SubDirection direction, int units) {
            Direction = direction;
            Units = units;
        }

        public void Parse(string[] input) {
            Direction = Enum.Parse<SubDirection>(input[0]);
            Units = int.Parse(input[1]);
        }

        public static Point operator +(Point pos, SubInstruction instruction) {
            return new Point(
                pos.X + instruction.dx,
                pos.Y + instruction.dy
            );
        }

        public static Point operator -(Point pos, SubInstruction instruction) {
            return new Point(
                pos.X - instruction.dx,
                pos.Y - instruction.dy
            );
        }
    }
}
