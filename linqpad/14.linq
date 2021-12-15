<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	BruteForce(TestInput, 10).Dump();
	Solve(await GetInput(), 40).Dump();
}

long Solve(string[] input, int maxSteps)
{
	var (pairs, rules) = ParseInput(input);

	var finalPairs = RunSteps(pairs, rules, 1, maxSteps);
	
	var elementCounts = 
		finalPairs
			.Append((pair: input[0][^1..], count: 1L))
			.GroupBy(pair => pair.pair[0], (element, count) => (element, count: count.Sum(p => p.count)));

	var max = elementCounts.MaxBy(e => e.count);
	var min = elementCounts.MinBy(e => e.count);

	return max.count - min.count;
}

List<(string pair, long count)> RunSteps(List<(string pair, long count)> pairs, Dictionary<string, string[]> rules, int step, int maxSteps)
{
	var newPairs = 
		pairs
			.SelectMany(pair => rules[pair.pair].Select(childPair => (pair: childPair, count: pair.count)))
			.GroupBy(grp => grp.pair, (pair, count) => (pair, count.Sum(i => i.count)))
			.ToList();
			
	if (step < maxSteps)
	{
		return RunSteps(newPairs, rules, ++step, maxSteps);
	}

	return newPairs;
}

(List<(string pair, long count)> pairs, Dictionary<string, string[]> rules) ParseInput(string[] input)
{
	var pairs = input
			.Take(1)
			.SelectMany(s => s.Select((c, i) => i < (s.Length - 1) ? string.Join("", c, s[i + 1]) : string.Empty))
			.Where(pair => !String.IsNullOrWhiteSpace(pair))
			.Select(pair => (pair, count: 1L))
			.ToList();

	var rules = input
		.Skip(1)
		.Select(s => s.Split(" -> "))
		.ToDictionary(
			rule => rule[0],
			rule => new[]
			{
				string.Join("", rule[0][0], rule[1]),
				string.Join("", rule[1], rule[0][1])
			});
			
	return (pairs, rules);
}

int BruteForce(string[] input, int steps)
{
	var basePolymer = new LinkedList<char>(input.First());
	var rules = input.Skip(1).ToDictionary(s => s.Split("->")[0].Trim(), s => Convert.ToChar(s.Split("->")[1].Trim()));

	var finalPolymer = GrowPolymer(basePolymer, rules, 0, steps);
	var elementCounts = finalPolymer
		.GroupBy(c => c)
		.Select(grp => (element: grp.Key, count: grp.Count()));
	
	var max = elementCounts.MaxBy(e => e.count);
	var min = elementCounts.MinBy(e => e.count);
	
	return max.count - min.count;
}

LinkedList<char> GrowPolymer(LinkedList<char> basePolymer, Dictionary<string, char> rules, int step, int stepMax)
{
	if (step < stepMax)
	{
		var newPolymer = ApplyRules(basePolymer, rules);
		return GrowPolymer(newPolymer, rules, ++step, stepMax); 
	}
	
	return basePolymer;
}

LinkedList<char> ApplyRules(LinkedList<char> basePolymer, Dictionary<string, char> rules)
{
	var newPolymer = new LinkedList<char>();
	
	var currentNode = basePolymer.First;
	while((currentNode != null))
	{
		if (currentNode.Next == null)
		{
			newPolymer.AddLast(currentNode.Value);
			break;
		}
			
		var pair = new string(new[] { currentNode.Value, currentNode.Next.Value });
		var element = rules[pair];
		
		if (newPolymer.First == null)
		{
			newPolymer.AddFirst(currentNode.Value);
		}
		else
		{
			newPolymer.AddLast(currentNode.Value);
		}
		
		newPolymer.AddLast(element);
		currentNode = currentNode.Next;
	}
	
	return newPolymer;
}

async Task<string[]> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };

	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/14/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
}

public string[] TestInput = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C".Split("\r\n", StringSplitOptions.RemoveEmptyEntries);