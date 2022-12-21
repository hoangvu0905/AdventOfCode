using System.Numerics;
using NUnit.Framework;

namespace Year2022;

public class Day21 : BaseDayTest
{
    private readonly Dictionary<string, BigInteger> _dict = new();

    [Test]
    public override void Part1()
    {
        var result = GetValueNumberRoot();
        Console.WriteLine($"{result.Root}");

        Assert.That(result.Root.ToString(), Is.EqualTo("121868120894282"));
    }

    [Test]
    public override void Part2()
    {
        var min = new BigInteger(0);
        var max = BigInteger.Parse("121868120894282");

        while (min != max)
        {
            (min, max) = GetValueNumberRoot(min, max);
        }

        // Because will have a range satisfied. So we need min of range.
        var result = GetValueNumberRoot(--min);
        while (result.Left == result.Right)
        {
            result = GetValueNumberRoot(--min);
        }

        Console.WriteLine(++min);
        Assert.That(min.ToString(), Is.EqualTo("3582317956029"));
    }

    private (BigInteger MinHuman, BigInteger MaxHuman) GetValueNumberRoot(BigInteger minHuman, BigInteger maxHuman)
    {
        var mid = (minHuman + maxHuman) / 2;
        var value = GetValueNumberRoot(mid);

        if (value.Left == value.Right)
        {
            return (mid, mid);
        }

        return value.Left > value.Right ? (mid, maxHuman) : (minHuman, mid);
    }

    private (BigInteger Root, BigInteger Left, BigInteger Right) GetValueNumberRoot(BigInteger? human = null)
    {
        _dict.Clear();
        if (human.HasValue) _dict.Add("humn", human.Value);

        while (true)
        {
            foreach (var line in lines)
            {
                var name = line[..4];
                if (_dict.ContainsKey(name)) continue;

                if (BigInteger.TryParse(line[6..], out var number))
                {
                    _dict.Add(name, number);
                    continue;
                }

                var monkey1 = line[6..10];
                var monkey2 = line[13..];
                if (!_dict.ContainsKey(monkey1) || !_dict.ContainsKey(monkey2)) continue;

                var value = GetValueOperator(_dict[monkey1], _dict[monkey2], line[11]);
                _dict.Add(name, value);

                if (name == "root")
                {
                    return (value, _dict[monkey1], _dict[monkey2]);
                }
            }
        }
    }

    private static BigInteger GetValueOperator(BigInteger number1, BigInteger number2, char ope)
    {
        return ope switch
        {
            '+' => number1 + number2,
            '-' => number1 - number2,
            '*' => number1 * number2,
            '/' => number1 / number2,
            _ => throw new ArgumentOutOfRangeException(nameof(ope), "cannot identify a operator")
        };
    }
}