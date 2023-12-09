using NUnit.Framework;

namespace Year2023;

public class Day09 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        var result = 0L;
        foreach (var line in lines)
        {
            var numbers = line.Split(' ').Select(int.Parse).ToList();
            result += FindNextNumber(numbers);
        }

        Console.WriteLine(result);
    }

    [Test]
    public override void Part2()
    {
        var result = 0L;
        foreach (var line in lines)
        {
            var numbers = line.Split(' ').Select(int.Parse).ToList();
            result += FindPreviousNumber(numbers);
        }

        Console.WriteLine(result);
      
    }
    
    private int FindNextNumber(IList<int> numbers)
    {
        var tower = CalcTowerNumbers(numbers);
        return FindNextNumberForTower(tower);
    }
    
    private int FindPreviousNumber(IList<int> numbers)
    {
        var tower = CalcTowerNumbers(numbers);
        return FindPreviousNumberForTower(tower);
    }
    
    private static IList<IList<int>> CalcTowerNumbers(IList<int> numbers)
    {
        var arr = new List<IList<int>>() { numbers };
        var current = arr[0];
        while (current.Any(x => x != 0))
        {
            var newArray = new List<int>();
            for (var i = 0; i < current.Count-1; i++)
            {
                newArray.Add(current[i+1] - current[i]);
            }
            arr.Add(newArray);
            current = newArray;
        }

        return arr;
    }

    private static int FindNextNumberForTower(IList<IList<int>> tower)
    {
        // Add next number to the last tower
        tower.Last().Add(0);

        for (var i = tower.Count - 2; i >= 0; i--)
        {
            tower[i].Add(tower[i].Last() + tower[i + 1].Last());
        }

        return tower[0].Last();
    }
    
    private static int FindPreviousNumberForTower(IList<IList<int>> tower)
    {
        // Add previous number to the last tower
        tower.Last().Insert(0, 0);

        for (var i = tower.Count - 2; i >= 0; i--)
        {
            tower[i].Insert(0, tower[i][0] - tower[i + 1][0]);
        }

        return tower[0].First();
    }
}