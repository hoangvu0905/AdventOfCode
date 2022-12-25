using System.Numerics;
using NUnit.Framework;

namespace Year2022;

public class Day23 : BaseDayTest
{
    private readonly HashSet<Complex> _elves = new();

    private static readonly Complex N = new(0, -1);
    private static readonly Complex E = new(1, 0);
    private static readonly Complex S = new(0, 1);
    private static readonly Complex W = new(-1, 0);
    private static readonly Complex NW = N + W;
    private static readonly Complex NE = N + E;
    private static readonly Complex SE = S + E;
    private static readonly Complex SW = S + W;
    private static readonly Complex[] Directions = {NW, N, NE, E, SE, S, SW, W};

    [Test]
    public override void Part1()
    {
        Console.WriteLine(Simulate(_elves).Select(FindArea).ElementAt(9));
    }

    [Test]
    public override void Part2()
    {
        Console.WriteLine(Simulate(_elves).Count());
    }

    [SetUp]
    public void ParseData()
    {
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '#')
                {
                    _elves.Add(new Complex(j, i));
                }
            }
        }
    }

    private static IEnumerable<HashSet<Complex>> Simulate(HashSet<Complex> elves)
    {
        var lookAround = new Queue<Complex>(new[] {N, S, W, E});

        for (var fixpoint = false; !fixpoint; lookAround.Enqueue(lookAround.Dequeue()))
        {
            var proposals = new Dictionary<Complex, List<Complex>>();

            // Find all position elf can move
            foreach (var elf in elves)
            {
                // if elf lonely, do nothing.
                if (Directions.All(dir => !elves.Contains(elf + dir)))
                {
                    continue;
                }

                foreach (var dir in lookAround)
                {
                    // Continue if cannot move it
                    if (ExtendDir(dir).Any(d => elves.Contains(elf + d))) continue;

                    // add position
                    var pos = elf + dir;
                    if (!proposals.ContainsKey(pos))
                    {
                        proposals[pos] = new List<Complex>();
                    }

                    proposals[pos].Add(elf);
                    break;
                }
            }

            // real move.
            fixpoint = true;
            foreach (var p in proposals)
            {
                var (to, from) = p;
                if (from.Count != 1) continue;
                elves.Remove(from.Single());
                elves.Add(to);
                fixpoint = false;
            }

            yield return elves;
        }
    }

    private static double FindArea(HashSet<Complex> elves)
    {
        var width = elves.Select(p => p.Real).Max() -
            elves.Select(p => p.Real).Min() + 1;

        var height = elves.Select(p => p.Imaginary).Max() -
            elves.Select(p => p.Imaginary).Min() + 1;

        return width * height - elves.Count;
    }

    private static IEnumerable<Complex> ExtendDir(Complex dir)
    {
        if (dir == N) return new[] {NW, N, NE};
        if (dir == E) return new[] {NE, E, SE};
        if (dir == S) return new[] {SW, S, SE};
        if (dir == W) return new[] {NW, W, SW};
        throw new Exception();
    }
}