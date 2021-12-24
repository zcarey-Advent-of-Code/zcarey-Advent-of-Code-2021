using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_19 {
    public class Beacon {

        public Point Global { get; }

        private Beacon(Point global) {
            this.Global = global;
        }


        public static Dictionary<Point, Beacon> BeaconLocations = new(); 
        public static IEnumerable<Beacon> AllBeacons { get => BeaconLocations.Values; }

        public static Beacon FindOrCreateBeacon(Point global) {
            Beacon beacon;
            if (BeaconLocations.TryGetValue(global, out beacon)){
                return beacon;
            } else {
                beacon = new(global);
                BeaconLocations[global] = beacon;
                return beacon;
            }
        }

    }
}
