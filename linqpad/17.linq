<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

void Main()
{
	Solve(GetInput()).Dump();
}

long Solve(string input)
{
	var targetArea = ParseInput(input);
	var targetAreaMinX = targetArea.MinBy(p => p.x).x;
	
	var sum = 0;
	var minX = Enumerable.Range(0, targetAreaMinX / 2).First(i => (sum+=i) > targetAreaMinX);

	var validVelocities =
		LaunchProbe(new Point(0, 0), targetArea, new List<(int maxY, Point velocity)>());
	
	return validVelocities.MaxBy(v => v.maxY).maxY;
}

IEnumerable<(int maxY, Point velocity)> LaunchProbe(
	Point velocity, 
	IList<Point> targetArea, 
	List<(int maxY, Point velocity)> validVelocities)
{
	var probePositions = CalculateProbeArc(velocity.x, velocity.y, targetArea, new Stack<Point>(new[] { new Point(0, 0) } ));
	if (probePositions.Any(p => targetArea.Contains(p)))
	{
		validVelocities.Add((probePositions.MaxBy(p => p.y).y, new Point(velocity.x, velocity.y)));
		LaunchProbe(velocity with { y = velocity.y + 1 }, targetArea, validVelocities);
	}
	
	else if (velocity.x < targetArea.MaxBy(p => p.x).x)
	{
		LaunchProbe(velocity with { x = velocity.x + 1, y = 0 }, targetArea, validVelocities);
	}
	
	return validVelocities;
}

IEnumerable<Point> CalculateProbeArc(int vx, int vy, IList<Point> targetArea, Stack<Point> visited)
{
	var current = visited.Peek();
	
	var position = current with { x = current.x + vx, y = current.y + vy };
	visited.Push(position);
	
	var targetAreaMinX = targetArea.MinBy(p => p.x).x;
	var targetAreaMaxY = targetArea.MaxBy(p => p.y).y;
	
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

string GetInput()
{
	return "target area: x=70..96, y=-179..-124";
}

public string TestInput = @"target area: x=20..30, y=-10..-5";