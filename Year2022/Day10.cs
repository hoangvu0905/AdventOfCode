using NUnit.Framework;

namespace Year2022;

public class Day10 : BaseDayTest
{
    private int _cycle = -1;
    private int _x = 1;
    private readonly IList<long> _results = new List<long>();
    private readonly bool[,] _light = new bool[6, 40];

    [Test]
    public override void Part1()
    {
        foreach (var line in lines)
        {
            Handle(line);
        }

        Console.WriteLine(_results.Sum());
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        foreach (var line in lines)
        {
            Handle(line);
        }

        Print(_light);

        Assert.Pass();
    }

    private void Handle(string line)
    {
        if (line.StartsWith("noop"))
        {
            _cycle++;
            HandleCycleAndDraw();
        }
        else
        {
            _cycle++;
            HandleCycleAndDraw();
            _cycle++;
            HandleCycleAndDraw();
            _x += int.Parse(line[5..]);
        }
    }

    private void HandleCycleAndDraw()
    {
        if ((_cycle - 20) % 40 == 0)
        {
            _results.Add(_cycle * _x);
        }
        
        var y = _cycle % 40;
        if (Math.Abs(_x - y) > 1) return;
        var row = _cycle / 40;
        _light[row, y] = true;
    }
}