<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	SolvePartOne(TestSoundings()).Dump();
	SolvePartTwo(TestSoundings().ToArray()).Dump();
	
	var input = await GetInput();
	
	SolvePartOne(input).Dump();
	SolvePartTwo(input.ToArray()).Dump();
}

int SolvePartOne(IList<int> soundings)
{
	var prev = soundings[0];
	var numIncreased = 0;
	
	for (int i = 1; i < soundings.Count; i++)
	{
		if (soundings[i] > prev)
		{
			numIncreased++;
		}

		prev = soundings[i];
	}

	return numIncreased;
}

int SolvePartTwo(int[] soundings)
{
	var windowedSoundings = new List<int>();
	for (int i = 0; i < soundings.Length ; i++)
	{
		if (i + 3 > soundings.Length) break;
		
		var window = soundings[i..(i + 3)];
		
		windowedSoundings.Add(window.Sum());
	}
	
	return SolvePartOne(windowedSoundings);
}

private async Task<IList<int>> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoesvaluehere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/1/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Trim().Split("\n").Select(s => Convert.ToInt32(s)).ToList();
}

private IList<int> TestSoundings()
{
	return new List<int>
	{
		199,
		200,
		208,
		210,
		200,
		207,
		240,
		269,
		260,
		263,
	};
}