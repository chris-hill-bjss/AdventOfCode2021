using System.Text.RegularExpressions;

internal sealed class Day17
{
    private readonly string _input;

    public Day17(string input)
    {
        _input = input;
    }

    public int Solve()
    {
        var targetArea = ParseInput(_input);
        var targetAreaMinX = targetArea.MinBy(p => p.x)!.x;
        
        var sumX = 0;
        var minX = Enumerable.Range(0, targetAreaMinX).First(i => (sumX+=i) > targetAreaMinX);
        var maxY = Math.Abs(targetArea.MinBy(t => t.y)!.y) - 1;

        var trajectory = CalculateProbeArc(minX, maxY, targetArea, new Stack<Point>(new[] { new Point(0, 0) } ));
        return trajectory.MaxBy(p => p.y)!.y;
    }

    IEnumerable<(int maxY, Point velocity)> LaunchProbe(
        Point velocity, 
        int maxY,
        IList<Point> targetArea, 
        List<(int maxY, Point velocity)> validVelocities)
    {
        var probePositions = CalculateProbeArc(velocity.x, velocity.y, targetArea, new Stack<Point>(new[] { new Point(0, 0) } ));
        if (probePositions.Any(p => targetArea.Contains(p)))
        {
            validVelocities.Add((probePositions.MaxBy(p => p.y)!.y, new Point(velocity.x, velocity.y)));
        }
        
        if (velocity.y < maxY)
        {
            LaunchProbe(velocity with { y = velocity.y + 1 }, maxY, targetArea, validVelocities);
        }

        return validVelocities;
    }

    IEnumerable<Point> CalculateProbeArc(int vx, int vy, IList<Point> targetArea, Stack<Point> visited)
    {
        var current = visited.Peek();
        
        var position = current with { x = current.x + vx, y = current.y + vy };
        visited.Push(position);
        
        var targetAreaMinX = targetArea.MinBy(p => p.x)!.x;
        var targetAreaMaxY = targetArea.MaxBy(p => p.y)!.y;
        
        if ((vx > 0 && position.x < targetAreaMinX) || position.y > targetAreaMaxY)
        {
            CalculateProbeArc(--vx > 0 ? vx : 0, --vy, targetArea, visited);
        }
        
        return visited;
    }

    IList<Point> ParseInput(string input)
    {
        var matches = Regex.Matches(input, @"-?\d+");

        var minX = Convert.ToInt32(matches[0].Value);
        var maxX = Convert.ToInt32(matches[1].Value);
        var minY = Convert.ToInt32(matches[2].Value);
        var maxY = Convert.ToInt32(matches[3].Value);
        
        var points = from x in Enumerable.Range(minX, (maxX - minX) + 1)
                    from y in Enumerable.Range(minY, (maxY - minY) + 1)
                    select new Point(x, y);

        return new List<Point>(points);
    }

    record Point(int x, int y);
    
    public static string TestInput = "target area: x=20..30, y=-10..-5";
}