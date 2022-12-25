using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Year2022;

public class Day22 : BaseDayTest
{
    /// <summary>
    /// ' ' => 0
    /// '.' => 1
    /// '#' => 2
    /// => 3; end line.
    /// </summary>
    private int[,] _grid = null!;

    private string[] moves = null!;

    private readonly Player _player = new() {X = 0, Facing = Face.Right};

    [Test]
    public override void Part1()
    {
        foreach (var stepMove in moves)
        {
            if (int.TryParse(stepMove, out var numberStep))
            {
                MoveStep(numberStep);
            }
            else
            {
                Turn(stepMove);
            }
        }

        Console.WriteLine(CountResult());
        Assert.That(CountResult(), Is.EqualTo(182170));
    }

    [Test]
    public override void Part2()
    {
        foreach (var stepMove in moves)
        {
            if (int.TryParse(stepMove, out var numberStep))
            {
                MoveStep(numberStep);
            }
            else
            {
                Turn(stepMove);
            }
        }

        Console.WriteLine(CountResult());
        Assert.That(CountResult(), Is.EqualTo(182170));
    }

    private bool CalcNextCube()
    {
        var player = _player switch
        {
            // Check Left
            // Left A -> Right D
            {Y: 50, X: < 50} => new Player {X = 149 - _player.X, Y = 0, Facing = Face.Right},
            // Left C -> Down D
            {Y: 50, X: >= 50} => new Player {X = 100, Y = _player.X - 50, Facing = Face.Down},
            // Left D - > Right A
            {Y: 0, X: < 150} => new Player {X = 149 - _player.X, Y = 50, Facing = Face.Right},
            // Left F -> Down A
            {Y: 0, X: >= 150} => new Player {X = 0, Y = _player.X - 100, Facing = Face.Down},
            // Right
            // Right B -> Left E
            {Y: 149, X: < 50} => new Player {X = 149 - _player.X, Y = 99, Facing = Face.Left},
            // Right C -> Up B
            {Y: 99, X: < 100} => new Player {X = 49, Y = _player.X + 50, Facing = Face.Up},
            // Right E -> Left B
            {Y: 99, X: < 150} => new Player {X = 149 - _player.X, Y = 149, Facing = Face.Left},
            // Right F -> Up E
            {Y: 49, X: >= 150} => new Player {X = 149, Y = _player.X - 100, Facing = Face.Up},
            // Up
            // Up A -> Right F
            {X: 0, Y: < 100} => new Player {X = _player.Y + 100, Y = 0, Facing = Face.Right},
            // Up B -> Up F
            {X: 0, Y: >= 100} => new Player {X = 199, Y = _player.Y - 100, Facing = Face.Up},
            // Up D -> Right C
            {X: 100, Y: < 50} => new Player {X = _player.Y + 50, Y = 50, Facing = Face.Right},
            // Down 
            // Down B -> Left C
            {X: 49} => new Player {X = _player.Y - 50, Y = 99, Facing = Face.Left},
            // Down E -> Left F
            {X: 149} => new Player {X = _player.Y + 100, Y = 49, Facing = Face.Left},
            // Down F -> Down B
            {X: 199} => new Player {X = 0, Y = _player.Y + 100, Facing = Face.Down},
            _ => throw new ArgumentOutOfRangeException(nameof(Player), "Out range X, Y Cube")
        };
        
        if (_grid[player.X, player.Y] == 2) return false;
        if (_grid[player.X, player.Y] != 1 )
        {
            throw new Exception($"{_player}\t{player}");
        }
        _player.X = player.X;
        _player.Y = player.Y;
        _player.Facing = player.Facing;
        return true;

    }

    private int CountResult()
    {
        return 1000 * (_player.X + 1) + 4 * (_player.Y + 1) + (int) _player.Facing;
    }

    private void MoveStep(int numberStep)
    {
        for (var i = 0; i < numberStep; i++)
        {
            if (!MoveStep()) break;
        }
    }

    private bool MoveStep()
    {
        switch (_player.Facing)
        {
            case Face.Right:
                switch (_grid[_player.X, _player.Y + 1])
                {
                    case 1:
                        _player.Y++;
                        return true;
                    case 3 or 0:
                        return CalcNextCube();
                }

                break;
            case Face.Down:
            {
                var cell = _grid[_player.X + 1, _player.Y];
                switch (cell)
                {
                    case 1:
                        _player.X++;
                        return true;
                    case 3 or 0:
                        return CalcNextCube();
                }

                break;
            }
            case Face.Left:
            {
                if (_player.Y > 0)
                {
                    switch (_grid[_player.X, _player.Y - 1])
                    {
                        case 1:
                            _player.Y--;
                            return true;
                        case 2:
                            return false;
                    }
                }

                return CalcNextCube();
            }
            case Face.Up:
            {
                if (_player.X > 0)
                {
                    switch (_grid[_player.X - 1, _player.Y])
                    {
                        case 1:
                            _player.X--;
                            return true;
                        case 2:
                            return false;
                    }
                }

                return CalcNextCube();
            }
        }

        return false;
    }

    private void Turn(string direction)
    {
        _player.Facing = (Face) (((int) _player.Facing + (direction == "R" ? 1 : -1) + 4) % 4);
    }

    [OneTimeSetUp]
    public void ParseData()
    {
        var maxLine = lines.SkipLast(1).Max(x => x.Length) + 1;

        _grid = new int[lines.Count - 1, maxLine];
        for (var i = 0; i < _grid.GetLongLength(0) - 1; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                if (_player.Y == 0 && lines[i][j] == '.')
                    _player.Y = j;

                if (lines[i][j] == ' ')
                {
                    _grid[i, j] = 0;
                }
                else
                {
                    _grid[i, j] = lines[i][j] == '.' ? 1 : 2;
                }
            }

            _grid[i, lines[i].Length] = 3;
        }

        var lastX = lines.Count - 2;

        for (var i = 0; i < _grid.GetLongLength(1); i++)
        {
            _grid[lastX, i] = 3;
        }

        moves = new Regex(@"(\?|\d+|\w)").Matches(lines.Last()).Select(x => x.Groups[1].Value).ToArray();
    }

    private enum Face
    {
        Right,
        Down,
        Left,
        Up,
    }

    private class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Face Facing { get; set; }

        public override string ToString()
        {
            return $"X = {X}; Y = {Y}; Facing = {Facing.ToString()}";
        }
    }
}