using NUnit.Framework;

namespace Year2022;

[TestFixture]
public abstract class BaseDayTest
{
    protected IList<string> lines;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        lines = File.ReadAllLines("input/" + TestContext.CurrentContext.Test.ClassName!.Split(".").Last() + ".txt");
    }

    [Test]
    public abstract void Part1();
    
    [Test]
    public abstract void Part2();
}