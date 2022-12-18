using System.Collections;
using System.Text;
using NUnit.Framework;

namespace Year2022;

public class Day16 : BaseDayTest
{
    private IList<Node> _nodes = new List<Node>();
    private Map _map = null!;

    private readonly Dictionary<(Player, Player, string, int), int> _cache = new();

    [Test]
    public override void Part1()
    {
        Parse();

        var start = _nodes.Single(x => x.Name == "AA");

        Console.WriteLine(MaxFlow(0, 0, new Player(start, 0), new Player(start, int.MaxValue), GetInitState(), 30));

        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        Parse();

        var start = _nodes.Single(x => x.Name == "AA");

        Console.WriteLine(MaxFlow(0, 0, new Player(start, 0), new Player(start, 0), GetInitState(), 26));
    }

    private BitArray GetInitState()
    {
        var state = new BitArray(_map.Nodes.Length);
        for (var i = 0; i < _map.Nodes.Length; i++)
        {
            if (_map.Nodes[i].Rate > 0)
            {
                state[i] = true; // only check if state true. (true is close, should be open)
            }
        }

        return state;
    }

    private int MaxFlow(int max, int current, Player player, Player elephant, BitArray state, int round)
    {
        var keyCache = (player, elephant, BitArrayToString(state), round);
        if (_cache.ContainsKey(keyCache))
        {
            return _cache[keyCache];
        }
        
        var nextState = new Player[2][];

        for (var indexPlayer = 0; indexPlayer < 2; indexPlayer++)
        {
            var currentPlayer = indexPlayer == 0 ? player : elephant;
            if (currentPlayer.Distance > 0)
            {
                nextState[indexPlayer] = new[] {currentPlayer with {Distance = currentPlayer.Distance - 1}};
            }
            else if (state[currentPlayer.Value.Id])
            {
                current += currentPlayer.Value.Rate * (round - 1);
                if (current > max) max = current;

                state = new BitArray(state)
                {
                    [currentPlayer.Value.Id] = false
                }; // copy state; set opened
                nextState[indexPlayer] = new[] {currentPlayer}; // for skip round because used open.
            }
            else
            {
                var stateContinue = new List<Player>();
                for (var i = 0; i < state.Length; i++)
                {
                    if (!state[i]) continue;
                    var nextNode = _map.Nodes[i];
                    stateContinue.Add(new Player(nextNode, _map.Distances[currentPlayer.Value.Id, nextNode.Id] - 1));
                }

                nextState[indexPlayer] = stateContinue.ToArray();
            }
        }

        round--;
        if (round < 1) return max;

        for (var iPlayer = 0; iPlayer < nextState[0].Length; iPlayer++)
        {
            for (var iElephant = 0; iElephant < nextState[1].Length; iElephant++)
            {
                var currentPlayer = nextState[0][iPlayer];
                var currentElephant = nextState[1][iElephant];

                if ((nextState[0].Length > 1 || nextState[1].Length > 1) && currentPlayer.Value == currentElephant.Value)
                {
                    continue;
                }

                // Skip move
                var advance = 0;
                if (currentPlayer.Distance > 0 && currentElephant.Distance > 0)
                {
                    advance = Math.Min(currentPlayer.Distance, currentElephant.Distance);
                    currentPlayer = currentPlayer with {Distance = currentPlayer.Distance - advance};
                    currentElephant = currentElephant with {Distance = currentElephant.Distance - advance};
                }

                max = MaxFlow(
                    max,
                    current,
                    currentPlayer,
                    currentElephant,
                    state,
                    round - advance
                );
            }
        }

        _cache.Add(keyCache, max);
        return max;
    }
    
    string BitArrayToString(BitArray bitArray){
        var sb = new StringBuilder();
        for (int i = 0; i < bitArray.Count; i++)
        {
            sb.Append(bitArray[i] ? '1' : '0');
        }
    
        return sb.ToString();
    }

    private void ParseLine(string line)
    {
        var parts = line.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries).ToList();
        var current = parts[1];
        var rate = int.Parse(parts[4][5..^1]);

        _nodes.Add(new Node(0, current, rate, parts.Skip(9).ToArray()));
    }

    private void Parse()
    {
        foreach (var line in lines)
        {
            ParseLine(line);
        }

        _nodes = _nodes.OrderByDescending(x => x.Rate).Select((x, i) => x with {Id = i}).ToList();

        _map = new Map(ComputeDistances(), _nodes.ToArray());
    }

    private int[,] ComputeDistances()
    {
        var distances = new int[_nodes.Count, _nodes.Count];
        for (var i = 0; i < distances.GetLongLength(0); i++)
        {
            for (var j = 0; j < distances.GetLongLength(1); j++)
            {
                distances[i, j] = int.MaxValue;
            }
        }

        foreach (var node in _nodes)
        {
            foreach (var tunnel in node.Tunnels)
            {
                var targetNode = _nodes.Single(x => x.Name == tunnel);
                distances[node.Id, targetNode.Id] = 1;
                distances[targetNode.Id, node.Id] = 1;
            }
        }

        var done = true;
        while (done)
        {
            done = false;
            for (var k = 0; k < _nodes.Count; k++)
            {
                for (var i = 0; i < _nodes.Count; i++)
                {
                    for (var j = 0; j < _nodes.Count; j++)
                    {
                        if (i == j || distances[i, k] == int.MaxValue || distances[k, j] == int.MaxValue ||
                            distances[i, j] <= distances[i, k] + distances[k, j]) continue;
                        done = true;
                        distances[i, j] = distances[i, k] + distances[k, j];
                    }
                }
            }
        }

        return distances;
    }

    record Node(int Id, string Name, int Rate, string[] Tunnels);

    record Map(int[,] Distances, Node[] Nodes);

    record Player(Node Value, int Distance);
}