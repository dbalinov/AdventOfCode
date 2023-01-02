namespace AdventOfCode.Year2019.Day3;

internal sealed class Day3 : IDay
{
    private const string FileName = "Year2019/Day3/input.txt";

    public string Name => "Crossed Wires";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);
        
        var paths = new List<IList<Point>>();

        await foreach (var line in lines)
        {
            var path = Parse(line);

            paths.Add(path);
        }

        var points = GetIntersectionPoints(paths[0], paths[1]);

        var start = new Point(0, 0);
        var minDistance = points
            .Min(p => p.GetManhattanDistance(start));
        
        Console.WriteLine(minDistance);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var paths = new List<IList<Point>>();

        await foreach (var line in lines)
        {
            var path = Parse(line);

            paths.Add(path);
        }

        var points = GetIntersectionPoints(paths[0], paths[1]);

        var minSteps = points
            .Min(p => GetStepsCount(p, paths));

        Console.WriteLine(minSteps);
    }

    private static int GetStepsCount(Point point, List<IList<Point>> paths)
    {
        var count = 0;
        foreach (var path in paths)
        {
            for (var i = 0; i < path.Count - 1; i++)
            {
                var p1 = path[i];
                var p2 = path[i + 1];

                if (p1.Col == p2.Col &&
                    p1.Col == point.Col &&
                    Math.Min(p1.Row, p2.Row) <= point.Row && Math.Max(p1.Row, p2.Row) >= point.Row ||
                    p1.Row == p2.Row &&
                    p1.Row == point.Row &&
                    Math.Min(p1.Col, p2.Col) <= point.Col && Math.Max(p1.Col, p2.Col) >= point.Col)
                {
                    // point on current line
                    count += p1.GetManhattanDistance(point);
                    break;
                }
                
                count += p1.GetManhattanDistance(p2);
            }
        }

        return count;
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection#Given_two_points_on_each_line
    /// </summary>
    private static IList<Point> GetIntersectionPoints(IList<Point> path1, IList<Point> path2)
    {
        var points  = new List<Point>();

        for (var i = 0; i < path1.Count - 1; i++)
        {
            for (var j = 0; j < path2.Count - 1; j++)
            {
                var p1 = path1[i];
                var p2 = path1[i + 1];
                var p3 = path2[j];
                var p4 = path2[j + 1];

                var divider = (p1.Col - p2.Col) * (p3.Row - p4.Row) - (p1.Row - p2.Row) * (p3.Col - p4.Col);
                if (divider == 0)
                {
                    // lines are parallel
                    continue;
                }

                var col = ((p1.Col * p2.Row - p1.Row * p2.Col) * (p3.Col - p4.Col) - (p1.Col - p2.Col) * (p3.Col * p4.Row - p3.Row * p4.Col)) / divider;
                var row = ((p1.Col * p2.Row - p1.Row * p2.Col) * (p3.Row - p4.Row) - (p1.Row - p2.Row) * (p3.Col * p4.Row - p3.Row * p4.Col)) / divider;

                if (row == 0 && col == 0)
                {
                    continue;
                }

                if (col >= Math.Min(p1.Col, p2.Col) && col <= Math.Max(p1.Col, p2.Col) &&
                    col >= Math.Min(p3.Col, p4.Col) && col <= Math.Max(p3.Col, p4.Col) &&
                    row >= Math.Min(p1.Row, p2.Row) && row <= Math.Max(p1.Row, p2.Row) &&
                    row >= Math.Min(p3.Row, p4.Row) && row <= Math.Max(p3.Row, p4.Row))
                {
                    points.Add(new Point(row, col));
                }
            }
        }

        return points;
    }

    private static IList<Point> Parse(string line)
    {
        var result = new List<Point>();

        var current = new Point(0, 0);

        result.Add(current);

        var moves = line.Split(',');
        foreach (var move in moves)
        {
            var direction = move[0];
            var number = int.Parse(move[1..]);

            current = direction switch
            {
                'R' => new Point(current.Row, current.Col + number),
                'U' => new Point(current.Row + number, current.Col),
                'L' => new Point(current.Row, current.Col - number),
                'D' => new Point(current.Row - number, current.Col), 
                _ => throw new InvalidOperationException("Invalid move.")
            };

            result.Add(current);
        }

        return result;
    }
}
