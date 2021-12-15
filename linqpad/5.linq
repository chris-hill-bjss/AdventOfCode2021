<Query Kind="Program">
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Shapes</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

async Task Main()
{
	Solve(TestInput).Dump();
	
	Solve(await GetInput()).Dump();
}

public int Solve(string input)
{
	var lines = ProcessInput(input).ToArray();

	var width = new[] { lines.Max(l => l.X1), lines.Max(l => l.X2) }.Max();
	var height = new[] { lines.Max(l => l.Y1), lines.Max(l => l.Y2) }.Max();

	return Enumerable
		.Range(0, width + 1)
		.SelectMany(x => 
			Enumerable
				.Range(0, height + 1)
				.Select(y => 
					(
						x: x, 
						y: y, 
						points: lines
						.Where(l => l.ContainsPoint(x, y))
					)))
		.Count(a => a.points.Skip(1).Any());
}

class VentLine
{
	public VentLine(int x1, int y1, int x2, int y2)
	{
		X1 = x1;
		Y1 = y1;
		X2 = x2;
		Y2 = y2;
	}
	
	public int X1 { get; }
	public int Y1 { get; }
	public int X2 { get; }
	public int Y2 { get; }
	
	public bool ContainsPoint(int x, int y)
	{
		float dxc = x - X1;
		float dyc = y - Y1;

		float dxl = X2 - X1;
		float dyl = Y2 - Y1;

		float cross = dxc * dyl - dyc * dxl;
		
		if (cross != 0) 
			return false;

		if (Math.Abs(dxl) >= Math.Abs(dyl))
			return dxl > 0 ?
			  X1 <= x && x <= X2 :
			  X2 <= x && x <= X1;
		else
			return dyl > 0 ?
			  Y1 <= y && y <= Y2 :
			  Y2 <= y && y <= Y1;
	}

	public bool IsDiagonal => X1 != X2 && Y1 != Y2;
}

private VentLine[] ProcessInput(string input)
{
	return 
		input
			.Split("\n", StringSplitOptions.RemoveEmptyEntries)
			.Select(i => {
				var matches = Regex.Matches(i, @"(?<x>\d+),(?<y>\d+)");
				
				return new VentLine(
					Convert.ToInt32(matches[0].Groups["x"].Value),
					Convert.ToInt32(matches[0].Groups["y"].Value),
					Convert.ToInt32(matches[1].Groups["x"].Value),
					Convert.ToInt32(matches[1].Groups["y"].Value));
			})
			.ToArray();
}

private async Task<string> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/5/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content;
}

public string TestInput = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";