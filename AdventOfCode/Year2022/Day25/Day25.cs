namespace AdventOfCode.Year2022.Day25;

internal class Day25 : IDay
{
    private const string FileName = "Year2022/Day25/input.txt";

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
    {
        return Task.CompletedTask;
    }

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
        var n = number;

        var digits = new List<long>();
        var i = 0;
        while (n > 0)
        {
            var remainder = n % 5;

            n /= 5;

            switch (remainder)
            {
                case >= 0 and <= 2:
                {
                    Add(digits, i, remainder);
                    break;
                }
                case 3:
                {
                    Add(digits, i, -2);
                    Add(digits, i + 1, 1);
                    break;
                }
                case 4:
                {
                    Add(digits, i, -1);
                    Add(digits, i + 1, 1);
                    break;
                }
            }

            i++;
        }

        // Normalize
        while (digits.Any(x => x == 3))
        {
            for (var j = 0; j < digits.Count; j++)
            {
                if (digits[j] != 3)
                {
                    continue;
                }

                digits[j] = -2;
                Add(digits, j + 1, 1);
            }
        }

        digits.Reverse();

        var chars = digits
            .Select(digit => digit switch
            {
                >= 0 and <= 2 => (char) ('0' + digit),
                -1 => '-',
                -2 => '=',
                _ => throw new NotSupportedException($"Invalid char {digit}")
            });

        var toString = string.Join(string.Empty, chars);

        return toString;
    }
    
    private static void Add(IList<long> digits, int index, long value)
    {
        if (digits.Count == index)
        {
            digits.Add(value);
        }
        else
        {
            digits[index] += value;
        }
    }
}
