using AdventOfCode.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_16 {
    public class Packet : IObjectParser<BitBlitz> {

        public int Version { get; private set; }
        public PacketType Type { get; private set; }
        public BinaryLiteral LiteralValue { get; private set; } = null;
        public List<Packet> SubPackets { get; private set; } = new();


        public Packet() { }


        public void Parse(BitBlitz bits) {
            ParsePacket(bits);
            // The hexadecimal representation of this packet might encode a few extra 0 bits at the end; these are not part of the transmission and should be ignored.
        }

        // Returns the number of read bits
        private int ParsePacket(BitBlitz bits) {
            int bitsRead = 0;

            // Every packet begins with a standard header:
            this.Version = bits.Take(3); // the first three bits encode the packet version,
            this.Type = (PacketType)bits.Take(3); // and the next three bits encode the packet type ID.
            bitsRead += 6;

            if (this.Type == PacketType.LiteralValue) {
                int valueBitsRead;
                LiteralValue = BinaryLiteral.Parse(bits, out valueBitsRead);
                bitsRead += valueBitsRead;
            } else {
                // Operator packet
                bool lengthType = bits.GetBool();
                bitsRead++;
                if (lengthType) {
                    // If the length type ID is 1, then the next 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
                    int numberOfPackets = bits.Take(11);
                    bitsRead += 11;
                    for(int i = 0; i < numberOfPackets; i++) {
                        Packet subPacket = new();
                        bitsRead += subPacket.ParsePacket(bits);
                        SubPackets.Add(subPacket);
                    }
                } else {
                    // If the length type ID is 0, then the next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
                    int bitsToRead = bits.Take(15);
                    bitsRead += 15;
                    while(bitsToRead > 0) {
                        Packet subPacket = new();
                        int packetBits = subPacket.ParsePacket(bits);
                        SubPackets.Add(subPacket);
                        bitsRead += packetBits;
                        bitsToRead -= packetBits;
                    }
                }

            }

            return bitsRead;
        }
    }

}
