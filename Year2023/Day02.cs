using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Year2023;

public partial class Day02 : BaseDayTest
{
    public static readonly Dictionary<string, int> MaxValues = new()
    {
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 },
    };

    [Test]
    public override void Part1()
    {
        var result = 0;

        foreach (var line in lines)
        {
            var color = ParseMaxInput(line);
            if (color.IsValid())
            {
                var game = int.Parse(IndexGame().Match(line).Groups[1].Value);
                result += game;
            }
        }

        Console.WriteLine(result);
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        var result = lines.Select(ParseMaxInput).Select(color => color.red * color.blue * color.green).Sum();

        Console.WriteLine(result);
        Assert.Pass();
    }

    public record Color(int red, int blue, int green)
    {
        public bool IsValid()
        {
            return red <= MaxValues[nameof(red)] && blue <= MaxValues[nameof(blue)] &&
                   green <= MaxValues[nameof(green)];
        }
    }

    private Color ParseMaxInput(string line)
    {
        var dictionary = RegexNumberCube().Matches(line).GroupBy(x => x.Groups[2].Value)
            .ToDictionary(x => x.Key, x => x.Max(y => int.Parse(y.Groups[1].Value)));

        return new Color(dictionary.GetValueOrDefault(nameof(Color.red)),
            dictionary.GetValueOrDefault(nameof(Color.blue)), dictionary.GetValueOrDefault(nameof(Color.green)));
    }

    [GeneratedRegex("Game (\\d+)")]
    private static partial Regex IndexGame();

    [GeneratedRegex("(\\d+) (red|blue|green)")]
    private static partial Regex RegexNumberCube();
}