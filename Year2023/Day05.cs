using NUnit.Framework;

namespace Year2023;

public class Day05 : BaseDayTest
{
    private long[] Seeds;
    private readonly IList<IList<Map>> Maps = new List<IList<Map>>();
    
    [OneTimeSetUp]
    public void ParseData()
    {
        Seeds = lines[0][6..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        var currentMaps = new List<Map>();

        for (var i = 1; i < lines.Count; i++)
        {
            var line = lines[i];
            if (string.IsNullOrEmpty(line))
            {
                currentMaps = new List<Map>();
                Maps.Add(currentMaps);
                i++;
                continue;
            }
            
            var numbers = line.Split(' ').Select(long.Parse).ToArray();
            currentMaps.Add(new Map(numbers[0], numbers[1], numbers[2]));
        }
    }
    
    [Test]
    public override void Part1()
    {
        var seeds = Seeds.Select(x => new Seed(x, x));

        var currentSeeds = Maps.Aggregate(seeds, (current, map) => FindAllSeedForMap(map, current.ToArray()).ToList());

        Console.WriteLine(currentSeeds.Min(x => x.From));
    }

    [Test]
    public override void Part2()
    {
        var seeds = new List<Seed>();
        for (var i = 0; i < Seeds.Length; i+=2)
        {
            seeds.Add(new Seed(Seeds[i], Seeds[i] + Seeds[i + 1]));
        }

        var currentSeeds = Maps.Aggregate(seeds, (current, map) => FindAllSeedForMap(map, current.ToArray()).ToList());

        Console.WriteLine(currentSeeds.Min(x => x.From));
    }

    private static IList<Seed> FindAllSeedForMap(IList<Map> maps, params Seed[] seeds)
    {
        var todo = new Queue<Seed>(seeds);
        var outputRanges = new List<Seed>();

        while (todo.Any())
        {
            var seed = todo.Dequeue();
            var map = maps.FirstOrDefault(x => x.Intersects(seed));
            // We have 3 cases:
            // 1. No map intersects the seed -> just add it to the output.
            // 2. An entry completely contains the seed -> add new seed map to the output.
            // 3. Some entry partly covers the seed.
            // In this case 'chop' the seed into two halfs getting rid of the intersection.
            // The new pieces are added back to the queue for further processing and will be ultimately consumed by the first two cases. 
            if (map is null)
            {
                outputRanges.Add(seed);
            }
            else if (seed.From >= map.Source && seed.To < map.Source + map.Range)
            {
                outputRanges.Add(map.GetMapSeed(seed));
            }else if (seed.From < map.Source)
            {
                todo.Enqueue(new Seed(seed.From, map.Source - 1));
                todo.Enqueue(new Seed(map.Source, seed.To));
            }
            else
            {
                todo.Enqueue(new Seed(seed.From, map.Source + map.Range - 1));
                todo.Enqueue(new Seed(map.Source + map.Range, seed.To));
            }
        }

        return outputRanges;
    }


    private record Map(long Destination, long Source, long Range)
    {
        public bool Intersects(Seed seed)
        {
            return Source <= seed.To && seed.From <= Source + Range - 1;
        }

        public Seed GetMapSeed(Seed seed)
        {
            return new Seed(Destination + seed.From - Source, Destination + seed.To - Source - 1);
        }
    }

    private class Seed
    {
        public long From { get; }
        public long To { get; }
        
        public Seed(long from, long to)
        {
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return $"({From}, {To})";
        }
    }
}