using NUnit.Framework;

namespace Year2022;

public class Day24 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        var (entry, exit, maps) = Parse(string.Join('\n', lines));
        var result = WalkTo(entry, exit, maps).Time;
        Console.WriteLine(result);
        Assert.That(result, Is.EqualTo(245));

    }

    [Test]
    public override void Part2()
    {
        var (entry, exit, maps) = Parse(string.Join('\n', lines));
        var pos = WalkTo(entry, exit, maps);
        pos = WalkTo(pos, entry, maps);
        pos = WalkTo(pos, exit, maps);

        Console.WriteLine(pos.Time);
        Assert.That(pos.Time, Is.EqualTo(798));
    }

    private Pos WalkTo(Pos start, Pos goal, Maps maps)
    {
        var q = new PriorityQueue<Pos, int>();

        int F(Pos pos)
        {
            var dist =
                Math.Abs(goal.Irow - pos.Irow) +
                Math.Abs(goal.Icol - pos.Icol);
            return pos.Time + dist;
        }

        q.Enqueue(start, F(start));
        var seen = new HashSet<Pos>();

        while (q.Count > 0)
        {
            var pos = q.Dequeue();
            if (pos.Irow == goal.Irow && pos.Icol == goal.Icol)
            {
                return pos;
            }

            foreach (var nextPos in NextPositions(pos, maps).Where(nextPos => !seen.Contains(nextPos)))
            {
                seen.Add(nextPos);
                q.Enqueue(nextPos, F(nextPos));
            }
        }

        throw new Exception();
    }

    // Increase time, look for free neighbours
    private IEnumerable<Pos> NextPositions(Pos pos, Maps maps)
    {
        pos = pos with {Time = pos.Time + 1};
        foreach (var nextPos in new Pos[]
                 {
                     pos,
                     pos with {Irow = pos.Irow - 1},
                     pos with {Irow = pos.Irow + 1},
                     pos with {Icol = pos.Icol - 1},
                     pos with {Icol = pos.Icol + 1},
                 })
        {
            if (maps.Get(nextPos) == '.')
            {
                yield return nextPos;
            }
        }
    }

    private record Pos(int Time, int Irow, int Icol);

    private static (Pos entry, Pos exit, Maps maps) Parse(string input)
    {
        var maps = new Maps(input);
        var entry = new Pos(0, 0, 1);
        var exit = new Pos(int.MaxValue, maps.Crow - 1, maps.Ccol - 2);
        return (entry, exit, maps);
    }

    private class Maps
    {
        private readonly string[] map;
        public readonly int Crow;
        public readonly int Ccol;

        public Maps(string input)
        {
            map = input.Split("\n");
            this.Crow = map.Length;
            this.Ccol = map[0].Length;
        }

        public char Get(Pos pos)
        {
            if (pos.Irow == 0 && pos.Icol == 1)
            {
                return '.';
            }

            if (pos.Irow == Crow - 1 && pos.Icol == Ccol - 2)
            {
                return '.';
            }

            if (pos.Irow <= 0 || pos.Irow >= Crow - 1 ||
                pos.Icol <= 0 || pos.Icol >= Ccol - 1
               )
            {
                return '#';
            }

            var hmod = Ccol - 2;
            var vmod = Crow - 2;

            var icolW = (pos.Icol - 1 + hmod - pos.Time % hmod) % hmod + 1;
            var icolE = (pos.Icol - 1 + hmod + pos.Time % hmod) % hmod + 1;
            var icolN = (pos.Irow - 1 + vmod - pos.Time % vmod) % vmod + 1;
            var icolS = (pos.Irow - 1 + vmod + pos.Time % vmod) % vmod + 1;

            return
                map[pos.Irow][icolW] == '>' ? '>' :
                map[pos.Irow][icolE] == '<' ? '<' :
                map[icolN][pos.Icol] == 'v' ? 'v' :
                map[icolS][pos.Icol] == '^' ? '^' :
                '.';
        }
    }
}