// See https://aka.ms/new-console-template for more information


var input = File.ReadAllLines("./input/day18.txt");

var day18P1Test = new Day18(Day18.TestInput);
Console.WriteLine(day18P1Test.SolvePartOne());


var day18P2Test = new Day18(input);
Console.WriteLine(day18P2Test.SolvePartTwo());
