using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_16 {
    public enum PacketType {
        // Literal value packets encode a single binary number.
        // To do this, the binary number is padded with leading zeroes until its length is a multiple of four bits,
        // and then it is broken into groups of four bits.
        // Each group is prefixed by a 1 bit except the last group, which is prefixed by a 0 bit.
        // These groups of five bits immediately follow the packet header.
        LiteralValue = 4 
    }
}
