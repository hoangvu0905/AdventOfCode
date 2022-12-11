using System.Numerics;
using NUnit.Framework;

namespace Year2022;

public class Day11 : BaseDayTest
{
    private readonly IList<Monkey> _monkeys = new List<Monkey>();


    [Test]
    public override void Part1()
    {
        for (var index = 1; index < lines.Count; index += 7)
        {
            _monkeys.Add(Monkey.Parse(lines.Skip(index).Take(5).ToList()));
        }

        for (var i = 0; i < 20; i++)
        {
            foreach (var monkey in _monkeys)
            {
                monkey.CheckItem(_monkeys);
            }
        }

        var maxChecks = _monkeys.OrderByDescending(x => x.NoCheck).Select(x => x.NoCheck).Take(2).ToList();

        Console.WriteLine(maxChecks[0] * maxChecks[1]);

        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        for (var index = 1; index < lines.Count; index += 7)
        {
            _monkeys.Add(Monkey.Parse(lines.Skip(index).Take(5).ToList()));
        }
        
        Monkey.LimitNumber = _monkeys.Aggregate<Monkey, long>(1, (current, mokey) => current * mokey.NoDivisible);

        for (var i = 0; i < 10000; i++)
        {
            foreach (var monkey in _monkeys)
            {
                monkey.CheckItem(_monkeys);
            }
        }

        var maxChecks = _monkeys.OrderByDescending(x => x.NoCheck).Select(x => x.NoCheck).Take(2).ToList();

        
        Console.WriteLine(new BigInteger(maxChecks[0]) * new BigInteger(maxChecks[1]));

        Assert.Pass();
    }

    public class Monkey
    {
        private IList<BigInteger> _items = null!;
        private Func<BigInteger, BigInteger> _operation = null!;
        public long NoDivisible;
        private int _testTrue;
        private int _testFail;

        public long NoCheck;

        public static long LimitNumber;

        public static Monkey Parse(IList<string> lines)
        {
            var items = lines[0].Split(new[] {':', ','}, StringSplitOptions.TrimEntries).Skip(1).Select(BigInteger.Parse).ToList();

            Func<BigInteger, BigInteger> operation;
            if (int.TryParse(lines[1].Split(' ').Last(), out var noOperation))
            {
                operation = lines[1].Contains('*') ? x => x * noOperation : x => x + noOperation;
            }
            else
            {
                operation = x => x * x;
            }

            return new Monkey
            {
                _items = items,
                _operation = operation,
                NoDivisible = long.Parse(lines[2].Split(' ').Last()),
                _testTrue = int.Parse(lines[3].Split(' ').Last()),
                _testFail = int.Parse(lines[4].Split(' ').Last()),
            };
        }


        public void CheckItem(IList<Monkey> monkeys)
        {
            while (_items.Count > 0)
            {
                NoCheck++;
                var newWorry = LimitNumber == 0 ? _operation(_items[0]) / 3 : _operation(_items[0]) % LimitNumber;
                if (newWorry % NoDivisible == 0)
                {
                    monkeys[_testTrue]._items.Add(newWorry);
                }
                else
                {
                    monkeys[_testFail]._items.Add(newWorry);
                }

                _items = _items.Skip(1).ToList();
            }
        }
    }
}