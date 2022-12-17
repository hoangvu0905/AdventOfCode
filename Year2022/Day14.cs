using NUnit.Framework;

namespace Year2022;

public class Day14 : BaseDayTest
{
    private int _maxHose;
    private int _countStep;
    private readonly bool[,] _grid = new bool[200, 1000];

    [Test]
    public override void Part1()
    {
        foreach (var line in lines)
        {
            var parts = line.Split("->", StringSplitOptions.TrimEntries);
            var points = parts.Select(x =>
            {
                var numbers = x.Split(',').Select(int.Parse).ToArray();
                return new Point(numbers[0], numbers[1]);
            }).ToList();

            BuildSandTank(points);
        }

        Console.WriteLine(StartStand());

        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        foreach (var line in lines)
        {
            var parts = line.Split("->", StringSplitOptions.TrimEntries);
            var points = parts.Select(x =>
            {
                var numbers = x.Split(',').Select(int.Parse).ToArray();
                return new Point(numbers[0], numbers[1]);
            }).ToList();

            BuildSandTank(points);
        }

        Console.WriteLine(StartStand2());

        Assert.Pass();
    }

    private int StartStand()
    {
        while (true)
        {
            int x = 0, y = 500; // Start point of stand
            while (true)
            {
                if (x > _maxHose) return _countStep;
                x++;
                if (!_grid[x,y])
                {
                    // Do nothing, stand will be move strange
                }else if (!_grid[x, Math.Max(y - 1, 0)])
                {
                    y--;
                }else if (!_grid[x, y + 1])
                {
                    y++;
                }
                else
                {
                    _grid[x - 1, y] = true;
                    break;
                }
            }

            _countStep++;
        }
    }

    private int StartStand2()
    {
        while (true)
        {
            int x = 0, y = 500; // Start point of stand
            while (true)
            {
                x++;
                if (!_grid[x,y])
                {
                    // Do nothing, stand will be move strange
                }else if (!_grid[x, Math.Max(y - 1, 0)])
                {
                    y--;
                }else if (!_grid[x, y + 1])
                {
                    y++;
                }
                else
                {
                    _grid[x - 1, y] = true;
                    break;
                }
            }

            _countStep++;
            if (_grid[0, 500])
            {
                return _countStep;
            }
        }
    }
    
    private void BuildSandTank(IList<Point> points)
    {
        for (var i = 0; i < points.Count - 1; i++)
        {
            BuildStepTank(points[i], points[i + 1]);
        }
    }

    private void BuildStepTank(Point p1, Point p2)
    {
        _maxHose = Math.Max(_maxHose, Math.Max(p1.Y, p2.Y));
        for (var i = Math.Min(p1.X, p2.X); i <= Math.Max(p1.X, p2.X); i++)
        {
            for (var j = Math.Min(p1.Y, p2.Y); j <= Math.Max(p1.Y, p2.Y); j++)
            {
                _grid[j, i] = true;
            }
        }
    }
    
}