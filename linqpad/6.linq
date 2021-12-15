<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	Solve(await GetInput(), untilDay: 256).Dump();
}

class Shoal
{
	public Shoal(long count, int daysToSpawn = 8)
	{
		Count = count;
		DaysToSpawn = daysToSpawn;
	}

	public long Count { get; private set; }
	public int DaysToSpawn { get; private set; }

	public Shoal? Age(int days)
	{
		var Shoal = DaysToSpawn == 0 ? new Shoal(Count) : null;

		DaysToSpawn = DaysToSpawn > 0 ? DaysToSpawn - days : 6;

		return Shoal;
	}
}

long Solve(string input, int untilDay)
{
	var initialPopulation = input
		.Split(',', StringSplitOptions.RemoveEmptyEntries)
		.Select(s => new Shoal(1, Convert.ToInt32(s)))
		.GroupBy(s => s.DaysToSpawn)
		.Select(grp => new Shoal(grp.Sum(s => s.Count), grp.Key))
		.ToArray();
		
	var finalPopulation = RunSimulation(initialPopulation, untilDay, 1);
		
	return finalPopulation.Sum(f => f.Count);
}

Shoal[] RunSimulation(Shoal[] shoals, int untilDay, int currentDay)
{
	var newShoals = shoals.Select(s => s.Age(1)).Where(s => s != null).ToArray();

	var allShoals = 
		shoals
			.Concat(newShoals)
			.GroupBy(s => s.DaysToSpawn)
			.Select(grp => new Shoal(grp.Sum(s => s.Count), grp.Key))
			.ToArray();
	
	if (currentDay < untilDay)
	{
		return RunSimulation(allShoals, untilDay, ++currentDay);
	}
	
	return allShoals;
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