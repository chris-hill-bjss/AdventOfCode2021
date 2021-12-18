using System.Text.RegularExpressions;

internal sealed class Day18
{
    private readonly string[] _input;

    public Day18(string[] input)
    {
        _input = input;
    }

    public int SolvePartOne()
    {
        var answer = _input.Aggregate((a, b) => Reduce($"[{a},{b}]"));
        
        return AnswerMagnitude(answer);
    }

    public int SolvePartTwo()
    {
        var answerMagnitudes = 
            _input
                .SelectMany(a => 
                    _input
                        .Where(b => a != b)
                        .Select(b => AnswerMagnitude(Reduce($"[{a},{b}]"))))
                .ToArray();

        return answerMagnitudes.Max();
    }

    private int AnswerMagnitude(string answer)
    {
        MatchCollection matches;
        while((matches = Regex.Matches(answer, @"\[(\d+),(\d+)\]")).Any())
        {
            foreach(Match match in matches)
            {
                var a = Convert.ToInt32(match.Groups[1].Value);
                var b = Convert.ToInt32(match.Groups[2].Value);

                var magnitude = (a * 3) + (b * 2);

                answer = answer.Replace(match.Value, $"{magnitude}");
            }
        }

        return Convert.ToInt32(answer);
    }

    private string Reduce(string sum)
    {
        var (shouldExplode, explodeAt) = ShouldExplode(sum);
        if (shouldExplode)
        {
            sum = Explode(sum, explodeAt);
            return Reduce(sum);
        }

        if (Regex.IsMatch(sum, @"\d{2}"))
        {
            var number = Regex.Matches(sum, @"\d{2}")[0].Value;
            var numberPos = sum.IndexOf(number);
            var val = Convert.ToInt32(number);

            sum = sum.Remove(numberPos, number.Length).Insert(numberPos, $"[{Math.Floor(val/2d)},{Math.Ceiling(val/2d)}]");

            return Reduce(sum);
        }
            
        return sum;
    }

    private string Explode(string sum, int explodeAt)
    {
            int offset = 1;
            var pair = string.Join("", sum[explodeAt..].TakeWhile(c => c !=']').ToArray());

            var rightNumberDigits = GetNumberDigits(sum[(explodeAt+pair.Length)..]);
            if (rightNumberDigits.Any())
            {
                var rightNumber = Convert.ToInt32(string.Join("", rightNumberDigits.Select(t => t.c)));
                var rightNumberPos = rightNumberDigits.First().pos;

                rightNumber += Convert.ToInt32(pair.Split(",")[1]);
                var replaceNumberAt = rightNumberPos + explodeAt + pair.Length;
                sum = sum.Remove(replaceNumberAt, rightNumberDigits.Length).Insert(replaceNumberAt, $"{rightNumber}");
            }

            var leftNumberDigits = 
                GetNumberDigits(string.Join("", sum[0..explodeAt].Reverse().ToArray()))
                    .Reverse()
                    .ToArray();

            if (leftNumberDigits.Any())
            {
                var leftNumber = Convert.ToInt32(string.Join("", leftNumberDigits.Select(t => t.c)));
                var leftNumberPos = leftNumberDigits.First().pos;

                leftNumber += Convert.ToInt32(pair.Split(",")[0]);
                var replaceNumberAt = (explodeAt - leftNumberPos) - 1;
                sum = sum.Remove(replaceNumberAt, leftNumberDigits.Length).Insert(replaceNumberAt, $"{leftNumber}");
                
                if (leftNumberDigits.Length < 2 && leftNumber > 9) --offset;
            }
            
            sum = sum.Remove(explodeAt - offset, pair.Length + 2);
            sum = sum.Insert(explodeAt - offset, "0");

            return sum;
    }

    private (int pos, char c)[] GetNumberDigits(string input) =>
        input
            .Select((c, i) => (i, c))
            .SkipWhile(t => !Char.IsNumber(t.c))
            .TakeWhile(t => Char.IsNumber(t.c))
            .ToArray();

    private (bool explode, int pos) ShouldExplode(string sum)
    {
        var depth = 0;
        var explodeAt = sum.Select((c, i) => (i, c)).FirstOrDefault(t =>
        {
            depth+= t.c == '[' ? 1 : t.c == ']' ? - 1 : 0;
            return depth > 4;
        }, (i: -1, c: '.'));
        
        return (explodeAt.i != -1, explodeAt.i + 1);
    }

    public static string[] TestInput = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"
    .Split("\n", StringSplitOptions.RemoveEmptyEntries);
}