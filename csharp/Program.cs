// See https://aka.ms/new-console-template for more information


var input = File.ReadAllText("./input/day19.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

Console.WriteLine(new Day19(input).SolvePartOne());
Console.WriteLine(new Day19(input).SolvePartTwo());