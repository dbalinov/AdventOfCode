namespace AdventOfCode.Year2022.Day3;

internal sealed class Day3 : IDay
{
    private const string FileName = "Year2022/Day3/input.txt";

    public string Name => "Rucksack Reorganization";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var priorities = 0;
        await foreach (var line in lines)
        {
            var item = GetRepetitiveItem(line);
            if (item == null)
            {
                continue;
            }

            var priority = GetPriority(item.Value);
            priorities += priority;
        }

        Console.WriteLine(priorities);
    }

    public async Task SolvePart2()
    {
        using var streamReader = new StreamReader(FileName);

        string? line1;
        string? line2;
        string? line3;
        var priorities = 0;
        while ((line1 = await streamReader.ReadLineAsync()) != null &&
               (line2 = await streamReader.ReadLineAsync()) != null &&
               (line3 = await streamReader.ReadLineAsync()) != null)
        {

            var item = GetBadgeItem(line1, line2, line3);
            if (item == null)
            {
                continue;
            }

            priorities += GetPriority(item.Value);
        }

        Console.WriteLine(priorities);
    }
    
    private static char? GetBadgeItem(ReadOnlySpan<char> line1, ReadOnlySpan<char> line2, ReadOnlySpan<char> line3)
    {
        foreach (var item in line1)
        {
            if (line2.Contains(item) && line3.Contains(item))
            {
                return item;
            }
        }

        return null;
    }
    
    private static int GetPriority(char item)
        => item switch
        {
            >= 'a' and <= 'z' => item - 'a' + 1,
            >= 'A' and <= 'Z' => item - 'A' + 27,
            _ => throw new NotSupportedException("Not defined use case 2. " + item)
        };

    private static char? GetRepetitiveItem(ReadOnlySpan<char> line)
    {
        var middleIndex = line.Length / 2;
        var part1 = line[..middleIndex];
        var part2 = line[middleIndex..];

        foreach (var letter in part1)
        {
            if (part2.Contains(letter))
            {
                return letter;
            }
        }

        return null;
    }
}