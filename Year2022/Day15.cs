using NUnit.Framework;

namespace Year2022;

public class Day15 : BaseDayTest
{
    private readonly IList<Circle> _circles = new List<Circle>();
    private readonly IList<Point> _beacons = new List<Point>();
    private const int LineCount = 2000000;
    private readonly IList<long> _posLines = new List<long>();
    private readonly IList<long> _nesLines = new List<long>();

    [Test]
    public override void Part1()
    {
        foreach (var line in lines)
        {
            var numbers = line.Split('=', ',', ':')
                .Where(x => int.TryParse(x, out _)).Select(int.Parse)
                .ToList();
            _beacons.Add(new Point(numbers[2], numbers[3]));
            _circles.Add(new Circle(numbers[0], numbers[1], numbers[2], numbers[3]));
        }

        Console.WriteLine(CountNoBeacons());
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        foreach (var line in lines)
        {
            var numbers = line.Split('=', ',', ':')
                .Where(x => int.TryParse(x, out _)).Select(int.Parse)
                .ToList();
            var circle = new Circle(numbers[0], numbers[1], numbers[2], numbers[3]);
            _circles.Add(circle);
        }

        Console.WriteLine(FindBeacons());
        Assert.Pass();
    }

    private void FindIntersectionPoint()
    {
        foreach (var circle in _circles)
        {
            _posLines.Add(circle.X + circle.Y - circle.R);
            _posLines.Add(circle.X + circle.Y + circle.R);
            _nesLines.Add(circle.X - circle.Y - circle.R);
            _nesLines.Add(circle.X - circle.Y + circle.R);
        }
    }

    private long FindBeacons()
    {
        FindIntersectionPoint();

        var pos = 0L;
        var nes = 0L;
        
        for (var i = 0; i < _circles.Count * 2 - 1; i++)
        {
            for (var j = i+1; j < _circles.Count * 2; j++)
            {
                if (Math.Abs(_posLines[i] - _posLines[j]) == 2)
                {
                    pos = Math.Min(_posLines[i], _posLines[j]) + 1;
                }
                
                if (Math.Abs(_nesLines[i] - _nesLines[j]) == 2)
                {
                    nes = Math.Min(_nesLines[i], _nesLines[j]) + 1;
                }
            }
        }

        var x = Math.Abs(nes + pos) / 2;
        var y = Math.Abs(nes - pos) / 2;

        var ans = x * 4000000 + y;
        return ans;
    }

    private long CountNoBeacons()
    {
        var minY = _circles.Select(x => x.Y - x.R).Min();
        var maxY = _circles.Select(x => x.Y + x.R).Max();

        var result = 0;

        for (var i = minY; i <= maxY; i++)
        {
            if (CheckLine(i)) result++;
        }

        return result;
    }

    private bool CheckLine(int index)
    {
        if (_beacons.Any(x => x.X == index && x.Y == LineCount)) return false;
        if (_circles.Any(x => x.X == index && x.Y == LineCount)) return false;
        return _circles.Any(circle => CalcManhattan(circle.X, circle.Y, index, LineCount) <= circle.R);
    }

    private class Circle : Point
    {
        public Circle(int x, int y, int x2, int y2): base(x,y)
        {
            R = CalcManhattan(x, y, x2, y2);
        }

        public int R { get; set; }

        public override string ToString()
        {
            return $"{{X = {X}; Y = {Y}; R = {R} }}";
        }
    }
}