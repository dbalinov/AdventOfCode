namespace AdventOfCode.Year2022.Day18;

internal sealed class Day18 : IDay
{
    private const string FileName = "Year2022/Day18/input.txt";

    private static readonly IReadOnlyList<Point3D> Adjacencies = new Point3D[]
    {
        new(1, 0, 0),
        new(-1, 0, 0),
        new(0, 1, 0),
        new(0, -1, 0),
        new(0, 0, 1),
        new(0, 0, -1),
    };

    public string Name => "Boiling Boulders";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var points = await GetPointsAsync(lines);

        var allVisibleSides = 0;
        foreach (var point1 in points)
        {
            var visibleSides = 6;

            foreach (var point2 in points)
            {
                if (point1 == point2) { continue; }

                foreach (var adjacency in Adjacencies)
                {
                    if (point1 + adjacency == point2)
                    {
                        visibleSides--;
                    }
                }
            }

            allVisibleSides += visibleSides;
        }

        Console.WriteLine(allVisibleSides);
    }
    
    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var points = await GetPointsAsync(lines);

        var minX = points.Select(p => p.X).Min() - 1;
        var maxX = points.Select(p => p.X).Max() + 1;
        var minY = points.Select(p => p.Y).Min() - 1;
        var maxY = points.Select(p => p.Y).Max() + 1;
        var minZ = points.Select(p => p.Z).Min() - 1;
        var maxZ = points.Select(p => p.Z).Max() + 1;

        var start = new Point3D(minX, minY, minZ);

        var allVisibleSides = 0;

        // BFS
        var visited = new HashSet<Point3D>();
        var queue = new Queue<Point3D>();
        
        visited.Add(start);
        queue.Enqueue(start);

        while (queue.Any())
        {
            start = queue.First();
            queue.Dequeue();

            if (start.X < minX || start.X > maxX ||
                start.Y < minY || start.Y > maxY ||
                start.Z < minZ || start.Z > maxZ)
            {
                continue;
            }

            foreach (var adjacency in Adjacencies)
            {
                var p = start + adjacency;
                
                if (points.Contains(p))
                {
                    allVisibleSides++;
                }
                else if (!visited.Contains(p))
                {
                    visited.Add(p);
                    queue.Enqueue(p);
                }
            }
        }

        Console.Write(allVisibleSides);
    }

    private static async Task<IList<Point3D>> GetPointsAsync(IAsyncEnumerable<string> lines)
    {
        var list = new List<Point3D>();

        await foreach (var line in lines)
        {
            var numbers = line.Split(',').Select(int.Parse).ToArray();

            var point = new Point3D(numbers[0], numbers[1], numbers[2]);

            list.Add(point);
        }

        return list;
    }
}
