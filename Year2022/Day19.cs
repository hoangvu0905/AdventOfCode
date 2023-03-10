using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Year2022;

public class Day19 : BaseDayTest
{
    private readonly IList<Blueprint> _bluePrints = new List<Blueprint>();

    [Test]
    public override void Part1()
    {
        var result = _bluePrints.Sum(x => MaxGeodes(x, 24) * x.Id);

        Console.WriteLine(result);
        Assert.That(result, Is.EqualTo(1962));
    }

    [Test]
    public override void Part2()
    {
        var result = _bluePrints.Take(3).Aggregate(1, (current, blueprint) => current * MaxGeodes(blueprint, 32));

        Console.WriteLine(result);
        Assert.That(result, Is.EqualTo(88160));
    }

    [OneTimeSetUp]
    public void ParseData()
    {
        foreach (var line in lines)
        {
            var numbers = Regex.Matches(line, @"(\d+)").Select(x => int.Parse(x.Value)).ToArray();
            _bluePrints.Add(new Blueprint(numbers[0],
                new Robot(0b1, numbers[1] * Ore, Ore),
                new Robot(0b10, numbers[2] * Ore, Clay),
                new Robot(0b100, numbers[3] * Ore + numbers[4] * Clay, Obsidian),
                new Robot(0b1000, numbers[5] * Ore + numbers[6] * Obsidian, Geode)
            ));
        }
    }

    private static int MaxGeodes(Blueprint blueprint, int timeLimit)
    {
        var q = new PriorityQueue<State, int>();
        var seen = new HashSet<State>();

        Enqueue(q, new State(
            RemainingTime: timeLimit,
            Available: Nothing,
            Producing: Ore,
            DontBuild: 0
        ));

        var max = 0;
        while (q.Count > 0)
        {
            var state = q.Dequeue();

            if (PotentialGeodeCount(state) < max) break;

            if (seen.Contains(state)) continue;
            seen.Add(state);

            if (state.RemainingTime == 0)
            {
                max = Math.Max(max, state.Available.Geode);
                continue;
            }

            var buildableRobots = blueprint.Robots
                .Where(robot => state.Available >= robot.Cost)
                .ToArray();

            foreach (var robot in buildableRobots)
            {
                if (WorthBuilding(blueprint, state, robot))
                {
                    Enqueue(q, new State(RemainingTime: state.RemainingTime - 1,
                        Available: state.Available + state.Producing - robot.Cost,
                        Producing: state.Producing + robot.Producing, DontBuild: 0));
                }
            }

            Enqueue(q, 
                state with
                {
                    RemainingTime = state.RemainingTime - 1,
                    Available = state.Available + state.Producing,
                    DontBuild = buildableRobots.Select(robot => robot.Id).Sum(),
                }
            );
        }

        return max;
    }

    private static int PotentialGeodeCount(State state)
    {
        var future = (2 * state.Producing.Geode + state.RemainingTime - 1) * state.RemainingTime / 2;
        return state.Available.Geode + future;
    }

    private static bool WorthBuilding(Blueprint blueprint, State state, Robot robot)
    {
        if ((state.DontBuild & robot.Id) != 0)
        {
            return false;
        }

        return state.Producing + robot.Producing <= blueprint.MaxCost;
    }

    private static void Enqueue(PriorityQueue<State, int> q, State state) => q.Enqueue(state, -PotentialGeodeCount(state));

    private static readonly Material Nothing = new(0, 0, 0, 0);
    private static readonly Material Ore = new(1, 0, 0, 0);
    private static readonly Material Clay = new(0, 1, 0, 0);
    private static readonly Material Obsidian = new(0, 0, 1, 0);
    private static readonly Material Geode = new(0, 0, 0, 1);

    private record Material(int Ore, int Clay, int Obsidian, int Geode)
    {
        public static Material operator *(int m, Material a)
        {
            return new Material(m * a.Ore, m * a.Clay, m * a.Obsidian, m * a.Geode);
        }

        public static Material operator +(Material a, Material b)
        {
            return new Material(
                a.Ore + b.Ore,
                a.Clay + b.Clay,
                a.Obsidian + b.Obsidian,
                a.Geode + b.Geode
            );
        }

        public static Material operator -(Material a, Material b)
        {
            return new Material(
                a.Ore - b.Ore,
                a.Clay - b.Clay,
                a.Obsidian - b.Obsidian,
                a.Geode - b.Geode
            );
        }

        public static bool operator <=(Material a, Material b)
        {
            return
                a.Ore <= b.Ore &&
                a.Clay <= b.Clay &&
                a.Obsidian <= b.Obsidian &&
                a.Geode <= b.Geode;
        }

        public static bool operator >=(Material a, Material b)
        {
            return
                a.Ore >= b.Ore &&
                a.Clay >= b.Clay &&
                a.Obsidian >= b.Obsidian &&
                a.Geode >= b.Geode;
        }
    }

    record Robot(int Id, Material Cost, Material Producing);

    record State(int RemainingTime, Material Available, Material Producing, int DontBuild);

    record Blueprint(int Id, params Robot[] Robots)
    {
        public readonly Material MaxCost = new(
            Ore: Robots.Select(robot => robot.Cost.Ore).Max(),
            Clay: Robots.Select(robot => robot.Cost.Clay).Max(),
            Obsidian: Robots.Select(robot => robot.Cost.Obsidian).Max(),
            Geode: int.MaxValue
        );
    }
}