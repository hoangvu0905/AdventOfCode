using NUnit.Framework;

namespace Year2022;

public class Day03 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        ulong result = 0;
        foreach (var line in lines)
        {
            var length = line.Length;

            var p1 = line[..(length / 2)];
            var p2 = line[(length / 2)..];

            var chrBoth = FindCharBoth(p1, p2);
            result += PrioritiesChar(chrBoth);
        }

        Console.WriteLine(result);
    }
    
    [Test]
    public override void Part2()
    {
        ulong result = 0;
        for (var i = 0; i < lines.Count;)
        {
            var l1 = lines[i++];
            var l2 = lines[i++];
            var l3 = lines[i++];

            var chrBoth = FindCharThreeLine(l1, l2, l3);
            result += PrioritiesChar(chrBoth);
        }

        Console.WriteLine(result);
    }

    private static char FindCharThreeLine(string l1, string l2, string l3)
    {
        var dic = l1.Distinct().ToDictionary(x => x, _ => 1);
        AddCharDictionary(dic, l2);
        AddCharDictionary(dic, l3);
        return dic.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
    }

    private static void AddCharDictionary(IDictionary<char, int> dic, string line)
    {
        foreach (var chr in line.Distinct())
        {
            if (dic.ContainsKey(chr))
            {
                dic[chr]++;
            }
            else
            {
                dic.Add(chr, 1);
            }
        }
    }

    private static char FindCharBoth(string p1, string p2) => p1.First(p2.Contains);

    private static ulong PrioritiesChar(char chr) => chr >= 97 ? chr - 96ul : chr - 64ul + 26;
}