namespace AdventOfCode.Year2022.Day15;

/// <summary>
/// Beacon Exclusion Zone
/// </summary>
internal sealed class Day15 : IDay
{
    private const string FileName = "Year2022/Day15/input.txt";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        const int targetLine = 2000000; // test: 10;
        var set = new HashSet<int>();

        await foreach (var line in lines)
        {
            var points = ParseLine(line);

            var sensorX = points[0];
            var sensorY = points[1];
            var beaconX = points[2];
            var beaconY = points[3];

            var distance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);

            var lineDistance = Math.Abs(targetLine - sensorY);

            var fromX = sensorX - distance + lineDistance;
            var toX = sensorX + distance - lineDistance;

            if (toX < fromX)
            {
                continue;
            }

            for (var i = fromX; i < toX; i++)
            {
                set.Add(i);
            }
        }

        Console.WriteLine(set.Count);
    }

    public async Task SolvePart2()
    {
        throw new NotImplementedException();
    }

    private static IList<int> ParseLine(ReadOnlySpan<char> line)
    {
        var numbers = new List<int>();

        int? number = null;
        var isNegative = false;
        foreach (var ch in line)
        {
            if (ch == '-')
            {
                isNegative = true;
            }

            if (char.IsNumber(ch))
            {
                number ??= 0;

                number *= 10;
                number += ch - '0';
            }
            else if (number.HasValue)
            {
                if (isNegative)
                {
                    number = number.Value * -1;
                }

                numbers.Add(number.Value);
                number = null;
                isNegative = false;
            }
        }

        if (number.HasValue)
        {
            numbers.Add(number.Value);
        }

        return numbers;
    }
}