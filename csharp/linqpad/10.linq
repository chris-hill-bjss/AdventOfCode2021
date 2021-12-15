<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	SolvePartOne(await GetInput()).Dump();
	SolvePartTwo(await GetInput()).Dump();
}

Dictionary<char, int> _corruptCharacterScores = new()
{
	{ ')', 3 },
	{ ']', 57 },
	{ '}', 1197 },
	{ '>', 25137 }
};

Dictionary<char, int> _completionCharacterScores = new()
{
	{ ')', 1 },
	{ ']', 2 },
	{ '}', 3 },
	{ '>', 4 }
};

char[] _openingChars = new[] { '(', '[', '{', '<' };
char[] _closingChars = new[] { ')', ']', '}', '>' };

char[] ValidChars => _openingChars.Union(_closingChars).ToArray();

char ClosingCharForOpening(char opening) => _closingChars[Array.IndexOf(_openingChars, opening)];

int SolvePartOne(string[] input)
{
	return input
		.Select(line =>
		{
			var open = new List<char>();
			var chunks = new List<(char open, char close, bool isCorrupt, bool isComplete)>();
			for (int i = 0; i < line.Length; i++)
			{
				var c = line[i];

				if (_openingChars.Contains(c))
				{
					open.Add(c);
				}
				else if (_closingChars.Contains(c))
				{
					var last = open.Last();
					
					chunks.Add((last, c, c != ClosingCharForOpening(last), ValidChars.Contains(c)));
				
					open.RemoveAt(open.LastIndexOf(last));
				}
			}
			
			foreach (var incomplete in open)
			{
				chunks.Add((incomplete, ' ', false, false));
			}
			
			return (line, chunks);
		})
		.Where(syntaxChecks => syntaxChecks.chunks.Any(c => c.isCorrupt))
		.SelectMany(syntaxChecks => syntaxChecks.chunks.Where(c => c.isCorrupt))
		.Select(syntaxCheck => _corruptCharacterScores[syntaxCheck.close])
		.Sum();
}

long SolvePartTwo(string[] input)
{
	var scores = input
		.Select(line =>
		{
			var open = new List<char>();
			var chunks = new List<(char open, char close, bool isCorrupt, bool isComplete)>();
			for (int i = 0; i < line.Length; i++)
			{
				var c = line[i];

				if (_openingChars.Contains(c))
				{
					open.Add(c);
				}
				else if (_closingChars.Contains(c))
				{
					var last = open.Last();

					chunks.Add((last, c, c != ClosingCharForOpening(last), ValidChars.Contains(c)));

					open.RemoveAt(open.LastIndexOf(last));
				}
			}

			foreach (var incomplete in open)
			{
				chunks.Add((incomplete, ClosingCharForOpening(incomplete), false, false));
			}

			return (line, chunks);
		})
		.Where(syntaxChecks => syntaxChecks.chunks.All(c => !c.isCorrupt))
		.Select(syntaxChecks => syntaxChecks.chunks.Where(c => !c.isComplete).Reverse().Select(c => c.close))
		.Select(closingChars => 
		{
			long total = 0;
			
			foreach(var c in closingChars)
			{
				total = total * 5;
				total += _completionCharacterScores[c];
			}
			
			return total;
		})
		.OrderBy(score => score)
		.ToArray();
		
	return scores[scores.Length / 2];
}

async Task<string[]> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/10/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();
}

public string[] TestInput = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]".Split('\n', StringSplitOptions.RemoveEmptyEntries).ToArray();