using NUnit.Framework;

namespace Year2022;

public class Day08 : BaseDayTest
{
    private int[,] grid;
    private bool[,] canVisible;
    private int row = 0;
    private int col = 0;

    [Test]
    public override void Part1()
    {
        ParseGrid();
        CalcCanView();

        Console.WriteLine(CountTreeVisible());
        Assert.Pass();
    }

    [Test]
    public override void Part2()
    {
        ParseGrid();
        CalcCanView();

        Console.WriteLine(CalcTreeBuildHouse());
        Assert.Pass();
    }

    private int CalcTreeBuildHouse()
    {
        var max = 0;
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                if (!canVisible[i, j]) continue;
                max = Math.Max(max, GetTreeSee(i, j));
            }
        }

        return max;
    }

    private int GetTreeSee(int xCell, int yCell)
    {
        var value = grid[yCell, xCell];

        var treeLeft = Enumerable.Range(0, xCell).Reverse().TakeWhile(x => grid[yCell, x] < value).Count();
        var treeRight = Enumerable.Range(xCell + 1, row - xCell - 1).TakeWhile(x => grid[yCell, x] < value).Count();
        var treeUp = Enumerable.Range(0, yCell).Reverse().TakeWhile(x => grid[x, xCell] < value).Count();
        var treeDown = Enumerable.Range(yCell + 1, col - yCell - 1).TakeWhile(x => grid[x, xCell] < value).Count();

        return AddIfHightTree(treeLeft, xCell) *
               AddIfHightTree(treeRight, row - xCell - 1) *
               AddIfHightTree(treeUp, yCell) *
               AddIfHightTree(treeDown, col - yCell - 1);
    }

    private int AddIfHightTree(int count1, int count2)
    {
        return count1 == count2 ? count1 : count1 + 1;
    }

    private int CountTreeVisible()
    {
        var count = 0;
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                if (canVisible[i, j]) count++;
            }
        }

        return count;
    }

    private void CalcCanView()
    {
        for (var i = 1; i < row; i++)
        {
            for (var j = 1; j < col; j++)
            {
                TreeCanVisible(i, j);
            }
        }
    }

    private void TreeCanVisible(int xCell, int yCell)
    {
        if (canVisible[xCell, yCell])
        {
            return;
        }

        var value = grid[yCell, xCell];

        canVisible[xCell, yCell] = Enumerable.Range(0, xCell).All(x => grid[yCell, x] < value)
                                   || Enumerable.Range(xCell + 1, row - xCell - 1)
                                       .All(x => grid[yCell, x] < value)
                                   || Enumerable.Range(0, yCell).All(x => grid[x, xCell] < value)
                                   || Enumerable.Range(yCell + 1, col - yCell - 1)
                                       .All(x => grid[x, xCell] < value);
    }

    private void ParseGrid()
    {
        row = lines.Count;
        col = lines[0].Length;

        grid = new int[row, col];
        canVisible = new bool[row, col];
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                grid[i, j] = lines[i][j] - '0';
                if (i == 0 || j == 0 || i == row - 1 || j == col - 1)
                {
                    canVisible[i, j] = true;
                }
            }
        }
    }
}