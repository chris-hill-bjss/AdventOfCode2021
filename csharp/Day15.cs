internal sealed class Day15
{
    private readonly int[][] _input;
    private readonly NodeFactory _factory;
    public Day15(int[][] input)
    {
        _input = input;
        _factory = new NodeFactory(_input);
    }
    public int Solve()
    {
        var start = _factory.CreateNode((0, 0));
        var end = _factory.CreateNode((_input.Length - 1, _input[0].Length - 1));

        var options = new PriorityQueue<Node, int>();
        var visited = new Dictionary<(int y, int x), Node>();

        options.Enqueue(start, 0);

        var current = start;
        while(options.Count > 0)
        {
            current = options.Dequeue();

            if (!visited.TryAdd(current.Position, current))
                continue;

            foreach(var adjacentNode in GetAdjacentNodes(current, visited, _input.Length, _input[0].Length))
            {
                options.Enqueue(adjacentNode, adjacentNode.Rank);
            }
        }

        return visited[end.Position].Cost;
    }

    private List<Node> GetAdjacentNodes(Node node, Dictionary<(int y, int x), Node> visited, int maxY, int maxX)
    {
        var adjacentNodes = new List<Node>();

        if (node.Position.y - 1 >= 0)
        {
            var adjacent = _factory.CreateNode(node, (node.Position.y - 1, node.Position.x));
            if (!visited.ContainsKey(adjacent.Position)) 
                adjacentNodes.Add(adjacent);
        }
        if (node.Position.y + 1 < maxY)
        {
            var adjacent = _factory.CreateNode(node, (node.Position.y + 1, node.Position.x));
            if (!visited.ContainsKey(adjacent.Position)) 
                adjacentNodes.Add(adjacent);
        }
        if (node.Position.x - 1 >= 0)
        {
            var adjacent = _factory.CreateNode(node, (node.Position.y, node.Position.x - 1));
            if (!visited.ContainsKey(adjacent.Position)) 
                adjacentNodes.Add(adjacent);
        }
        if (node.Position.x + 1 < maxX)
        {
            var adjacent = _factory.CreateNode(node, (node.Position.y, node.Position.x + 1));
            if (!visited.ContainsKey(adjacent.Position)) 
                adjacentNodes.Add(adjacent);
        }

        return adjacentNodes;
    }

    class NodeFactory
    {
        private readonly int[][] _grid;
        private readonly (int y, int x) _targetPosition;
        public NodeFactory(int[][] grid)
        {
            _grid = grid;
            _targetPosition = (grid.Length, _grid[0].Length);
        }

        public Node CreateNode((int y, int x) position)
        {
            return CreateNode(null, position);
        }

        public Node CreateNode(Node? parent, (int y, int x) position)
        {
            var distanceToTarget = Math.Abs(position.y - _targetPosition.y) + Math.Abs(position.x - _targetPosition.x);
            var cost = _grid[position.y][position.x] + parent?.Cost ?? 0;
            
            return new Node(
                parent,
                position,
                distanceToTarget,
                cost
            );
        }
    }

    record Node(Node? Parent, (int y, int x) Position, int DistanceToTarget, int Cost)
    {
        public int Rank => DistanceToTarget + Cost;
    }

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