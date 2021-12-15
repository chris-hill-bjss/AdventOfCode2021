// See https://aka.ms/new-console-template for more information


var input = File.ReadAllLines("./csharp/input/day15.txt")
    .Select(row => row.Where(c => Char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToArray())
    .ToArray();

var day15PartOne = new Day15(input);
Console.WriteLine(day15PartOne.Solve());

var largeInput = GrowArray(input);

var day15PartTwo = new Day15(largeInput);
Console.WriteLine(day15PartTwo.Solve());

int[][] GrowArray(int[][] input)
{
    var largeInput = new int[input.Length * 5][];

    for (int y = 0; y < largeInput.Length; y++)
    {
        largeInput[y] = new int[input[0].Length * 5];
        for (int x = 0; x < largeInput[0].Length; x++)
        {
            var oldY = y % input.Length;
            var oldX = x % input[0].Length;
            var oldVal = input[oldY][oldX];

            var modY = y / input.Length;
            var modX = x / input[0].Length;

            var newVal = oldVal + modY + modX;
            largeInput[y][x] = newVal > 9 ? newVal - 9 : newVal;
        }
    }

    return largeInput;
}