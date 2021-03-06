internal sealed class Day15
{
    private readonly Dictionary<Node, int> _nodeCostMap;
    public Day15(int[][] input)
    {
        _nodeCostMap = new Dictionary<Node, int>(
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[0].Length)
            select new KeyValuePair<Node, int>(new Node(y, x), input[y][x]));
    }
    public int Solve()
    {
        var start = new Node(0, 0);
        var end = new Node(_nodeCostMap.Keys.MaxBy(p => p.Y)!.Y, _nodeCostMap.Keys.MaxBy(p => p.X)!.X);

        var options = new PriorityQueue<Node, int>();
        var visited = new Dictionary<Node, int>();

        visited[start] = 0;
        options.Enqueue(start, 0);

        while(options.Count > 0)
        {
            var current = options.Dequeue();

            foreach(var adjacentNode in GetAdjacentNodes(current))
            {
                if (_nodeCostMap.ContainsKey(adjacentNode) && !visited.ContainsKey(adjacentNode))
                {
                    var cost = _nodeCostMap[adjacentNode] + visited[current];
                    visited.Add(adjacentNode, cost);
                    options.Enqueue(adjacentNode, cost);
                }
            }
        }

        return visited[end];
    }

    public int[][] GrowArray(int[][] input)
    {
        var largeInput = new int[input.Length * 5][];

        for (int y = 0; y < largeInput.Length; y++)
        {
            largeInput[y] = new int[input[0].Length * 5];
            for (int x = 0; x < largeInput[0].Length; x++)
            {
                var oldY = y % input.Length;
                var oldX = x % input[0].Length;
                var oldVal = input[oldY][oldX];

                var modY = y / input.Length;
                var modX = x / input[0].Length;

                var newVal = oldVal + modY + modX;
                largeInput[y][x] = newVal > 9 ? newVal - 9 : newVal;
            }
        }

        return largeInput;
    }

    private List<Node> GetAdjacentNodes(Node node)
    {
        return new List<Node>
        {
            node with { Y = node.Y - 1},
            node with { Y = node.Y + 1},
            node with { X = node.X - 1},
            node with { X = node.X + 1}
        };
    }

    record Node(int Y, int X);

    public static int[][] TestInput = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581"
    .Split("\n", StringSplitOptions.RemoveEmptyEntries)
    .Select(row => row.Where(c => Char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToArray())
    .ToArray();
}