using System.Numerics;
using NUnit.Framework;

namespace Year2022;

public class Day20 : BaseDayTest
{
    private LinkedList<BigInteger> _linkedList = null!;
    private readonly IList<LinkedListNode<BigInteger>> _list = new List<LinkedListNode<BigInteger>>();
    private LinkedListNode<BigInteger> _nodeZero = null!;
    private const int NumberMultiple = 811589153;

    [Test]
    public override void Part1()
    {
        Parse(1);

        foreach (var item in _list)
        {
            if (_nodeZero is null && item.Value == 0)
            {
                _nodeZero = item;
            }

            MoveItem(item);
        }

        var sum = new BigInteger(0);
        for (var i = 1; i <= 3; i++)
        {
            var node = GetNextNode(_nodeZero!, i * 1000);
            sum += node.Value;
        }

        Console.WriteLine(sum);

        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        Parse(NumberMultiple);

        for (var i = 0; i < 10; i++)
        {
            foreach (var item in _list)
            {
                if (_nodeZero is null && item.Value == 0)
                {
                    _nodeZero = item;
                }

                MoveItem(item);
            }
        }

        var sum = new BigInteger(0);
        for (var i = 1; i <= 3; i++)
        {
            var node = GetNextNode(_nodeZero!, i * 1000);
            sum += node.Value;
        }

        Console.WriteLine(sum);

        Assert.Pass();
    }

    private void Parse(int numberMultiple)
    {
        _linkedList = new LinkedList<BigInteger>(lines.Select(x => new BigInteger(long.Parse(x)) * numberMultiple));
        var node = _linkedList.First;
        while (node != null)
        {
            _list.Add(node);
            node = node.Next;
        }
    }

    private static void MoveItem(LinkedListNode<BigInteger> linkedNode)
    {
        var swapNode = GetNextNode(linkedNode);
        if (linkedNode == swapNode) return;
        linkedNode.List!.Remove(linkedNode);
        swapNode.List!.AddAfter(swapNode, linkedNode);
    }

    private static LinkedListNode<BigInteger> GetNextNode(LinkedListNode<BigInteger> linkedNode, int? noNext = null)
    {
        noNext ??= GetNumberMove(linkedNode);
        if (noNext == 0) return linkedNode;
        var current = noNext < 0 ? linkedNode.Previous ?? linkedNode.List!.Last : linkedNode;
        while (noNext != 0)
        {
            current = noNext < 0 ? current!.Previous ?? current.List!.Last : current!.Next ?? current.List!.First;
            noNext -= Math.Sign(noNext.Value);
        }

        return current!;
    }

    private static int GetNumberMove(LinkedListNode<BigInteger> linkedNode)
    {
        var noNext = (int) (linkedNode.Value % (linkedNode.List!.Count - 1));

        return noNext;
    }
}