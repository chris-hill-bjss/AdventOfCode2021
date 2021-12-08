<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	SolvePartOne(TestInput).Dump();
	
	SolvePartTwo(await GetInput()).Dump();
}

readonly Dictionary<int, int[]> PatternLengthToPossibleNumberMap = new()
{
	{ 2, new[] { 1 } },
	{ 3, new[] { 7 } },
	{ 4, new[] { 4 } },
	{ 5, new[] { 2, 3, 5 } },
	{ 6, new[] { 0, 6, 9 } },
	{ 7, new[] { 8 } }
};

int SolvePartOne(string[] input)
{
	return
		ParseNotes(input)
		.Sum(n => n.output.Count(o => o.possibleNumbers.Length == 1));
}

int SolvePartTwo(string[] input)
{
	var outputs = ParseNotes(input)
		.Select(n => ConvertOutput(n));

	return outputs.Sum();
}

int ConvertOutput((IEnumerable<string> patterns, IEnumerable<(string pattern, int length, int[] possibleNumbers)> output) note)
{
	var wires =
		note.patterns
			.Select(p => String.Concat(p.OrderBy(c => c)));
	// rules
	// 1 is c f
	// 2 is a c d e g
	// 3 is a c d f g
	// 4 is b c d f
	// 5 is a b d e g
	// 6 is a b d e f g
	// 7 is a c f
	// 8 is a b c d e f g
	// 9 is a b c d f g
	
	var mappings = new Dictionary<int, string>();
	mappings[1] = wires.Single(w => w.Length == 2);
	mappings[4] = wires.Single(w => w.Length == 4);
	mappings[7] = wires.Single(w => w.Length == 3);
	mappings[8] = wires.Single(w => w.Length == 7);


	mappings[6] = wires.Single(w => w.Length == 6 && !mappings[1].All(c => w.Contains(c)));
	var c = mappings[1].Except(mappings[6]).Single();
	var f = mappings[1].Except(new[] { c }).Single();
	
	mappings[2] = wires.Single(w => w.Length == 5 && (w.Contains(c) && !w.Contains(f)));
	mappings[3] = wires.Single(w => w.Length == 5 && (w.Contains(c) && w.Contains(f)));
	mappings[5] = wires.Single(w => w.Length == 5 && (!w.Contains(c) && w.Contains(f)));

	var e = mappings[6].Except(mappings[5]).Single();
	mappings[0] = wires.Single(w => w.Length == 6 && w.Contains(e) && !mappings.ContainsValue(w));
	
	mappings[9] = wires.Single(w => !mappings.ContainsValue(w));	
	
	var reverseMap = mappings.ToDictionary(m => m.Value, m => m.Key);
	var sortedOutput = note.output.Select(o => String.Concat(o.pattern.OrderBy(c => c)));
	
	var output = String.Join(String.Empty, sortedOutput.Select(p => reverseMap[p]));
	return Convert.ToInt32(output);
}

IEnumerable<(IEnumerable<string> patterns, IEnumerable<(string pattern, int length, int[] possibleNumbers)> output)> ParseNotes(string[] input)
{
	return
		input
			.Select(s =>
			{
				var combinedInput = s.Split('|', StringSplitOptions.RemoveEmptyEntries);
				var patterns = combinedInput[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());
				var output = combinedInput[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim());

				return (patterns, output: output.Select(o => (pattern: o, length: o.Length, possibleNumbers: PatternLengthToPossibleNumberMap[o.Length])));
			});
}

async Task<string[]> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoesehere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/8/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();
}

public string[] TestInput = @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce".Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();