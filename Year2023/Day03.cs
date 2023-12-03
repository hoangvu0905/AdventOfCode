using NUnit.Framework;

namespace Year2023;

public class Day03 : BaseDayTest
{
    private readonly IList<Number> _numbers = new List<Number>();

    [OneTimeSetUp]
    public void ParseData()
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var number = 0;
            var start = 0;

            for (var j = 0; j < line.Length; j++)
            {
                if (char.IsDigit(line[j]))
                {
                    // If it is number continue adding to the number
                    if (number == 0) start = j;
                    number = number * 10 + (line[j] - '0');
                }
                else
                {
                    // If it is not a number, check if it is a valid number and reset the number
                    if (number == 0) continue;

                    if (NumberIsNearSymbol(i, start, j - 1))
                    {
                        _numbers.Add(new Number(number, i, start, j - 1));
                    }

                    number = 0;
                }
            }

            if (NumberIsNearSymbol(i, start, line.Length - 1) && number != 0)
            {
                _numbers.Add(new Number(number, i, start, line.Length - 1));
            }
        }
    }
    
    [Test]
    public override void Part1()
    {
        Console.WriteLine(_numbers.Sum(x => x.Value));
        Assert.Pass();
    }

    private bool NumberIsNearSymbol(int indexLine, int startIndex, int endIndex)
    {
        for (var i = Math.Max(0, indexLine - 1); i < Math.Min(lines.Count, indexLine + 2); i++)
        {
            var line = lines[i];
            for (int j = Math.Max(0, startIndex - 1); j < Math.Min(line.Length, endIndex + 2); j++)
            {
                if (!char.IsDigit(line[j]) && line[j] != '.') return true;
            }
        }

        return false;
    }


    [Test]
    public override void Part2()
    {
        var result = 0L;

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            for (var j = 0; j < line.Length; j++)
            {
                if (line[j] == '*')
                {
                    var minX = Math.Max(0, i - 1);
                    var maxX = Math.Min(lines.Count - 1, i + 1);
                    var minY = Math.Max(0, j - 1);
                    var maxY = Math.Min(line.Length - 1, j + 1);
                    var numbers = _numbers.Where(x => x.Line >= minX && x.Line <= maxX && x.IndexRight >= minY && x.IndexLeft <= maxY).ToList();
                    if (numbers.Count < 2) continue;

                    if (numbers.Count == 3) throw new Exception("Have 3 numbers valid");

                    result += numbers[0].Value * numbers[1].Value;
                }
            }
        }

        Console.WriteLine(result);
        Assert.Pass();
    }
    
    public record Number(int Value, int Line, int IndexLeft, int IndexRight);
}