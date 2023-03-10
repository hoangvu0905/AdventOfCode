using System.Numerics;
using NUnit.Framework;

namespace Year2022;

public class Day25 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        var result = new BigInteger(0);
        foreach (var line in lines)
        {
            var number = DecodeSNAFU(line);
            result += number;
            // Console.WriteLine(number);
        }

        Console.WriteLine(result);
        Console.WriteLine(EncodeSNAFU(result));
    }

    [Test]
    public override void Part2()
    {
        foreach (var line in lines)
        {
        }
    }

    private static BigInteger DecodeSNAFU(string line)
    {
        var result = new BigInteger(0);
        for (var i = 0; i < line.Length; i++)
        {
            var number = new BigInteger(line[line.Length - i - 1] switch
            {
                '-' => -1,
                '=' => -2,
                var chr => chr - '0'
            });
            
            result += number * BigInteger.Pow(new BigInteger(5), i);
        }

        return result;
    }

    private static string EncodeSNAFU(BigInteger number)
    {
        var result = "";

        int maxPoint;

        for (var i = 0;; i++)
        {
            if (Math.Exp(BigInteger.Log(number) - BigInteger.Log(BigInteger.Pow(new BigInteger(5), i))) < 2.5d)
            {
                maxPoint = i;
                break;
            }
        }

        for (int i = maxPoint; i >= 0; i--)
        {
            var numberBase = BigInteger.Pow(new BigInteger(5), i);
            var temp =  (int)Math.Round(Math.Exp(BigInteger.Log(BigInteger.Abs(number)) - BigInteger.Log(numberBase))) * number.Sign;
            number -= BigInteger.Multiply(temp, numberBase);
            result += temp switch
            {
                -1 => '-',
                -2 => '=',
                _ => temp.ToString()
            };
        }

        return result;
    }
}