<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	Solve(await GetInput(), winners => winners.First()).Dump();
	Solve(await GetInput(), winners => winners.Last()).Dump();
}

int Solve(IList<string> input, Func<IList<(int lastBall, Board board)>, (int lastBall, Board board)> winCondition) {
	var bingoBalls = input[0].Split(',').Select(s => Convert.ToInt32(s));

	var boards = GetBoards(input.Skip(1)).ToArray();
	
	var winningBoards = PlayBingo(bingoBalls, boards);
	
	var (lastBall, winningBoard) = winCondition(winningBoards);
	var unmarkedNumbersSum = winningBoard
		.Values
		.Where(v => v.Marked == false)
		.Sum(v => v.BallNumber);
	
	return lastBall * unmarkedNumbersSum;
}

private IList<(int lastBall, Board board)> PlayBingo(IEnumerable<int> bingoBalls, Board[] boards)
{
	var winningBoards = new List<(int lastBall, Board board)>();
	foreach (var ballNumber in bingoBalls)
	{
		foreach (var board in boards)
		{
			if (board.Bingo()) continue;
			
			board.AcceptBall(ballNumber);
			if (board.Bingo())
			{
				winningBoards.Add((ballNumber, board));
			}
		}
	}
	
	return winningBoards;
}

private IEnumerable<Board> GetBoards(IEnumerable<string> input)
{
	return input.Select(s =>
	{
		var boardValues =
			s
				.Split('\n', StringSplitOptions.RemoveEmptyEntries)
				.Select((row, y) =>
					(
						y,
						rowValues: row
							.Split(' ', StringSplitOptions.RemoveEmptyEntries)
							.Select(val => new BoardValue(Convert.ToInt32(val)))
							.ToArray()
					))
				.ToDictionary(a => a.y, a => a.rowValues);

		return new Board(boardValues);
	});
}

class BoardValue
{
	public BoardValue(int ballNumber)
	{
		BallNumber = ballNumber;
	}
	
	public int BallNumber { get; set; }
	public bool Marked { get; private set; }
	
	public void Mark() => Marked = true;

	public override string ToString()
	{
		return $"{BallNumber.ToString("D2")}:{Marked}";
	}
}

class Board
{
	private readonly IDictionary<int, BoardValue[]> _board = new Dictionary<int, BoardValue[]>();

	public Board(IDictionary<int, BoardValue[]> boardValues)
	{
		_board = boardValues;
	}

	public IEnumerable<BoardValue> Values => _board.Values.SelectMany(v => v);
	
	public void AcceptBall(int ballNumber)
	{
		if (Values.ToDictionary(v => v.BallNumber).TryGetValue(ballNumber, out BoardValue value))
		{
			value.Mark();
		}
	}
	
	public bool Bingo()
	{
		var hasBingoOnRow = _board.Values.Any(row => row.All(v => v.Marked));

		var hasBingoOnCol = Enumerable.Range(0, _board[0].Length)
			.Select(col => _board.Keys.Select(row => _board[row][col]))
			.Any(col => col.All(v => v.Marked));
		
		return hasBingoOnRow || hasBingoOnCol;
	}

	public override string ToString()
	{
		var sb = new StringBuilder();
		
		foreach(var row in _board.Keys)
		{
			foreach(var value in _board[row])
			{
				sb.Append(value.ToString());
			}
			sb.AppendLine();
		}
		
		return sb.ToString();
	}
}

private async Task<IList<string>> GetInput()
{
	using var httpClient = new HttpClient { BaseAddress = new Uri("https://adventofcode.com/") };
	httpClient.DefaultRequestHeaders.Add("Cookie", "<yourcookiegoeshere>");
	var response = await httpClient.GetAsync(new Uri("/2021/day/4/input", UriKind.Relative));
	response.EnsureSuccessStatusCode();

	var content = await response.Content.ReadAsStringAsync();

	return content.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
}

IList<string> TestInput = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7".Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
