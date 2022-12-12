using NUnit.Framework;

namespace Year2022;

public class Day12 : BaseDayTest
{
    private Point _start = null!;
    private Point _end = null!;
    private int[,] _grid = null!;
    private bool[,] _moved = null!;
    private bool _findEnd = true;

    private Point? _found;

    private readonly Queue<Point> _stack = new();

    private long Row => _grid.GetLongLength(0);
    private long Column => _grid.GetLongLength(1);

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _grid = new int[lines.Count, lines[0].Length];
        _moved = new bool[lines.Count, lines[0].Length];
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                switch (lines[i][j])
                {
                    case 'S':
                        _start = new Point(i, j);
                        _grid[i, j] = 0;
                        _moved[i, j] = true;
                        break;
                    case 'E':
                        _end = new Point(i, j);
                        _grid[i, j] = 'z' - 'a';
                        break;
                    default:
                        _grid[i, j] = lines[i][j] - 'a';
                        break;
                }
            }
        }
    }

    [Test]
    public override void Part1()
    {
        _findEnd = true;
        _stack.Enqueue(_start);

        while (_stack.Count > 0 && _found is null)
        {
            NextMove(_stack.Dequeue());
        }

        Console.WriteLine(CountStepMove(_found!));

        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        _findEnd = false;
        _stack.Enqueue(_end);

        while (_stack.Count > 0 && _found is null)
        {
            NextMove(_stack.Dequeue());
        }

        Console.WriteLine(CountStepMove(_found!));

        Assert.Pass();
    }

    private static int CountStepMove(Point found)
    {
        var count = 0;
        var current = found;
        do
        {
            count++;
            current = current.Parent;
        } while (current is not null);

        // step move = node -1
        return count - 1;
    }

    private void NextMove(Point point)
    {
        if (_grid[point.X, point.Y] == (_findEnd ? 'z' : 'a') - 'a')
        {
            _found = point;
            return;
        }

        CanMove(point, new Point(point.X - 1, point.Y));
        CanMove(point, new Point(point.X + 1, point.Y));
        CanMove(point, new Point(point.X, point.Y + 1));
        CanMove(point, new Point(point.X, point.Y - 1));
    }

    private void CanMove(Point point, Point nextPoint)
    {
        if (nextPoint.X < 0 || nextPoint.Y < 0 || nextPoint.X >= Row || nextPoint.Y >= Column)
        {
            return;
        }

        if (_moved[nextPoint.X, nextPoint.Y])
        {
            return;
        }

        switch (_findEnd)
        {
            case true when _grid[nextPoint.X, nextPoint.Y] <= _grid[point.X, point.Y] + 1:
                _moved[nextPoint.X, nextPoint.Y] = true;
                nextPoint.Parent = point;
                _stack.Enqueue(nextPoint);
                break;
            case false when _grid[nextPoint.X, nextPoint.Y] >= _grid[point.X, point.Y] - 1:
                _moved[nextPoint.X, nextPoint.Y] = true;
                nextPoint.Parent = point;
                _stack.Enqueue(nextPoint);
                break;
        }
    }
}