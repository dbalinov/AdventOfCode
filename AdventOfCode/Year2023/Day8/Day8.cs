namespace AdventOfCode.Year2023.Day8;

/// <summary>
/// Haunted Wasteland
/// </summary>
internal class Day8 : IDay
{
    private const string FileName = "Year2023/Day8/input.txt";
    
    public string Name => "Haunted Wasteland";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);
        
        var map = new Dictionary<string, (string left, string right)>();
        var instructions = await ParseInputAsync(lines, map);

        var pathLength = 0;
        var current = "AAA";
        foreach (var instruction in GetInstructions(instructions))
        {
            var nextPaths = map[current];
            
            current = instruction == 'L' ? nextPaths.left : nextPaths.right;
            
            pathLength++;
            
            if (current == "ZZZ")
            {
                break;
            }
        }
        
        Console.WriteLine(pathLength);
    }
    
    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);
        
        var map = new Dictionary<string, (string left, string right)>();
        var instructions = await ParseInputAsync(lines, map);

        var startKeys = map.Keys.Where(x => x[2] == 'A').ToList();

        var result = 1L;
        for (var i = 0; i < startKeys.Count; i++)
        {
            var pathLength = 0;
            var current = startKeys[i];
            
            foreach (var instruction in GetInstructions(instructions))
            {
                var nextPaths = map[current];

                current = instruction == 'L' ? nextPaths.left : nextPaths.right;

                pathLength++;
                
                if (current[2] == 'Z')
                {
                    result = LeastCommonMultiple(pathLength, result);
                    break;
                }
            }
        }

        Console.WriteLine(result);
    }

    private static IEnumerable<char> GetInstructions(string input)
    {
        var i = 0;
        while (true)
        {
            yield return input[i];
            i = (i == input.Length - 1) ? 0 : i + 1;
        }
    }
    
    private static async Task<string> ParseInputAsync(IAsyncEnumerable<string> lines, IDictionary<string, (string left, string right)> map)
    {
        string? instructions = null;
        await foreach (var line in lines)
        {
            if (instructions is null)
            {
                instructions = line;
                continue;
            }

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            
            var lineParts = line.Split('=', StringSplitOptions.TrimEntries);
            var nextItems = lineParts[1][1..^1].Split(',', StringSplitOptions.TrimEntries);
            
            map.Add(lineParts[0], (left: nextItems[0], right: nextItems[1]));
        }

        if (instructions is null)
        {
            throw new InvalidOperationException("instructions not set");
        }

        return instructions;
    }
    
    private static long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    private static long LeastCommonMultiple(long a, long b)
        => a * b / GreatestCommonDivisor(a, b);
}