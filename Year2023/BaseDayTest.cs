using NUnit.Framework;

namespace Year2023;

[TestFixture]
public abstract class BaseDayTest
{
    protected IList<string> lines;

    protected string FullFilename;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var filename = TestContext.CurrentContext.Test.ClassName!.Split(".").Last();

        FullFilename = "input/" + filename + ".txt";
        lines = File.ReadAllLines(FullFilename);
    }

    [Test]
    public abstract void Part1();
    
    [Test]
    public abstract void Part2();
}
