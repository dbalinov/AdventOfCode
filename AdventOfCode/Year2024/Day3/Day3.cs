using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024.Day3;

public partial class Day3 : IDay
{
    private const string FileName = "Year2024/Day3/input.txt";

    public string Name => "Mull It Over";

    public async Task SolvePart1()
    {
        var memory = await File.ReadAllTextAsync(FileName);
        var matches = MulRegex().Matches(memory);
        var sum = matches.Sum(x => ComputeMul(x.Value));
        Console.WriteLine(sum);
    }

    public async Task SolvePart2()
    {
        var memory = await File.ReadAllTextAsync(FileName);

        var regExes = new[] { MulRegex(), DoRegex(), DontRegex() };
        var list = new SortedList<int, string>();
        foreach (var regex in regExes)
        {
            var matches = regex.Matches(memory);
            foreach (Match match in matches)
            {
                list.Add(match.Index, match.Value);
            }
        }

        var isEnabled = true;
        var sum = 0;
        foreach (var value in list.Values)
        {
            if (value == "do()")
            {
                isEnabled = true;
            }
            else if (value == "don't()")
            {
                isEnabled = false;
            }
            else if (isEnabled)
            {
                sum += ComputeMul(value);
            }
        }
        
        Console.WriteLine(sum);
    }

    private static int ComputeMul(string value)
    {
        var numbersString = value.Substring(4, value.Length - 5);
        return numbersString.Split(',').Select(int.Parse)
            .Aggregate((a, b) => a * b);
    }
    
    [GeneratedRegex(@"mul\(\d{1,3},\d{1,3}\)")]
    private static partial Regex MulRegex();
    
    [GeneratedRegex(@"do\(\)")]
    private static partial Regex DoRegex();
    
    [GeneratedRegex(@"don't\(\)")]
    private static partial Regex DontRegex();
}