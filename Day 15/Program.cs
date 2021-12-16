using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day_15 {
    internal class Program : ProgramStructure<int[][]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .ForEach(
                new Parser<string>()
                .ForEach<string, string, char>()
                .Filter(x => x.ToString())
                .Filter(int.Parse)
                .ToArray()
            )
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
            //new Program().Run(args, "Example.txt");
        }

        protected override object SolvePart1(int[][] input) {
            Point target = new Point(input[input.Length - 1].Length - 1, input.Length - 1);
            IEnumerable<Point> path = FindSafestPath(input, new Point(0, 0), target);
            // Skip the start position danger
            int danger = path.Skip(1).Select(p => input[p.Y][p.X]).Sum();
            return danger;
        }

        protected override object SolvePart2(int[][] input) {
            return null;
        }

        private static readonly Size[] Directions = new Size[] { 
            // Always check right then down first to find a path quickly and avoid overflow errors
            new Size(1, 0),
            new Size(0, 1),
            new Size(0, -1),
            new Size(-1, 0)
        };

        private static IEnumerable<Point> FindSafestPath(int[][] input, Point start, Point target) {
            // As always, my favorite website for pathing (dijkstra):
            // https://www.redblobgames.com/pathfinding/a-star/introduction.html#dijkstra
            PriorityQueue<Point, int> frontier = new();
            Dictionary<Point, Point?> cameFrom = new();
            Dictionary<Point, int> costSoFar = new();

            frontier.Enqueue(start, 0);
            cameFrom[start] = null;
            costSoFar[start] = 0;

            while(frontier.Count > 0) {
                Point current = frontier.Dequeue();
                if(current == target) {
                    break;
                }

                foreach(Size direction in Directions) {
                    Point next = current + direction;
                    if(next.X < 0 || next.Y < 0 || next.Y >= input.Length || next.X >= input[next.Y].Length) {
                        // Invalid position
                        continue;
                    }

                    int newCost = costSoFar[current] + input[next.Y][next.X];
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                        costSoFar[next] = newCost;
                        int priority = newCost; // Semantics, am I right?
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            List<Point> reversePath = new List<Point>();
            reversePath.Add(target);
            Point trace = target;
            Point? nextTrace;
            while (cameFrom.TryGetValue(trace, out nextTrace) && nextTrace != null) {
                trace = (Point)nextTrace;
                reversePath.Add(trace);
            }
            if (trace != start) throw new Exception("Failed to trace the path.");

            // Flip the list and return the path
            for(int i = reversePath.Count - 1; i >= 0; i--) {
                yield return reversePath[i];
            }
        }


    }
}
