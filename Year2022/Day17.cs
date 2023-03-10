using NUnit.Framework;

namespace Year2022;

public class Day17 : BaseDayTest
{
    private const int MaxLineSaving = 100;
    private long _linesNotSaving;

    private readonly string[][] _rocks =
    {
        new[] {"####"},
        new[] {" # ", "###", " # "},
        new[] {"  #", "  #", "###"},
        new[] {"#", "#", "#", "#"},
        new[] {"##", "##"}
    };

    private string _wind = null!;
    private ModCounter _iRock;
    private ModCounter _iWind;
    private readonly List<char[]> _lines = new();

    [SetUp]
    public void ParseData()
    {
        _wind = lines[0];
        _iRock = new ModCounter(0, _rocks.Length);
        _iWind = new ModCounter(0, _wind.Length);
        _linesNotSaving = 0;
        _lines.Clear();
    }

    [Test]
    public override void Part1()
    {
        var height = AddRocks(2022);

        Console.WriteLine(height);
        Assert.That(height, Is.EqualTo(3184));
    }

    [Test]
    public override void Part2()
    {
        var height = AddRocks(1000000000000);

        Console.WriteLine(height);
        Assert.That(height, Is.EqualTo(1577077363915));
    }

    private long AddRocks(long rocksToAdd)
    {
        var seen = new Dictionary<string, (long rocksToAdd, long height)>();
        while (rocksToAdd > 0)
        {
            var hash = string.Join("", _lines.SelectMany(ch => ch));
            if (seen.TryGetValue(hash, out var cache))
            {
                var heightOfPeriod = _lines.Count + _linesNotSaving - cache.height;
                var periodLength = cache.rocksToAdd - rocksToAdd;
                _linesNotSaving += rocksToAdd / periodLength * heightOfPeriod;
                rocksToAdd %= periodLength;
                break;
            }

            seen[hash] = (rocksToAdd, _lines.Count + _linesNotSaving);
            AddRock();
            rocksToAdd--;
        }

        while (rocksToAdd > 0)
        {
            AddRock();
            rocksToAdd--;
        }

        return _lines.Count + _linesNotSaving;
    }

    private void AddRock()
    {
        var rock = _rocks[(int) _iRock++];

        for (var i = 0; i < rock.Length + 3; i++)
        {
            _lines.Insert(0, "|       |".ToArray());
        }

        var pos = new Pos(0, 3);
        while (true)
        {
            var jet = _wind[(int) _iWind++];
            pos = jet switch
            {
                '>' when !Hit(rock, pos.Right) => pos.Right,
                '<' when !Hit(rock, pos.Left) => pos.Left,
                _ => pos
            };

            if (Hit(rock, pos.Below))
            {
                break;
            }

            pos = pos.Below;
        }

        Draw(rock, pos);
    }

    private bool Hit(IReadOnlyList<string> rock, Pos pos)
    {
        return Area(rock).Any(pt => Get(rock, pt) == '#' && Get(_lines, pt + pos) != ' ');
    }

    private void Draw(IReadOnlyList<string> rock, Pos pos)
    {
        foreach (var pt in Area(rock))
        {
            if (Get(rock, pt) == '#')
            {
                Set(_lines, pt + pos, '#');
            }
        }

        while (!_lines[0].Contains('#'))
        {
            _lines.RemoveAt(0);
        }

        while (_lines.Count > MaxLineSaving)
        {
            _lines.RemoveAt(_lines.Count - 1);
            _linesNotSaving++;
        }
    }

    private static IEnumerable<Pos> Area(IReadOnlyList<string> mat)
    {
        for (var i = 0; i < mat.Count; i++)
        {
            for (var j = 0; j < mat[0].Length; j++)
            {
                yield return new Pos(i, j);
            }
        }
    }

    private static char Get(IEnumerable<IEnumerable<char>> mat, Pos pos)
    {
        return (mat.ElementAtOrDefault(pos.Row) ?? "#########").ElementAt(pos.Col);
    }

    private static void Set(IList<char[]> mat, Pos pos, char ch)
    {
        mat[pos.Row][pos.Col] = ch;
    }

    private readonly record struct Pos(int Row, int Col)
    {
        public Pos Left => this with {Col = Col - 1};
        public Pos Right => this with {Col = Col + 1};
        public Pos Below => this with {Row = Row + 1};

        public static Pos operator +(Pos posA, Pos posB) =>
            new(posA.Row + posB.Row, posA.Col + posB.Col);
    }

    private record struct ModCounter(int Index, int Mod)
    {
        public static explicit operator int(ModCounter c) => c.Index;

        public static ModCounter operator ++(ModCounter c) =>
            c with {Index = c.Index == c.Mod - 1 ? 0 : c.Index + 1};
    }
}