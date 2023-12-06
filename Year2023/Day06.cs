using NUnit.Framework;

namespace Year2023;

public class Day06 : BaseDayTest
{
    private readonly IList<Game> _games = new List<Game>();
    
    [OneTimeSetUp]
    public void ParseData()
    {
        var times = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var distances = lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        for (int i = 0; i < times.Count; i++)
        {
            _games.Add(new Game(times[i], distances[i]));
        }
    }
    
    [Test]
    public override void Part1()
    {
        var result = 1L;

        foreach (var game in _games)
        {
            var temp = game.FindMaxTimeNeedToWin() - game.FindMinTimeNeedToWin() + 1;
            result *= temp;
            Console.WriteLine(temp);
        }

        Console.WriteLine(result);
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        var time = long.Parse(string.Join("", _games.Select(x => x.Time)));
        var distance = long.Parse(string.Join("", _games.Select(x => x.Distance)));

        var game = new Game(time, distance);
        var min = game.FindMinTimeNeedToWin();
        var max = game.FindMaxTimeNeedToWin();

        Console.WriteLine(max - min + 1);
        Assert.Pass();
    }

    public record Game(long Time, long Distance)
    {
        public long FindMinTimeNeedToWin()
        {
            var midTime = (Time / 2) + 1;
            for (long i = 1; i < midTime; i++)
            {
                var distance = CountDistance(i);
                if (CountDistance(i) > Distance)
                {
                    return i;
                }
            }

            return 0;
        }
        
        public long FindMaxTimeNeedToWin()
        {
            var midTime = Time / 2;
            for (var i = Time-1; i > midTime; i--)
            {
                if (CountDistance(i) > Distance)
                {
                    return i;
                }
            }
            return 0;
        }

        private long CountDistance(long time)
        {
            return time * (Time - time);
        }
    }
}