<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	SolvePartOne(await GetInput()).Dump();
	SolvePartTwo(await GetInput()).Dump();
}

int SolvePartOne(int[][] input)
{
	return GenerateFloorMapFromInput(input)
		.Where(point => point.isLowPoint)
		.Sum(point => point.positionHeight + 1);
}

int SolvePartTwo(int[][] input)
{
	return GenerateFloorMapFromInput(input)
		.Where(point => point.isLowPoint)
		.Select(lowPoint =>
		{
			var seen = new List<(int y, int x)>();
			
			var basin = MapBasin(lowPoint.pos.y, lowPoint.pos.x, input, seen);
			return basin.Count;
		})
		.OrderByDescending(basinSize => basinSize)
		.Take(3)
		.Aggregate((a, b) => a * b);
}

List<(int y, int x)> MapBasin(int y, int x, int[][] input, List<(int y, int x)> seen)
{
	seen.Add((y, x));
	var adjacentPoints =
		GetAdjacentPoints(y, x, input)
			.Where(adjacent => new[] { 9, -1 }.Contains(adjacent.height) == false)
			.ToArray();

	foreach (var adjacentPoint in adjacentPoints)
	{	
		if (seen.Contains((adjacentPoint.y, adjacentPoint.x))) continue;
		
		MapBasin(adjacentPoint.y, adjacentPoint.x, input, seen);
	}
	
	return seen;
}

(int y, int x, int height)[] GetAdjacentPoints(int y, int x, int[][] input)
{
	return new (int y, int x, int height)[]
		{
			y > 0 ? (y - 1, x, input[y - 1][x]) : (-1, -1, -1),
			y < input.Length - 1 ? (y+1, x, input[y + 1][x]) : (-1, -1, -1),
			x > 0 ? (y, x - 1, input[y][x - 1]) : (-1, -1, -1),
			x < input[y].Length - 1 ? (y, x + 1, input[y][x + 1]) : (-1, -1, -1),
		};
}

IEnumerable<((int y, int x) pos, bool isLowPoint, int positionHeight)> GenerateFloorMapFromInput(int[][] input)
{
	for (int y = 0; y < input.Length; y++)
	{
		for (int x = 0; x < input[y].Length; x++)
		{
			int positionHeight = input[y][x];

			bool isLowPoint =
				GetAdjacentPoints(y, x, input)
				.Where(adjacent => adjacent.height != -1)
				.All(adjacent => adjacent.height > positionHeight);

			yield return (pos: (y, x), isLowPoint, positionHeight);
		}
	}
}

async Task<int[][]> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/9/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Select(c => (int)Char.GetNumericValue(c)).Where(c => c >= 0).ToArray()).ToArray();
}

public int[][] TestInput = @"2199943210
3987894921
9856789892
8767896789
9899965678".Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Select(c => (int)Char.GetNumericValue(c)).Where(c => c >= 0).ToArray()).ToArray();