// See https://aka.ms/new-console-template for more information


var input = File.ReadAllText("./csharp/input/day16.txt");

foreach(var testInput in Day16.TestInputs)
{
    var day16Test = new Day16(testInput);
    Console.WriteLine(day16Test.Solve());
}

var day16 = new Day16(input);
Console.WriteLine(day16.Solve());
