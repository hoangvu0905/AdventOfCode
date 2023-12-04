using NUnit.Framework;

namespace Year2023;

public class Day04 : BaseDayTest
{
    private const int MaxCard = 201;
    private readonly int[] _noCards = new int[MaxCard];
    
    [Test]
    public override void Part1()
    {
        var result = 0;

        foreach (var line in lines)
        {
            var numbers = GetNumberWinning(line);
            result += numbers.Count == 0 ? 0 : (int)Math.Pow(2, numbers.Count - 1);
        }

        Console.WriteLine(result);
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        Array.Fill(_noCards, 1, 0, lines.Count);

        for (var i = 0; i < lines.Count; i++)
        {
            var numbers = GetNumberWinning(lines[i]);
            for (var j = i + 1; j <= numbers.Count + i; j++)
            {
                _noCards[j] += _noCards[i];
            }
        }
        
        Console.WriteLine(_noCards.Sum());
        Assert.Pass();
    }

    public IList<int> GetNumberWinning(string line)
    {
        var parts = line.Split(new[] { ':', '|' }, StringSplitOptions.TrimEntries);
        var winningNumbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var numbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        return numbers.Intersect(winningNumbers).ToList();
    }
}