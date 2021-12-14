<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

async Task Main()
{
	SolvePartOne(await GetInput()).Dump();
	SolvePartTwo(await GetInput());
}

int SolvePartOne(string[] input)
{
	var (points, instructions) = ExtractPointsAndInstructions(input);
	
	return FoldPoints(points, instructions[0]).Count();
}

void SolvePartTwo(string[] input)
{
	var (points, instructions) = ExtractPointsAndInstructions(input);

	var foldedPoints = points;
	foreach(var instruction in instructions)
	{
		foldedPoints = FoldPoints(foldedPoints, instruction);
	}
	
	MapPointsToGrid(foldedPoints).Dump();
}

(Point[] points, (string axis, int pos)[]) ExtractPointsAndInstructions(string[] input)
{
	var points = input[0]
		.Split("\n", StringSplitOptions.RemoveEmptyEntries)
		.Select(coords => new Point(Convert.ToInt32(coords.Split(',')[0]), Convert.ToInt32(coords.Split(',')[1])))
		.ToArray();

	var instructions = input[1]
		.Split("\n", StringSplitOptions.RemoveEmptyEntries)
		.Select(instruction =>
		{
			var matches = Regex.Matches(instruction, @"\s(?<axis>\w)=(?<position>\d+)");

			return (axis: matches[0].Groups["axis"].Value, pos: Convert.ToInt32(matches[0].Groups["position"].Value));
		})
		.ToArray();

	return (points, instructions);
}

Point[] FoldPoints(Point[] points, (string axis, int pos) instruction)
{
	var fold = (int value, int pos) => (value > pos) ? value = pos - (value - pos) : value;

	return 
		points
			.Select(point => 
			{
				if (instruction.axis == "x")
				{
					var newX = fold(point.X, instruction.pos);
						
					return new Point(newX, point.Y);
				}

				var newY = fold(point.Y, instruction.pos);
						
				return new Point(point.X, newY);
			})
		.Distinct()
		.ToArray();
}

char[,] MapPointsToGrid(Point[] points)
{
	var grid = new char[points.Max(p => p.Y) + 1, points.Max(p => p.X) + 1];
	
	foreach(var point in points)
	{
		grid[point.Y, point.X] = '#';
	}
	
	return grid;
}

async Task<string[]> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/13/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
}

public string[] TestInput = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5".Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);