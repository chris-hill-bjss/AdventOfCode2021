<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	var initialCounts = new long[9];
	
	var ages = (await GetInput())
		.Split(",", StringSplitOptions.RemoveEmptyEntries)
		.Select(s => Convert.ToInt32(s))
		.GroupBy(i => i);
		
	foreach (var age in ages)
	{
		initialCounts[age.Key] = age.Count();
	}
	
	var finalCounts = RunSimulation(initialCounts, 256, 1);
	finalCounts.Sum().Dump();
}

long[] RunSimulation(long[] counts, int until, int current)
{
	var newCounts = new long[9];
	newCounts[8] = counts[0];
	newCounts[7] = counts[8];
	newCounts[6] = counts[7] + counts[0];
	newCounts[5] = counts[6];
	newCounts[4] = counts[5];
	newCounts[3] = counts[4];
	newCounts[2] = counts[3];
	newCounts[1] = counts[2];
	newCounts[0] = counts[1];
	
	if (current < until)
	{
		return RunSimulation(newCounts, until, ++current);
	}
	
	return newCounts;
}

async Task<string> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/6/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content;
}

public string TestInput = "3,4,3,1,2";