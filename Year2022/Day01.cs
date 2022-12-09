using NUnit.Framework;

namespace Year2022;

public class Day01 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        ulong current = 0;
        ulong max = 0;
        
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (current > max)
                {
                    max = current;
                }

                current = 0;
            }
            else
            {
                current += ulong.Parse(line);
            }
        }

        Console.WriteLine(max);
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        ulong current = 0;
        IList<ulong> sumCalos = new List<ulong>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                sumCalos.Add(current);
                current = 0;
            }
            else
            {
                current += ulong.Parse(line);
            }
        }

        var sum = sumCalos.OrderByDescending(x => x).Take(3).Sum(Convert.ToDecimal);
        Console.WriteLine(sum);
        Assert.Pass();
    }
}