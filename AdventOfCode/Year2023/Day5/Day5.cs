namespace AdventOfCode.Year2023.Day5;

/// <summary>
/// If You Give A Seed A Fertilizer
/// </summary>
internal class Day5 : IDay
{
    private const string FileName = "Year2023/Day5/input.txt";
    
    public string Name => "If You Give A Seed A Fertilizer";
    
    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);
        
        var seeds = new List<long>();

        var maps = new Dictionary<string, List<Item>>();
        await ParseAlmanacAsync(lines, seeds, maps);

        var minLocation = GetMinSeedLocation(seeds, maps);

        Console.WriteLine(minLocation);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);
        
        var seedRanges = new List<long>();
        var maps = new Dictionary<string, List<Item>>();
        await ParseAlmanacAsync(lines, seedRanges, maps);

        var index = 0;
        var seedGroups = seedRanges.GroupBy(x => index++ / 2).ToList();
        
        var minLocation = long.MaxValue;
        Parallel.ForEach(seedGroups, group =>
        {
            var length = group.Last();
            var start = group.First();
            var seeds = GetRange(start, length);

            var location = GetMinSeedLocation(seeds, maps);
            
            if (location < minLocation)
            {
                minLocation = location;
            }
            
            Console.WriteLine("> {0}", location);
        });
        
        Console.WriteLine(minLocation);
    }
    
    private static async Task ParseAlmanacAsync(
        IAsyncEnumerable<string> lines,
        List<long> seeds,
        IDictionary<string, List<Item>> maps)
    {
        List<Item>? currentMap = null;
        await foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            if (line.StartsWith("seeds"))
            {
                seeds.AddRange(line[7..]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse));
            }
            else if (line.Contains('-'))
            {
                currentMap = [];
                maps.Add(line, currentMap);
            }
            else if (currentMap is null)
            {
                throw new InvalidOperationException("line not valid");
            }
            else
            {
                var parts = line.Split(' ');
                var item = new Item
                {
                    DestinationRangeStart = long.Parse(parts[0]),
                    SourceRangeStart = long.Parse(parts[1]),
                    Length = long.Parse(parts[2])
                };
            
                currentMap.Add(item); 
            }
        }
    }
    
    private static long GetMinSeedLocation(IEnumerable<long> seeds, Dictionary<string, List<Item>> maps)
    {
        var minLocation = long.MaxValue;
        
        foreach (var seed in seeds)
        {
            var location = seed;
            foreach (var items in maps.Values)
            {
                foreach (var item in items)
                {
                    if (item.SourceRangeStart <= location && location <= item.SourceRangeStart + item.Length)
                    {
                        location = item.DestinationRangeStart + location - item.SourceRangeStart;
                        break;
                    }
                }
            }

            if (location < minLocation)
            {
                minLocation = location;
            }
        }

        return minLocation;
    }

    private static IEnumerable<long> GetRange(long start, long length)
    {
        var end = start + length;
        for (var i = start; i <= end; i++)
        {
            yield return i;
        }
    }
}

public struct Item
{
    public long DestinationRangeStart { get; init; }
    public long SourceRangeStart { get; init; }
    public long Length { get; init; }
}