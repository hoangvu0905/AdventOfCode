using NUnit.Framework;

namespace Year2022;

public class Day06 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        Console.WriteLine(GetIndexStartMessage(4));
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        Console.WriteLine(GetIndexStartMessage(14));
        Assert.Pass();
    }

    private int GetIndexStartMessage(int numberCharactor)
    {
        var queue = new Queue<char>();
        
        for (var index = 0; index < lines[0].Length; index++)
        {
            var chr = lines[0][index];
            if (queue.Count == numberCharactor)
            {
                queue.Dequeue();
                queue.Enqueue(chr);

                if (queue.Distinct().Count() != queue.Count) continue;
                return index + 1;
            }

            queue.Enqueue(chr);
        }

        throw new Exception();
    }
}