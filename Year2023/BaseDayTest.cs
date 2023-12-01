using NUnit.Framework;

namespace Year2023;

[TestFixture]
public abstract class BaseDayTest
{
    protected IList<string> lines;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var filename = TestContext.CurrentContext.Test.ClassName!.Split(".").Last();
// #if DEBUG
//         if (File.Exists($"input/{filename}_debug.txt"))
//         {
//             filename += "_debug";
//         }
// #endif
        
        lines = File.ReadAllLines("input/" + filename + ".txt");
    }

    [Test]
    public abstract void Part1();
    
    [Test]
    public abstract void Part2();

    protected void Print(bool[,] array2D) => LoopArray2D(array2D, x => Console.Write(x ? '#' : '.'), Console.WriteLine); 

    protected T LoopArray2D<T, TElement>(TElement[,] array2D, Func<TElement, T, T> calcElement, Func<T, T>? newLine = null)
    {
        T result = default;
        for (var i = 0; i < array2D.GetLongLength(0); i++)
        {
            for (var j = 0; j < array2D.GetLongLength(1); j++)
            {
                result = calcElement(array2D[i, j], result);
            }

            if (newLine is not null) result = newLine(result);
        }

        return result;
    }
    
    protected void LoopArray2D<TElement>(TElement[,] array2D, Action<TElement> calcElement, Action? newLine = null)
    {
        for (var i = 0; i < array2D.GetLongLength(0); i++)
        {
            for (var j = 0; j < array2D.GetLongLength(1); j++)
            {
                calcElement(array2D[i, j]);
            }

            newLine?.Invoke();
        }
    }

    protected static double CalcDiff(long x1, long y1, long x2, long y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }
    protected static int CalcManhattan(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
    }
    
    public class Point
    {
        public Point? Parent { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"{{X = {X}; Y = {Y} }}";
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }
}
