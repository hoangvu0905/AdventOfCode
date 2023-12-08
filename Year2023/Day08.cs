using NUnit.Framework;

namespace Year2023;

public class Day08 : BaseDayTest
{
    private string Navigate; 
    
    private readonly Dictionary<string, Node> _nodes = new();

    private const string StartNode = "AAA";
    private const string EndNode = "ZZZ";
    
    [OneTimeSetUp]
    public void ParseInput()
    {
        Navigate = lines[0];

        foreach (var line in lines.Skip(2))
        {
            var parts = line.Split(new []{'(', ')', '=', ','}, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            
            var node = GetNodeOrCreate(parts[0]);
            node.Left = GetNodeOrCreate(parts[1]);
            node.Right = GetNodeOrCreate(parts[2]);
        }
    }

    private Node GetNodeOrCreate(string name)
    {
        if (!_nodes.TryGetValue(name, out var value))
        {
            value = new Node(name);
            _nodes.Add(name, value);
        }

        return value;
    }
    
    private char GetNextNavigation(ref int index)
    {
        if (index >= Navigate.Length)
        {
            index = 0;
        }

        return Navigate[index++];
    }
    
    private char GetNextNavigation(int index)
    {
        return Navigate[index % Navigate.Length];
    }
    
    [Test]
    public override void Part1()
    {
        var result = 0;

        var currentNode = _nodes[StartNode];
        var currentIndex = 0;

        while (currentNode.Name != EndNode)
        {
            result++;
            var next = GetNextNavigation(ref currentIndex);
            currentNode = currentNode.GetNext(next);
        }
        
        Console.WriteLine(result);
    }

    [Test]
    public override void Part2()
    {
        var startNodes = _nodes.Values.Where(x => x.Name.EndsWith('A')).ToList();
        var infos = new Dictionary<Node, InfoNode>();

        foreach (var node in startNodes)
        {
            infos.Add(node, new InfoNode());
            var currentIndex = 0;
            var currentNode = node;

            while (true)
            {
                var next = GetNextNavigation(currentIndex++);
                currentNode = currentNode.GetNext(next);
                if (currentNode.Name.EndsWith('Z'))
                {
                    if (infos[node].End != 0)
                    {
                        infos[node].Loop = currentIndex - infos[node].End;
                        break;
                    }
                    infos[node].End = currentIndex;
                }
            }
        }
        
        var infoNodes = infos.Values.ToList();
        var steps = infoNodes.Select(x => (long)x.End).ToList();

        var lcm = steps[0];
        for (var i = 1; i < steps.Count; i++)
        {
            lcm = Lcm(lcm, steps[i]);
        }
        
        Console.WriteLine(lcm); // 22103062509257
    }
    
    private static long Lcm(long a, long b) => a * b / Gcd(a, b);

    private static long Gcd(long a, long b)
    {
        while (true)
        {
            if (b == 0) return a;
            var a1 = a;
            a = b;
            b = a1 % b;
        }
    }

    private class InfoNode
    {
        public int End { get; set; }
        public int Loop { get; set; }

        public override string ToString()
        {
            return $"{End} {Loop}";
        }
    }

    public class Node
    {
        public string Name { get; }

        public Node Left { get; set; } = null!;
        public Node Right { get; set; } = null!;

        public Node(string name)
        {
            Name = name;
        }
        
        public Node GetNext(char direction)
        {
            return direction switch
            {
                'L' => Left,
                'R' => Right,
                _ => throw new NotSupportedException()
            };
        }

        public override string ToString()
        {
            return $"{Name} ({Left.Name}, {Right.Name})";
        }
    }
}