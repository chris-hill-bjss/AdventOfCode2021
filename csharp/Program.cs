// See https://aka.ms/new-console-template for more information


var input = File.ReadAllText("./input/day17.txt");

var day17Test = new Day17(Day17.TestInput);
Console.WriteLine(day17Test.SolvePartOne());

var day17 = new Day17(input);
Console.WriteLine(day17.SolvePartOne());


var day17P2Test = new Day17(Day17.TestInput);
Console.WriteLine(day17Test.SolvePartTwo());

var day17P2 = new Day17(input);
Console.WriteLine(day17.SolvePartTwo());
