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

        var intervals = new List<Interval>();

        await foreach (var line in lines)
        {
            var points = ParseLine(line);

            var interval = GetInterval(points, targetLine);

            if (interval.End < interval.Start)
            {
                continue;
            }
            
            intervals.Add(interval);
        }

        var length = MergeAll(intervals)
            .Select(x => x.End - x.Start)
            .Sum();
        
        Console.WriteLine(length);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        const int size = 4_000_000; // 20

        var allIntervals = new List<Interval>[size];
        for (var i = 0; i < allIntervals.Length; i++)
        {
            allIntervals[i] = new List<Interval>();
        }

        await foreach (var line in lines)
        {
            var points = ParseLine(line);

            for (var targetLine = 0; targetLine < size; targetLine++)
            {
                var interval = GetInterval(points, targetLine);
                if (interval.End < interval.Start)
                {
                    continue;
                }

                var start = interval.Start <= 0 ? 0 : interval.Start;
                var end = interval.End > size - 1 ? size -1 : interval.End;

                var intervals = allIntervals[targetLine];

                intervals.Add(new Interval(start, end));
            }
        }

        for (var y = 0; y < allIntervals.Length; y++)
        {
            allIntervals[y] = MergeAll(allIntervals[y]);

            if (allIntervals[y].Count > 1)
            {
                var intervals = allIntervals[y];
                var x = intervals[0].End + 1;
                
                var frequency = x * 4000000d + y;

                Console.WriteLine(frequency);
                break;
            }
        }
    }

    private static Interval GetInterval(IList<int> points, int targetLine)
    {
        var sensorX = points[0];
        var sensorY = points[1];
        var beaconX = points[2];
        var beaconY = points[3];

        var distance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);

        var lineDistance = Math.Abs(targetLine - sensorY);

        var start = sensorX - distance + lineDistance;
        var end = sensorX + distance - lineDistance;

        return new Interval(start, end);
    }

    private static List<Interval> MergeAll(IEnumerable<Interval> intervals)
    {
        var ordered = intervals.OrderBy(x => x.Start).ToList();

        var result = new List<Interval>();

        var current = ordered[0];
        for (var i = 1; i < ordered.Count; i++)
        {
            if (ordered[i].Overlaps(current))
            {
                current = Merge(ordered[i], current);
            }
            else
            {
                result.Add(current);
                current = ordered[i];
            }
        }

        result.Add(current);

        return result;
    }

    private static Interval Merge(Interval int1, Interval int2)
    {
        if (!int1.Overlaps(int2))
        {
            throw new ArgumentException("Interval ranges do not overlap.");
        }

        var start = int1.Start < int2.Start ? int1.Start : int2.Start;
        var end = int1.End > int2.End ? int1.End : int2.End;

        return new Interval(start, end);
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