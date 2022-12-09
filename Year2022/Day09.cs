using NUnit.Framework;

namespace Year2022;

public class Day09 : BaseDayTest
{
    private const int Size = 500;
    private readonly bool[,] _moved = new bool[Size * 2, Size * 2];
    private readonly IList<Point> _snack = new List<Point>();

    [Test]
    public override void Part1()
    {
        _snack.Add(new Point(Size, Size));
        _snack.Add(new Point(Size, Size));

        _moved[_snack[0].X, _snack[0].Y] = true;
        foreach (var line in lines)
        {
            var numberMove = int.Parse(line[2..]);
            for (var i = 0; i < numberMove; i++)
            {
                MoveHead(line[0]);
                CheckEnd(_snack[0], _snack[1], true);
            }
        }

        Console.WriteLine(CountCellMoved());
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        for (var i = 0; i < 10; i++) _snack.Add(new Point(Size, Size));

        _moved[_snack[0].X, _snack[0].Y] = true;
        foreach (var line in lines)
        {
            var numberMove = int.Parse(line[2..]);
            for (var i = 0; i < numberMove; i++)
            {
                MoveHead(line[0]);

                for (var j = 0; j < 9; j++)
                {
                    CheckEnd(_snack[j], _snack[j + 1], j == 8);
                }
            }
        }

        Console.WriteLine(CountCellMoved());
    }

    private int CountCellMoved()
    {
        var count = 0;
        for (var i = 0; i < _moved.GetLongLength(0); i++)
        {
            for (var j = 0; j < _moved.GetLongLength(1); j++)
            {
                if (_moved[i, j]) count++;
            }
        }

        return count;
    }

    private void MoveHead(char direction)
    {
        switch (direction)
        {
            case 'R':
                _snack[0].Y++;
                break;
            case 'U':
                _snack[0].X--;
                break;
            case 'L':
                _snack[0].Y--;
                break;
            case 'D':
                _snack[0].X++;
                break;
        }
    }

    private void CheckEnd(Point head, Point end, bool addMoved = false)
    {
        if (Math.Sqrt(Math.Pow(head.X - end.X, 2) + Math.Pow(head.Y - end.Y, 2)) < 2)
        {
            return;
        }

        if (Math.Abs(head.X - end.X) == 2)
        {
            end.Y += (int) Math.Round((double) (head.Y - end.Y) / 2, MidpointRounding.AwayFromZero);
        }
        else if (Math.Abs(head.Y - end.Y) == 2)
        {
            end.X += (int) Math.Round((double) (head.X - end.X) / 2, MidpointRounding.AwayFromZero);
        }

        // Move with a straight line
        if (Math.Abs(head.Y - end.Y) == 2)
        {
            end.Y += (head.Y - end.Y) / 2;
        }

        if (Math.Abs(head.X - end.X) == 2)
        {
            end.X += (head.X - end.X) / 2;
        }

        if (addMoved) _moved[end.X, end.Y] = true;
    }
}