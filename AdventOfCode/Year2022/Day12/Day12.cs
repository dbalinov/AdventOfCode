namespace AdventOfCode.Year2022.Day12;

internal sealed class Day12 : IDay
{
    private const string FileName = "Year2022/Day12/input.txt";

    private const char StartSymbol = 'S';
    private const char EndSymbol = 'E';
    
    public async Task SolvePart1()
    {
        var lines = await File.ReadAllLinesAsync(FileName);
        
        Location start = default;
        Location end = default;
        for (var row = 0; row < lines.Length; row++)
        {
            for (var col = 0; col < lines[0].Length; col++)
            {
                if (lines[row][col] == StartSymbol)
                    start = new Location(row, col);
                if (lines[row][col] == EndSymbol)
                    end = new Location(row, col);
            }
        }

        var shortestPath = GetShortestPath(lines, start, end);
 
        Console.WriteLine(shortestPath);
    }

    public async Task SolvePart2()
    {
        var lines = await File.ReadAllLinesAsync(FileName);

        var starts = new List<Location>();
        Location end = default;
        for (var row = 0; row < lines.Length; row++)
        {
            for (var col = 0; col < lines[0].Length; col++)
            {
                if (lines[row][col] is StartSymbol or 'a')
                    starts.Add(new Location(row, col));
                if (lines[row][col] == EndSymbol)
                    end = new Location(row, col);
            }
        }

        var minShortestPath = starts
            .Select(start => GetShortestPath(lines, start, end))
            .Min();

        Console.WriteLine(minShortestPath);
    }

    public static int GetShortestPath(string[] lines, Location start, Location end)
    {
        var previous = new Dictionary<Location, Location>();

        var queue = new Queue<Location>();
        queue.Enqueue(end);

        while (queue.Count > 0)
        {
            var currentLocation = queue.Dequeue();

            var nextLocations = GetNextLocations(currentLocation)
                .Where(x => x.Row >= 0 && x.Row < lines.Length &&
                            x.Col >= 0 && x.Col < lines[0].Length);

            foreach (var nextLocation in nextLocations)
            {
                if (previous.ContainsKey(nextLocation))
                    continue;

                var currentAltitude = GetAltitude(lines[currentLocation.Row][currentLocation.Col]);
                var nextAltitude = GetAltitude(lines[nextLocation.Row][nextLocation.Col]);

                if (currentAltitude - nextAltitude <= 1)
                {
                    previous[nextLocation] = currentLocation;
                    queue.Enqueue(nextLocation);
                }
            }
        }

        var length = 0;
        var current = start;
        while (!current.Equals(end))
        {
            length++;
            previous.TryGetValue(current, out current);
        };

        return length;
    }

    private static IEnumerable<Location> GetNextLocations(Location l) => new List<Location>
    {
        new(l.Row, l.Col + 1),
        new(l.Row, col: l.Col - 1),
        new(l.Row + 1, l.Col),
        new(l.Row - 1, l.Col)
    };

    private static char GetAltitude(char ch) => ch switch
    {
        StartSymbol => 'a',
        EndSymbol => 'z',
        _ => ch
    };
}