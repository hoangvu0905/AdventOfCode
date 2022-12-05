using NUnit.Framework;

namespace Year2022;

public class Day05 : BaseDayTest
{
    private readonly IList<Stack<char>> stacks = new List<Stack<char>>();

    [Test]
    public override void Part1()
    {
        AddStack(lines);

        lines = lines.SkipWhile(line => !line.StartsWith("move")).ToList();
        foreach (var line in lines)
        {
            var (numberMove, from, to) = ReadLineMove(line);
            MoveStack(numberMove, from, to);
        }

        GetResult();
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        AddStack(lines);

        lines = lines.SkipWhile(line => !line.StartsWith("move")).ToList();
        foreach (var line in lines)
        {
            var (numberMove, from, to) = ReadLineMove(line);
            MoveStackPart2(numberMove, from, to);
        }
        
        GetResult();
        Assert.Pass();
    }


    private void GetResult()
    {
        foreach (var stack in stacks)
        {
            Console.Write(stack.Pop());
        }
    }

    private void AddStack(IList<string> lines)
    {
        for (var i = 1; i < lines[0].Length; i += 4)
        {
            var strStack = lines.TakeWhile(line => !char.IsDigit(line[i]))
                .Aggregate(string.Empty, (current, line) => current + line[i]);

            stacks.Add(new Stack<char>(strStack.Trim().Reverse()));
        }
    }

    private void MoveStack(int numberMove, int from, int to)
    {
        for (var i = 0; i < numberMove; i++)
        {
            stacks[to].Push(stacks[from].Pop());
        }
    }
    
    private void MoveStackPart2(int numberMove, int from, int to)
    {
        var temp = string.Empty;
        for (var i = 0; i < numberMove; i++)
        {
            temp+= stacks[from].Pop();
        }

        foreach (var chr in temp.Reverse())
        {
            stacks[to].Push(chr);
        }
    }

    private static (int NoMove, int From, int To) ReadLineMove(string line)
    {
        var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => char.IsDigit(x[0]))
            .Select(int.Parse).ToList();
        return (numbers[0], numbers[1] - 1, numbers[2] - 1);
    }
}