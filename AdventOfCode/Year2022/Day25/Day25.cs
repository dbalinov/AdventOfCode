namespace AdventOfCode.Year2022.Day25;

internal class Day25 : IDay
{
    private const string FileName = "Year2022/Day25/input.txt";

    public Day25() => Console.Title = "Full of Hot Air";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var sum = 0L;
        await foreach (var line in lines)
        {
            sum += ParseSnafu(line);
        }

        var snafu = SnafuToString(sum);

        Console.Write(snafu);
    }

    public Task SolvePart2()
        => Task.CompletedTask;

    private static long ParseSnafu(string line)
    {
        var number = 0L;
        var numberBase = 1L;

        for (var i = line.Length - 1; i >= 0; i--)
        {
            var digit = line[i] switch
            {
                '0' => 0,
                '1' => 1,
                '-' => -1,
                '2' => 2,
                '=' => -2,
                _ => throw new NotSupportedException("Invalid char")
            };

            number += numberBase * digit;
            numberBase *= 5;
        }

        return number;
    }

    private static string SnafuToString(long number)
    {
        var chars = new List<char>();
        for (; number > 0; number /= 5) {
            number += 2;

            var ch = (number % 5 - 2) switch
            {
                0 => '0',
                1 => '1',
                -1 => '-',
                2 => '2',
                -2 => '=',
                _ => throw new NotSupportedException("Invalid char")
            };

            chars.Add(ch);
        }

        chars.Reverse();

        return string.Join(string.Empty, chars);
    }
}
