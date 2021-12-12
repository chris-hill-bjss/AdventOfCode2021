<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	Solve(await GetInput(), PartOneVisitRule).Dump();
	Solve(await GetInput(), PartTwoVisitRule).Dump();
}

IEnumerable<string> PartOneVisitRule(Stack<string> currentRoute) => 
	currentRoute
		.Where(cave => cave.All(c => Char.IsLower(c)));


IEnumerable<string> PartTwoVisitRule(Stack<string> currentRoute)
{
	var smallCaves = currentRoute.Where(cave => cave.All(c => Char.IsLower(c))).GroupBy(cave => cave);
	
	if (smallCaves.All(visited => visited.Count() <= 1))
		return Enumerable.Empty<string>();
	
	return smallCaves.SelectMany(grp => grp);
}
	
int Solve(string[] input, Func<Stack<string>, IEnumerable<string>> cannotVisit)
{
	var map = input
		.Select(reading => (source: reading.Split('-')[0].Trim(), neighbour: reading.Split('-')[1].Trim()))
		.Union(input.Select(reading => (source: reading.Split('-')[1].Trim(), neighbour: reading.Split('-')[0].Trim())))
		.OrderBy(r => r.source)
		.Where(r => r.source != "end" && r.neighbour != "start")
		.GroupBy(r => r.source)
		.ToDictionary(grp => grp.Key, grp => grp.Select(reading => reading.neighbour).OrderBy(c => c).ToArray());

	var currentRoute = new Stack<string>();
	var routes = new List<string>();
	
	ChartCave("start", map, currentRoute, routes, cannotVisit);
	return routes.Count();
}

void ChartCave(
	string current, 
	Dictionary<string, string[]> map, 
	Stack<string> currentRoute, 
	List<string> routes, 
	Func<Stack<string>, IEnumerable<string>> cannotVisit)
{
	var paths = new Dictionary<string, List<string>>();
	var connectedCaves = map[current].Except(cannotVisit(currentRoute));
	
	foreach(var cave in connectedCaves)
	{	
		if (cave != "end")
		{
			if (!map.ContainsKey(cave))
				continue;
				
			currentRoute.Push(cave);
			ChartCave(cave, map, currentRoute, routes, cannotVisit);
			currentRoute.Pop();
		}
		else
		{
			routes.Add($"start,{String.Join(",", currentRoute.Reverse())},end");
		}
	}
}

async Task<String[]> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/12/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();
}

public string[] NanoTestInput = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end".Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();

public string[] MiniTestInput = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc".Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();

public string[] TestInput = @"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW".Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();