<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	Solve(await GetInput(), CalculateFuelCostPartOne).Dump();

	Solve(await GetInput(), CalculateFuelCostPartTwo).Dump();
}

int CalculateFuelCostPartOne(int pos, (int position, int count) c)
{
	return Math.Abs(pos - c.position) * c.count;
}

int CalculateFuelCostPartTwo(int pos, (int position, int count) c)
{
	var distance = Math.Abs(pos - c.position);
	var singleCrabCost = Enumerable.Range(1, distance).Sum();

	return singleCrabCost * c.count;
}

int Solve(int[] input, Func<int, (int, int), int> fuelCalculator)
{
	var cohorts = input.OrderBy(i => i).GroupBy(i => i).Select(grp => (position: grp.Key, count: grp.Count()));

	return
		Enumerable
			.Range(cohorts.First().position, cohorts.Last().position)
			.Select(pos => (position: pos, fuelCost: cohorts.Sum(c => fuelCalculator(pos, c))))
			.MinBy(a => a.fuelCost)
			.fuelCost;
}

async Task<int[]> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/7/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray();
}

public int[] TestInput = "16,1,2,0,4,2,7,1,2,14".Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray();