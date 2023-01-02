namespace AdventOfCode.Year2022.Day14;

internal sealed class Day14 : IDay
{
    private const string FileName = "Year2022/Day14/input.txt";

    public string Name => "Regolith Reservoir";

    public async Task SolvePart1()
    {
        var grid = await Parse();

        var start = new Point(500, 0);
        grid[start.Row, start.Col] = '+';

        var count = GetSandCount(grid, start);

        Console.WriteLine(count);
    }

    public async Task SolvePart2()
    {
        var grid = await Parse();

        var start = new Point(500, 0);
        grid[start.Row, start.Col] = '+';

        var maxCols = grid.GetLength(0);
        for (var i = 0; i < grid.GetLength(1); i++)
        {
            grid[maxCols - 1, i] = '#';
        }

        var count = GetSandCount(grid, start);

        Console.WriteLine(count);
    }

    private static int GetSandCount(char[,] grid, Point start)
    {
        var count = 0;
        var current = start;
        var cols = new[] { 0, -1, 1 };

        while (true)
        {
            if (current.Row + 1 >= grid.GetLength(0) ||
                current.Col + 1 >= grid.GetLength(1))
            {
                break;
            }

            var canMove = false;
            foreach (var col in cols)
            {
                if (grid[current.Row + 1, current.Col + col] == '.')
                {
                    current.Row++;
                    current.Col += col;

                    canMove = true;
                    
                    break;
                }
            }

            if (canMove)
            {
                continue;
            }

            count++;
            if (grid[current.Row, current.Col] == '+')
            {
                break;
            }

            grid[current.Row, current.Col] = 'o';
            current = start;
        }

        return count;
    }

    private static async Task<char[,]> Parse()
    {
        var lines = File.ReadLinesAsync(FileName);

        var rockPaths = new List<IList<Point>>();

        var maxRows = 0;
        var maxCols = 0;
        await foreach (var line in lines)
        {
            var rockPath = ParseLine(line);

            rockPaths.Add(rockPath);

            var currentMaxRows = rockPath.Max(p => p.Col);
            if (maxRows < currentMaxRows)
            {
                maxRows = currentMaxRows + 1;
            }

            var currentMaxCols = rockPath.Max(p => p.Row);
            if (maxCols < currentMaxCols)
            {
                maxCols = currentMaxCols + 1;
            }
        }
        
        maxRows *= 2;
        maxCols += 2;

        var grid = new char[maxCols, maxRows];

        for (var row = 0; row < grid.GetLength(0); row++)
        {
            for (var col = 0; col < grid.GetLength(1); col++)
            {
                grid[row, col] = '.';
            }
        }

        foreach (var rockPath in rockPaths)
        {
            for (var i = 1; i < rockPath.Count; i++)
            {
                AddLine(grid, rockPath[i-1], rockPath[i]);
            }
        }

        return grid;
    }

    private static void AddLine(char[,] grid, Point start, Point end)
    {
        if (start.Col == end.Col)
        {
            var limit = Math.Max(start.Row, end.Row);
            for (var row = Math.Min(start.Row, end.Row); row < limit; row++)
            {
                grid[row, start.Col] = '#';
            }
        }
        else if (start.Row == end.Row)
        {
            var limit = Math.Max(start.Col, end.Col);
            for (var col = Math.Min(start.Col, end.Col); col < limit; col++)
            {
                grid[start.Row, col] = '#';
            }
        }

        grid[end.Row, end.Col] = '#';
    }

    private static IList<Point> ParseLine(ReadOnlySpan<char> line)
    {
        var points = new List<Point>();

        var numbers = ParseNumbers(line);
        for (var i = 0; i < numbers.Count; i += 2)
        {
            var point = new Point(numbers[i], numbers[i + 1]);
            points.Add(point);
        }

        return points;
    }

    private static IList<int> ParseNumbers(ReadOnlySpan<char> line)
    {
        var number = 0;
        var processing = false;
        var numbers = new List<int>();

        foreach (var ch in line)
        {
            if (char.IsNumber(ch))
            {
                processing = true;
                number *= 10;
                number += ch - '0';
            }
            else if (processing)
            {
                processing = false;
                numbers.Add(number);
                number = 0;
            }
        }

        numbers.Add(number);

        return numbers;
    }
}