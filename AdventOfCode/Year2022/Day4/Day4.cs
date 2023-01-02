namespace AdventOfCode.Year2022.Day4;

internal sealed class Day4 : IDay
{
    private const string FileName = "Year2022/Day4/input.txt";

    public string Name => "Camp Cleanup";

    public async Task SolvePart1()
    {
        var count = 0;

        var lines = File.ReadLinesAsync(FileName);
        await foreach (var line in lines)
        {
            var indexOfComma = line.IndexOf(',');

            var (start1, end1) = GetRange(line.AsSpan(0, indexOfComma));
            var (start2, end2) = GetRange(line.AsSpan(indexOfComma + 1));

            if (start1 <= start2 && end1 >= end2 || start2 <= start1 && end2 >= end1)
            {
                count++;
            }
        }

        Console.WriteLine(count);
    }

    public async Task SolvePart2()
    {
        var count = 0;

        var lines = File.ReadLinesAsync(FileName);
        await foreach (var line in lines)
        {
            var indexOfComma = line.IndexOf(',');

            var (start1, end1) = GetRange(line.AsSpan(0, indexOfComma));
            var (start2, end2) = GetRange(line.AsSpan(indexOfComma + 1));

            if (!(start1 <= end1 && end1 < start2 && start2 <= end2 ||
                  start2 <= end1 && end2 < start1 && start1 <= end1))
            {
                count++;
            }
        }

        Console.WriteLine(count);
    }

    private static ValueTuple<int, int> GetRange(ReadOnlySpan<char> rangeSpan)
    {
        var separatorIndex = rangeSpan.IndexOf('-');

        var start = int.Parse(rangeSpan[..separatorIndex]);
        var end = int.Parse(rangeSpan[(separatorIndex + 1)..]);

        return (start, end);
    }
}
