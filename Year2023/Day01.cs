using NUnit.Framework;

namespace Year2023;

public class Day01 : BaseDayTest
{
    private readonly string[] _spelledLetters = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    [Test]
    public override void Part1()
    {
        var result = 0;
        
        foreach (var line in lines)
        {
            var numbers = line.Where(char.IsDigit).Select(x => x - '0').ToList();
            var number = numbers[0] * 10 + numbers.Last();
            result += number;
        }

        Console.WriteLine(result);
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        var result = 0;
        
        foreach (var line in lines)
        {
            var currentLine = line;
            for (var i = 0; i < _spelledLetters.Length; i++)
            {
                currentLine = currentLine.Replace(_spelledLetters[i], _spelledLetters[i][..^1] + i + _spelledLetters[i][1..]);
            }
            
            var numbers = currentLine.Where(char.IsDigit).Select(x => x - '0').ToList();
            var number = numbers[0] * 10 + numbers.Last();
            result += number;
        }

        Console.WriteLine(result);
        Assert.Pass();


    }
}