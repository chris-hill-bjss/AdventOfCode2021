internal sealed class Day16
{
    private readonly string _input;

    public Day16(string input)
    {
        _input = input;
    }

    public int Solve()
    {
        var packet = MessageToPacket();
        
        var (parentPacket, _) = ReadPacket(packet);

        var accumulator = new List<int>();
        SumPacketVersions(parentPacket, accumulator);

        return accumulator.Sum();
    }

    private int SumPacketVersions(Packet packet, List<int> accumulator)
    {
        accumulator.Add(packet.version);
        
        foreach(var subPacket in packet.subPackets)
        {
            SumPacketVersions(subPacket, accumulator);
        }

        return 0;
    }

    private (Packet packet, int packetLength) ReadPacket(string packet)
    {
        int pos = 0;

        var version = Convert.ToInt32(packet[pos..(pos+=3)], 2);
        var type = Convert.ToInt32(packet[pos..(pos+=3)], 2);

        var numbers = new List<string>();
        var subPackets = new List<Packet>();

        if (type == 4)
        {
            while(true)
            {
                var bits = packet[pos..(pos+=5)];
                numbers.Add(bits[1..]);

                if (bits[0] == '0')
                    break;
            }
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
            
        }

        return (new Packet(version, type, subPackets, numbers), pos);
    }

    record Packet(int version, int type, IList<Packet> subPackets, List<string> number);

    private string MessageToPacket() =>
        string.Join(
            "", 
            Enumerable
                .Range(0, _input.Length)
                .Select(i => Convert.ToString(Convert.ToByte(_input.Substring(i, 1), 16), 2).PadLeft(4, '0')));

    public static string[] TestInputs = new[] 
    {
        "D2FE28",
        "38006F45291200",
        "EE00D40C823060",
        "8A004A801A8002F478",
        "620080001611562C8802118E34",
        "C0015000016115A2E0802F182340",
        "A0016C880162017C3686B18A3D4780"
    };
}