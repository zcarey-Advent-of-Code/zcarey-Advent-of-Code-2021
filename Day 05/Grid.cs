using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rectangle = System.Drawing.Rectangle;

namespace Day_05 {
    public class Grid : IObjectParser<IEnumerable<Line>> {

        public Line[] Lines = new Line[] { };
        public Dictionary<Point, int> Intersections = new();
        public int Left = int.MaxValue;
        public int Right = int.MinValue;
        public int Top = int.MinValue;
        public int Bottom = int.MaxValue;
        public Rectangle Area {
            get {
                return new Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
        }

        public void Parse(IEnumerable<Line> input) {
            this.Lines = input.ToArray();

            foreach (Line line in this.Lines) {
                foreach (Point point in line.GetPoints()) {
                    if (!Intersections.ContainsKey(point)) {
                        Intersections[point] = 0;
                    }
                    Intersections[point]++;
                    Left = Math.Min(Left, point.X);
                    Right = Math.Max(Right, point.X);
                    Top = Math.Min(Top, point.Y);
                    Bottom = Math.Max(Bottom, point.Y);
                }
            }
        }
    }
}
