using System.Text;

internal sealed class Day16
{
    private readonly string _input;

    public Day16(string input)
    {
        _input = input;
    }

    public int SolvePartOne()
    {
        var packet = MessageToPacket();
        
        var (parentPacket, _) = ReadPacket(packet);

        var accumulator = new List<int>();
        SumPacketVersions(parentPacket, accumulator);

        return accumulator.Sum();
    }

    public long SolvePartTwo()
    {
        var packet = MessageToPacket();
        
        var (parentPacket, _) = ReadPacket(packet);

        return parentPacket.value;
    }

    private void SumPacketVersions(Packet packet, List<int> accumulator)
    {
        accumulator.Add(packet.version);
        
        foreach(var subPacket in packet.subPackets)
        {
            SumPacketVersions(subPacket, accumulator);
        }
    }

    private (Packet packet, int packetLength) ReadPacket(string packet)
    {
        int pos = 0;

        var version = Convert.ToInt32(packet[pos..(pos+=3)], 2);
        var type = Convert.ToInt32(packet[pos..(pos+=3)], 2);

        long value = 0;
        var subPackets = new List<Packet>();

        if (type == 4)
        {
            var sb = new StringBuilder();
            while(true)
            {
                var bits = packet[pos..(pos+=5)];
                sb.Append(bits[1..]);

                if (bits[0] == '0')
                    break;
            }
            value = Convert.ToInt64(sb.ToString(), 2);
        }
        else
        {
            var lengthType = packet[pos..(pos+=1)];
            if (lengthType == "0")
            {
                var subPacketsLength = Convert.ToInt32(packet[pos..(pos+=15)], 2);
                var readUntil = pos + subPacketsLength;

                while(pos < readUntil)
                {
                    var (subPacket, subPacketLength) = ReadPacket(packet[pos..]);
                    subPackets.Add(subPacket);
                    pos+= subPacketLength;
                }
            }
            else
            {
                var subPacketsCount = Convert.ToInt32(packet[pos..(pos+=11)], 2);

                while(subPackets.Count < subPacketsCount)
                {
                    var (subPacket, subPacketLength) = ReadPacket(packet[pos..]);
                    subPackets.Add(subPacket);
                    pos+= subPacketLength;
                }
            }

            value = type switch
            {
                0 => subPackets.Sum(p => p.value),
                1 => subPackets.Select(p => p.value).Aggregate((a, b) => a * b),
                2 => subPackets.MinBy(p => p.value)!.value,
                3 => subPackets.MaxBy(p => p.value)!.value,
                5 => Convert.ToInt32(subPackets.First().value > subPackets.Last().value),
                6 => Convert.ToInt32(subPackets.First().value < subPackets.Last().value),
                7 => Convert.ToInt32(subPackets.First().value == subPackets.Last().value),
                _ => 0
            }; 
        }
        return (new Packet(version, type, subPackets, value), pos);
    }

    record Packet(int version, int type, IList<Packet> subPackets, long value);

    private string MessageToPacket() =>
        string.Join(
            "", 
            Enumerable
                .Range(0, _input.Length)
                .Select(i => Convert.ToString(Convert.ToByte(_input.Substring(i, 1), 16), 2).PadLeft(4, '0')));

    public static string[] PartOneTestInputs = new[] 
    {
        "D2FE28",
        "38006F45291200",
        "EE00D40C823060",
        "8A004A801A8002F478",
        "620080001611562C8802118E34",
        "C0015000016115A2E0802F182340",
        "A0016C880162017C3686B18A3D4780"
    };

    public static string[] PartTwoTestInputs = new[] 
    {
        "C200B40A82",
        "04005AC33890",
        "880086C3E88112",
        "CE00C43D881120",
        "D8005AC2A8F0",
        "F600BC2D8F",
        "9C005AC2F8F0",
        "9C0141080250320F1802104A08"
    };
}