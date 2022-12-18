namespace AdventOfCode.Year2022.Day18;

internal sealed class Day18 : IDay
{
    private const string FileName = "Year2022/Day18/test-input.txt";

    private static readonly IReadOnlyList<Point3D> Adjacencies = new Point3D[]
    {
        new(1, 0, 0),
        new(-1, 0, 0),
        new(0, 1, 0),
        new(0, -1, 0),
        new(0, 0, 1),
        new(0, 0, -1),
    };

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
