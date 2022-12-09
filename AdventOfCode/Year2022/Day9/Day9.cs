namespace AdventOfCode.Year2022.Day9;

internal sealed class Day9 : IDay
{
    private const string FileName = "Year2022/Day9/input.txt";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var h = new Point(0, 0);
        var t = new Point(0, 0);

        var visited = new HashSet<Point> {t};

        await foreach (var line in lines)
        {
            var words = line.Split(' ');

            var direction = words[0][0];
            var count = int.Parse(words[1]);

            for (var i = 0; i < count; i++)
            {
                h += direction switch
                {
                    'R' => new Point(0, 1),
                    'L' => new Point(0, -1),
                    'U' => new Point(- 1, 0),
                    'D' => new Point(1, 0)
                };

                t = GetNextPosition(h, t);

                visited.Add(t);
            }
        }
        
        Console.WriteLine(visited.Count);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var h = new Point(0, 0);
        var t = Enumerable.Repeat(new Point(0, 0), 9).ToArray();
        var visited = new HashSet<Point> { t[0] };

        await foreach (var line in lines)
        {
            var words = line.Split(' ');

            var direction = words[0][0];
            var count = int.Parse(words[1]);

            for (var i = 0; i < count; i++)
            {
                h += direction switch
                {
                    'R' => new Point(0, 1),
                    'L' => new Point(0, -1),
                    'U' => new Point(-1, 0),
                    'D' => new Point(1, 0)
                };

                for (var j = 0; j < t.Length; j++)
                {
                    var head = j == 0 ? h : t[j - 1];

                    t[j] =  GetNextPosition(head, t[j]);
                }

                visited.Add(t[^1]);
            }
        }

        Console.WriteLine(visited.Count);
    }

    private static Point GetNextPosition(Point h, Point t)
    {
        if (t.X == h.X + 2 && t.Y == h.Y + 2)
        {
            return h + new Point(1, 1);
        }
        if (t.X == h.X + 2 && t.Y == h.Y - 2)
        {
            return h + new Point(1, -1);
        }
        if (t.X == h.X - 2 && t.Y == h.Y + 2)
        {
            return h + new Point(-1, 1);
        }
        if (t.X == h.X - 2 && t.Y == h.Y - 2)
        {
            return h + new Point(-1, -1);
        }
        if (t.X == h.X + 2)
        {
            return h + new Point(1, 0);
        }
        if (t.X == h.X - 2)
        {
            return h + new Point(-1, 0);
        }
        if (t.Y == h.Y + 2)
        {
            return h + new Point(0, 1);
        }
        if (t.Y == h.Y - 2)
        {
            return h + new Point(0, -1);
        }

        return t;
    }
}