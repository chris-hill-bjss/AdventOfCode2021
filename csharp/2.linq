<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

async Task Main()
{
	var testState = Solve(TestInput);
	(testState.X * testState.Y).Dump();
	
	var input = await GetInput();
	
	var inputState = Solve(input);
	(inputState.X * inputState.Y).Dump();
	inputState.Dump();
}

State Solve(IEnumerable<(string direction, int dinstance)> commands)
{
	var state = new State(0, 0, 0);
	
	foreach(var (direction, distance) in commands) 
	{
		state = direction switch
		{
			"up" => new State(state.X, state.Y, state.Aim - distance),
			"down" => new State(state.X, state.Y, state.Aim + distance),
			"forward" => new State(state.X + distance, state.Y + (state.Aim * distance), state.Aim),
			_ => throw new ArgumentException("direction not supported")
		};	
	}
	
	return state;
}

class State {
	public State(int x, int y, int aim)
	{
		X = x;
		Y = y;
		Aim = aim;
	}

	public int X { get; }
	public int Y { get; }
	public int Aim { get; }
}

IEnumerable<(string direction, int dinstance)> TestInput => new[] {
	("forward", 5),
	("down", 5),
	("forward", 8),
	("up", 3),
	("down", 8),
	("forward", 2)
};

async Task<IEnumerable<(string direction, int dinstance)>> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoesvaluehere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/2/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Trim().Split("\n").Select(s => (s.Split(' ')[0], Convert.ToInt32(s.Split(' ')[1])));
}