namespace AdventOfCode.Year2022.Day24;

internal sealed class Day24 : IDay
{
    private const string FileName = "Year2022/Day24/input.txt";

    public Day24() => Console.Title = "Blizzard Basin";

    public async Task SolvePart1()
    {
        var lines = await File.ReadAllLinesAsync(FileName);

        var start = new Location(0, 1, 0);
        var target = new Location(lines.Length - 1, lines[^1].Length - 2, 0);
        
        var pathLength = GetPathLength(lines, start, target, 0);
        
        Console.WriteLine(pathLength);
    }

    public async Task SolvePart2()
    {
        var lines = await File.ReadAllLinesAsync(FileName);

        var start = new Location(0, 1, 0);
        var target = new Location(lines.Length - 1, lines[^1].Length - 2, 0);

        var pathLength = 0;
        pathLength = GetPathLength(lines, start, target, pathLength);
        pathLength = GetPathLength(lines, target, start, pathLength);
        pathLength = GetPathLength(lines, start, target, pathLength);
        
        Console.WriteLine(pathLength);
    }

    private static int GetPathLength(IList<string> lines, Location start, Location target, int g)
    {
        var maxRow = lines.Count - 1;
        var maxCol = lines[0].Length - 1;

        var blizzards = Parse(lines);

        var blizzardsForMinutes = GetBlizzardsForMinutes(maxRow, maxCol, blizzards);

        var closedList = new HashSet<Location>();
        var openList = new Queue<Location>();
        openList.Enqueue(start with { G = g });

        var current = start;
        while (openList.Count > 0)
        {
            current = openList.Dequeue();

            if (current.Row == target.Row)
            {
                break;
            }

            if (closedList.Contains(current))
                continue;

            closedList.Add(current);

            var proposedLocations = GetProposedLocations(blizzardsForMinutes, current);

            foreach (var location in proposedLocations)
            {
                if (location.Row >= 0 && location.Row <= maxRow &&
                    location.Col >= 0 && location.Col <= maxCol &&
                    lines[location.Row][location.Col] != '#')
                {
                    openList.Enqueue(location);
                }
            }
        }

        return current.G - 1;
    }

    private static List<HashSet<Location>>? _blizzardsForMinutes = null;
    private static List<HashSet<Location>> GetBlizzardsForMinutes(int maxRow, int maxCol, IList<Blizzard> blizzards)
    {
        if (_blizzardsForMinutes != null)
        {
            return _blizzardsForMinutes;
        }

        var matricesLength = (maxRow - 1) * (maxCol - 1);
        _blizzardsForMinutes = new List<HashSet<Location>>();

        for (var g = 1; g <= matricesLength; g++)
        {
            var blizzardsForMinute = new HashSet<Location>();

            foreach (var blizzard in blizzards)
            {
                blizzardsForMinute.Add(new Location(blizzard.Row, blizzard.Col, g));

                blizzard.Move(maxRow, maxCol);
            }

            _blizzardsForMinutes.Add(blizzardsForMinute);
        }

         
        return _blizzardsForMinutes;
    }

    private static IList<Blizzard> Parse(IList<string> lines)
    {
        var blizzards = new List<Blizzard>();

        for (var row = 0; row < lines.Count; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                var c = lines[row][col];
                if (c != '.')
                {
                    blizzards.Add(new Blizzard
                    {
                        Value = c,
                        Row = row,
                        Col = col
                    });
                }
            }
        }

        return blizzards;
    }

    private static IEnumerable<Location> GetProposedLocations(IList<HashSet<Location>> blizzardsForMinutes, Location current)
    {
        var proposedLocations = new HashSet<Location>
        {
            new(current.Row, current.Col, current.G + 1),
            new(current.Row - 1, current.Col, current.G + 1),
            new(current.Row + 1, current.Col, current.G + 1),
            new(current.Row, current.Col - 1, current.G + 1),
            new(current.Row, current.Col + 1, current.G + 1)
        };

        var blizzardsForMinute = blizzardsForMinutes[current.G % blizzardsForMinutes.Count];

        return proposedLocations
            .Where(location => !blizzardsForMinute.Contains(location));
    }
}