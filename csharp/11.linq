<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	SolvePartOne(await GetInput(), 100).Dump();
	SolvePartTwo(await GetInput()).Dump();
}

int SolvePartOne(int[][] input, int days)
{
	return Enumerable.Range(1, days).Sum(_ => SimulateStep(input));
}

int SolvePartTwo(int[][] input)
{
	int day = 0;
	
	int octopiCount = input.Length * input[0].Length;
	
	bool allFlashed = false;
	while(!allFlashed)
	{
		day++;
		allFlashed = SimulateStep(input) == octopiCount;
	}
	
	return day;
}

void RenderGrid(int[][] octoGrid)
{
	var sb = new StringBuilder();
	for (int y = 0; y < octoGrid.Length; y++)
	{
		for (int x = 0; x <= octoGrid[y].Length - 1; x++)
		{
			sb.Append($"{octoGrid[y][x].ToString("D2")},");
		}
		sb.AppendLine();
	}
	
	sb.Dump();
}

int SimulateStep(int[][] octoGrid)
{
	var highEnergyOctopi = new List<(int y, int x)>();
	for (int y = 0; y < octoGrid.Length; y++)
	{
		for (int x = 0; x <= octoGrid[y].Length - 1; x++)
		{
			octoGrid[y][x]++;
			if (octoGrid[y][x] > 9)
			{
				highEnergyOctopi.Add((y, x));
			}
		}
	}
	
	var flashed = new List<(int y, int x)>();
	FlashOctopi(highEnergyOctopi.ToArray(), octoGrid, flashed);
	
	foreach (var octopus in flashed)
	{
		octoGrid[octopus.y][octopus.x] = 0;
	}
	
	return flashed.Count();
}

void FlashOctopi((int y, int x)[] highEnergyOctopi, int[][] octoGrid, List<(int y, int x)> flashed)
{
	var triggered = new List<(int y, int x)>();
	foreach(var octopus in highEnergyOctopi)
	{
		if (flashed.Contains(octopus))
			continue;
			
		triggered.AddRange(FlashOctopus(octopus, octoGrid, flashed).ToArray());
	}
	
	var unflashed = triggered.Except(flashed).ToArray();
	if (unflashed.Any())
	{
		FlashOctopi(unflashed, octoGrid, flashed);
	}
}

(int y, int x)[] FlashOctopus((int y, int x) octopus, int[][] octoGrid, List<(int y, int x)> flashed)
{	
	flashed.Add(octopus);
	
	var triggered = new List<(int y, int x)>();
	int minY = octopus.y - 1 >= 0 ? octopus.y - 1 : 0;
	int maxY = octopus.y + 1 < octoGrid.Length ? octopus.y + 1 : octoGrid.Length - 1;

	int minX = octopus.x - 1 >= 0 ? octopus.x - 1 : 0;
	int maxX = octopus.x + 1 < octoGrid[octopus.y].Length ? octopus.x + 1 : octoGrid[octopus.y].Length - 1;

	for (int y = minY; y <= maxY; y++)
	{
		for (int x = minX; x <= maxX; x++)
		{
			if (octopus.y == y && octopus.x == x)
				continue;

			try
			{
				octoGrid[y][x]++;
			}
			catch(IndexOutOfRangeException)
			{
				$"{octopus} - {minY}:{y}:{maxY} {minX}:{x}:{maxX}".Dump();
				RenderGrid(octoGrid);
			}
			if (octoGrid[y][x] > 9)
			{
				triggered.Add((y, x));
			}
		}
	};
	
	return triggered.ToArray();
}

async Task<int[][]> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/11/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Where(c => Char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToArray()).ToArray();
}

public int[][] MiniTestInput = @"11111
19991
19191
19991
11111".Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Where(c => Char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToArray()).ToArray();

public int[][] TestInput = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526".Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Where(c => Char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToArray()).ToArray();