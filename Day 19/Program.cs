using AdventOfCode;
using AdventOfCode.Parsing;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day_19 {
    internal class Program : ProgramStructure<Scanner[]> {

        Program() : base(new Parser()
            .Filter(new LineReader())
            .Filter(new TextBlockFilter())
            .ForEach(
                // Parse each scanner
                new Parser<string[]>()
                .Filter(x => x.Skip(1)) // Skip the "ID" line
                .Filter(Point.Parse)
                .Create<Scanner>()
            )
            .ToArray()
        ) { }

        static void Main(string[] args) {
            new Program().Run(args);
        }

        protected override object SolvePart1(Scanner[] input) {
            return LocateScanners(input)
                .SelectMany(scanner => scanner.GetGlobalBeacons())
                .Distinct()
                .Count();
        }

        protected override object SolvePart2(Scanner[] input) {
            HashSet<Scanner> scanners = LocateScanners(input);
            return scanners.Zip(scanners)
                .Where(x => x.First != x.Second)
                .Select(x => (x.First.Location - x.Second.Location).Abs)
                .Max();
                
            /*return (
                from sA in scanners
                from sB in scanners
                where sA != sB
                select (sA.Location - sB.Location).Abs.Sum()
            ).Max();*/
        }

        HashSet<Scanner> LocateScanners(Scanner[] input) {
            var scanners = new HashSet<Scanner>(input);
            var locatedScanners = new HashSet<Scanner>();
            var q = new Queue<Scanner>();

            // when a scanner is located, it gets into the queue so that we can
            // explore its neighbours.

            locatedScanners.Add(scanners.First());
            q.Enqueue(scanners.First());

            scanners.Remove(scanners.First());

            while (q.Any()) {
                var scannerA = q.Dequeue();
                foreach (var scannerB in scanners.ToArray()) {
                    var maybeLocatedScanner = TryToLocate(scannerA, scannerB);
                    if (maybeLocatedScanner != null) {

                        locatedScanners.Add(maybeLocatedScanner);
                        q.Enqueue(maybeLocatedScanner);

                        scanners.Remove(scannerB); // sic! 
                    }
                }
            }

            return locatedScanners;
        }
        Scanner TryToLocate(Scanner scannerA, Scanner scannerB) {
            var beaconsInA = scannerA.GetGlobalBeacons().ToArray();

            foreach (var (beaconInA, beaconInB) in PotentialMatchingBeacons(scannerA, scannerB)) {
                // now try to find the orientation for B:
                var rotatedB = scannerB;
                for (var rotation = 0; rotation < 24; rotation++, rotatedB = rotatedB.Rotate()) {
                    // Moving the rotated scanner so that beaconA and beaconB overlaps. Are there 12 matches? 
                    var beaconInRotatedB = beaconInB.Transform(rotatedB.Location, rotation);

                    var locatedB = rotatedB.Translate(beaconInA - beaconInRotatedB);

                    if (locatedB.GetGlobalBeacons().Intersect(beaconsInA).Count() >= 12) {
                        return locatedB;
                    }
                }
            }

            // no luck
            return null;
        }

        IEnumerable<(Point beaconInA, Point beaconInB)> PotentialMatchingBeacons(Scanner scannerA, Scanner scannerB) {
            // If we had a matching beaconInA and beaconInB and moved the center
            // of the scanners to these then we would find at least 12 beacons 
            // with the same coordinates.

            // The only problem is that the rotation of scannerB is not fixed yet.

            // We need to make our check invariant to that:

            // After the translation, we could form a set from each scanner 
            // taking the absolute values of the x y and z coordinates of their beacons 
            // and compare those. 

            IEnumerable<int> absCoordinates(Scanner scanner) => scanner.GetGlobalBeacons().SelectMany(x => x.Abs);

            // This is the same no matter how we rotate scannerB, so the two sets should 
            // have at least 3 * 12 common values (with multiplicity).

            // 🐦 We can also considerably speed up the search with the pigeonhole principle 
            // which says that it's enough to take all but 11 beacons from A and B. 
            // If there is no match amongst those, there cannot be 12 matching pairs:
            IEnumerable<T> pick<T>(IEnumerable<T> ts) => ts.Take(ts.Count() - 11);

            foreach (var beaconInA in pick(scannerA.GetGlobalBeacons())) {
                var absA = absCoordinates(
                    scannerA.Translate(-beaconInA)
                ).ToHashSet();

                foreach (var beaconInB in pick(scannerB.GetGlobalBeacons())) {
                    var absB = absCoordinates(
                        scannerB.Translate(-beaconInB)
                    );

                    if (absB.Count(d => absA.Contains(d)) >= 3 * 12) {
                        yield return (beaconInA, beaconInB);
                    }
                }
            }
        }

    }
 
}
