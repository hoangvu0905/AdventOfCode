using NUnit.Framework;

namespace Year2022;

public class Day04 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        ulong result = 0;
        foreach (var line in lines)
        {
            var numbers = GetNumber(line);
            if (IsRangeInRange(numbers[0], numbers[1], numbers[2], numbers[3]) ||
                IsRangeInRange(numbers[2], numbers[3], numbers[0], numbers[1]))
            {
                result++;
            }
        }

        Console.WriteLine(result);
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        ulong result = 0;
        foreach (var line in lines)
        {
            var numbers = GetNumber(line);
            if (IsOverlapSingle(numbers[0], numbers[1], numbers[2], numbers[3]) ||
                IsOverlapSingle(numbers[2], numbers[3], numbers[0], numbers[1]))
            {
                result++;
            }
        }

        Console.WriteLine(result);    
        Assert.Pass();
    }

    private static List<int> GetNumber(string line)
    {
        return line.Split(new []{',', '-'}).Select(int.Parse).ToList();
    }

    private static bool IsOverlapSingle(int min, int max, int min2, int max2) => min <= max2 && max >= min2;
    private static bool IsRangeInRange(int min, int max, int min2, int max2) => min <= min2 && max >= max2;
}