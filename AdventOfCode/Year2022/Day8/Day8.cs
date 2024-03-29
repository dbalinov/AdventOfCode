﻿namespace AdventOfCode.Year2022.Day8;

public sealed class Day8 : IDay
{
    private const string FileName = "Year2022/Day8/input.txt";

    public string Name => "Treetop Tree House";

    public async Task SolvePart1()
    {
        var data = await File.ReadAllLinesAsync(FileName);

        var countVisible = 0;

        var rowCount = data.Length;
        var colCount = data[0].Length;

        for (var row = 0; row < rowCount; row++)
        {
            for (var col = 0; col < colCount; col++)
            {
                if (row == 0 || col == 0 ||
                    row == rowCount - 1 || col == colCount - 1)
                {
                    countVisible++;
                    continue;
                }

                var visible = true;
                var current = data[row][col];

                for (var j = col - 1; j >= 0; j--)
                {
                    if (current <= data[row][j])
                    {
                        visible = false;
                        break;
                    }
                }

                if (visible)
                {
                    countVisible++;
                    continue;
                }

                visible = true;
                for (var j = col + 1; j < colCount; j++)
                {
                    if (current <= data[row][j])
                    {
                        visible = false;
                        break;
                    }
                }

                if (visible)
                {
                    countVisible++;
                    continue;
                }

                visible = true;
                for (var i = row - 1; i >= 0; i--)
                {
                    if (current <= data[i][col])
                    {
                        visible = false;
                        break;
                    }
                }

                if (visible)
                {
                    countVisible++;
                    continue;
                }

                visible = true;
                for (var i = row + 1; i < rowCount; i++)
                {
                    if (current <= data[i][col])
                    {
                        visible = false;
                        break;
                    }
                }

                if (visible)
                {
                    countVisible++;
                }
            }
        }

        Console.WriteLine(countVisible);
    }

    public async Task SolvePart2()
    {
        var data = await File.ReadAllLinesAsync(FileName);
        var maxScenicScore = 0;

        var rowCount = data.Length;
        var colCount = data[0].Length;

        for (var row = 0; row < rowCount; row++)
        {
            for (var col = 0; col < colCount; col++)
            {
                var scenicScore = GetLeftDistance(data, row, col) * GetRightDistance(data, row, col) *
                                  GetTopDistance(data, row, col) * GetBottomDistance(data, row, col);

                if (maxScenicScore < scenicScore)
                {
                    maxScenicScore = scenicScore;
                }
            }
        }

        Console.WriteLine(maxScenicScore);
    }

    private static int GetLeftDistance(IList<string> data, int row, int col)
    {
        var distanceLeft = 0;
        var current = data[row][col];
        
        for (var j = col - 1; j >= 0; j--)
        {
            distanceLeft++;

            if (current <= data[row][j])
            {
                break;
            }
        }

        return distanceLeft;
    }

    private static int GetRightDistance(IList<string> data, int row, int col)
    {
        var distanceRight = 0;
        var current = data[row][col];
        var colCount = data[row].Length;

        for (var j = col + 1; j < colCount; j++)
        {
            distanceRight++;

            if (current <= data[row][j])
            {
                break;
            }
        }

        return distanceRight;
    }

    private static int GetTopDistance(IList<string> data, int row, int col)
    {
        var distanceTop = 0;
        var current = data[row][col];

        for (var i = row - 1; i >= 0; i--)
        {
            distanceTop++;

            if (current <= data[i][col])
            {
                break;
            }
        }

        return distanceTop;
    }

    private static int GetBottomDistance(IList<string> data, int row, int col)
    {
        var distance = 0;
        var current = data[row][col];
        var rowCount = data.Count;

        for (var i = row + 1; i < rowCount; i++)
        {
            distance++;

            if (current <= data[i][col])
            {
                break;
            }
        }

        return distance;
    }
}