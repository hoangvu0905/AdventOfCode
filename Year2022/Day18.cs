using NUnit.Framework;

namespace Year2022;

public class Day18 : BaseDayTest
{
    private IList<Cube> _cubes = new List<Cube>();

    [Test]
    public override void Part1()
    {
        var result = _cubes.Count * 6;
        for (var i = 0; i < _cubes.Count - 1; i++)
        {
            for (var j = i + 1; j < _cubes.Count; j++)
            {
                if (CubeContiguous(_cubes[i], _cubes[j]))
                {
                    result -= 2;
                }
            }
        }

        Console.WriteLine(result);
    }

    [Test]
    public override void Part2()
    {
        var minWater = new Cube(_cubes.Min(x => x.X) - 1, _cubes.Min(x => x.Y) - 1, _cubes.Min(x => x.Z) - 1);
        var maxWater = new Cube(_cubes.Max(x => x.X) + 1, _cubes.Max(x => x.Y) + 1, _cubes.Max(x => x.Z) + 1);

        var q = new Queue<Cube>();
        var cache = new HashSet<Cube>();
        q.Enqueue(minWater);

        while (q.Any())
        {
            var water = q.Dequeue();
            foreach (var neighbour in Neighbours(water))
            {
                if (cache.Contains(neighbour) || !Inside(minWater, maxWater, neighbour) || _cubes.Contains(neighbour)) continue;
                q.Enqueue(neighbour);
                cache.Add(neighbour);
            }
        }

        var result = _cubes.SelectMany(Neighbours).Count(x => cache.Contains(x));

        Console.WriteLine(result);
    }

    private static bool CubeContiguous(Cube cube1, Cube cube2)
    {
        if (cube1.X == cube2.X && cube1.Y == cube2.Y && Math.Abs(cube1.Z - cube2.Z) == 1)
        {
            return true;
        }

        if (cube1.X == cube2.X && cube1.Z == cube2.Z && Math.Abs(cube1.Y - cube2.Y) == 1)
        {
            return true;
        }

        if (cube1.Z == cube2.Z && cube1.Y == cube2.Y && Math.Abs(cube1.X - cube2.X) == 1)
        {
            return true;
        }

        return false;
    }

    private static bool Inside(Cube min, Cube max, Cube cube)
    {
        return min.X <= cube.X && cube.X <= max.X &&
               min.Y <= cube.Y && cube.Y <= max.Y &&
               min.Z <= cube.Z && cube.Z <= max.Z;
    }

    private static Cube[] Neighbours(Cube cube)
    {
        return new[]
        {
            cube with {X = cube.X - 1},
            cube with {X = cube.X + 1},
            cube with {Y = cube.Y - 1},
            cube with {Y = cube.Y + 1},
            cube with {Z = cube.Z - 1},
            cube with {Z = cube.Z + 1},
        };
    }

    [OneTimeSetUp]
    public void ParseData()
    {
        foreach (var line in lines)
        {
            var numbers = line.Split(',').Select(int.Parse).ToArray();
            _cubes.Add(new Cube(numbers[0], numbers[1], numbers[2]));
        }
    }

    private record Cube(int X, int Y, int Z);
}