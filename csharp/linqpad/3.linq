<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	SolvePartOne(await GetInput()).Dump();
	SolvePartTwo(await GetInput()).Dump();
}

int SolvePartOne(IList<string> input)
{
	var bitPositions = ReadBitPositions(input);

	var gamma = new string((bitPositions.Select(bits => bits.Average(c => c == '1' ? 1 : 0) >= 0.5 ? '1' : '0').ToArray()));
	var epsilon = new string(gamma.Select(c => c == '1' ? '0' : '1').ToArray());

	var g = Convert.ToInt32(gamma, 2);
	var e = Convert.ToInt32(epsilon, 2);

	return g * e;
}

int SolvePartTwo(IList<string> input)
{
	var oxyGenRating = FindRating(input, 0, v => v >= 0.5 ? '1' : '0');
	var c02ScrubberRating = FindRating(input, 0, v => v >= 0.5 ? '0' : '1');

	var o = Convert.ToInt32(oxyGenRating, 2);
	var c = Convert.ToInt32(c02ScrubberRating, 2);

	return o * c;
}

string FindRating(IList<string> input, int position, Func<double, char> bitSelector)
{
	var bitPositions = ReadBitPositions(input).ToList();

	var significantBit = bitSelector(bitPositions[position].Average(c => c == '1' ? 1 : 0));
	var validInputs = input.Where(reading => reading[position] == significantBit).ToList();

	if (validInputs.Count > 1)
	{
		return FindRating(validInputs, ++position, bitSelector);
	}
	
	return validInputs.First();
}

IEnumerable<IEnumerable<char>> ReadBitPositions(IList<string> input)
{
	return Enumerable.Range(0, input[0].Length).Select(i => input.Select(r => r[i]));
}

IList<string> TestInput = new[]{
	"00100",
	"11110",
	"10110",
	"10111",
	"10101",
	"01111",
	"00111",
	"11100",
	"10000",
	"11001",
	"00010",
	"01010"
};

private async Task<IList<string>> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoesvaluehere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/3/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Trim().Split("\n").ToList();
}