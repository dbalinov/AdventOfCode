namespace AdventOfCode.Year2023.Day1;

/// <summary>
/// Trebuchet?!
/// </summary>
public class Day1 : IDay
{
    private const string FileName = "Year2023/Day1/input.txt";

    public string Name => "Trebuchet?!";

    public async Task SolvePart1()
    {
        var sum = 0;

        var lines = File.ReadLinesAsync(FileName);
        await foreach (var line in lines)
        {
            var firstDigit = 0;
            var firstDigitSet = false;
            var lastDigit = 0;
            foreach(var ch in line)
            {
                if (!char.IsDigit(ch))
                {
                    continue;
                }

                if (!firstDigitSet)
                {
                    firstDigit = ch - '0';
                    firstDigitSet = true;
                }

                lastDigit = ch - '0';
            }

            var number = firstDigit * 10 + lastDigit;

            sum += number;
        }
        
        Console.WriteLine(sum);
    }

    public async Task SolvePart2()
    {
        var validNumbers = new[]
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "one", "two", "three", "four", "five",
            "six", "seven", "eight", "nine"
        };
        
        var sum = 0;

        var lines = File.ReadLinesAsync(FileName);
        await foreach (var line in lines)
        {
            var firstIndex = -1;
            var firstDigit = 0;
            var lastIndex = -1;
            var lastDigit = 0;
            
            foreach (var digit in validNumbers)
            {
                var index = line.IndexOf(digit, StringComparison.Ordinal);
                if (index > -1 && (firstIndex == -1 || firstIndex > index))
                {
                    firstIndex = index;
                    firstDigit = ToInt(digit);
                }
                
                var indexEnd = line.LastIndexOf(digit, StringComparison.Ordinal);
                if (indexEnd > -1 && (lastIndex == -1 || lastIndex < indexEnd))
                {
                    lastIndex = indexEnd;
                    lastDigit = ToInt(digit);
                }
            }
            
            var number = firstDigit * 10 + lastDigit;
            
            sum += number;
        }
        
        Console.WriteLine(sum);
    }

    private static int ToInt(string input)
    {
        if (input.Length == 1)
        {
            return input[0] - '0';
        }

        return input switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => throw new NotSupportedException($"${input} cannot be converted to int.")
        };
    }
}