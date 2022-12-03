using NUnit.Framework;

namespace Year2022;

public class Day02 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        ulong result = 0;
        foreach (var line in lines)
        {
            result += GetScoreWin(line[0], line[2]);
            result += GetScore(line[2]);
            
        }

        Console.WriteLine(result);
    }
    
    [Test]
    public override void Part2()
    {
        ulong result = 0;
        foreach (var line in lines)
        {
            var myscore = GetScore(line[2]);
            result += (myscore - 1) * 3; // add score

            var temp = GetScore(line[0]) + myscore - 2;
            result += temp switch
            {
                0 => 3,
                4 => 1,
                _ => temp
            };
        }

        Console.WriteLine(result);
    }


    private static ulong GetScoreWin(char opp, char me)
    {
        var valueMe = GetScore(me);
        var valueOpp = GetScore(opp);
        if (valueMe == valueOpp)
        {
            return 3;
        }

        if (valueMe == valueOpp + 1 || valueMe + 2 == valueOpp)
        {
            return 6;
        }

        return 0;
    }

    private static ulong GetScore(char me)
    {
        return me switch
        {
            'X' or 'A' => 1ul,
            'Y' or 'B' => 2ul,
            'Z' or 'C' => 3ul,
            _ => throw new Exception()
        };
    }
}