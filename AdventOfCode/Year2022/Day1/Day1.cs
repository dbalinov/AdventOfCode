namespace AdventOfCode.Year2022.Day1;

/// <summary>
/// Calorie Counting
/// </summary>
public class Day1 : IDay
{
    private const string FileName = "Year2022/Day1/input.txt";

    public async Task SolvePart1()
    {
        var maxCalories = 0;
        var currentCalories = 0;

        var lines = FileReader.GetAllLinesAsync(FileName);
        await foreach (var line in lines)
        {
            if (line == string.Empty)
            {
                currentCalories = 0;
            }
            else
            {
                currentCalories += int.Parse(line);
            }

            if (currentCalories > maxCalories)
            {
                maxCalories = currentCalories;
            }
        }

        Console.WriteLine(maxCalories);
    }

    public async Task SolvePart2()
    {
        var top1Calories = 0;
        var top2Calories = 0;
        var top3Calories = 0;

        var currentCalories = 0;

        var lines = FileReader.GetAllLinesAsync(FileName);

        // Append empty string to handle the last group of calories
        lines = AppendEmptyString(lines);

        await foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                if (currentCalories > top1Calories)
                {
                    top3Calories = top2Calories;
                    top2Calories = top1Calories;
                    top1Calories = currentCalories;
                }
                else if (currentCalories > top2Calories)
                {
                    top3Calories = top2Calories;
                    top2Calories = currentCalories;
                }
                else if (currentCalories > top3Calories)
                {
                    top3Calories = currentCalories;
                }

                currentCalories = 0;
            }
            else
            {
                currentCalories += int.Parse(line);
            }
        }

        var sum = top1Calories + top2Calories + top3Calories;

        Console.WriteLine(sum);
    }

    private static async IAsyncEnumerable<string> AppendEmptyString(IAsyncEnumerable<string> lines)
    {
        await foreach (var line in lines)
        {
            yield return line;
        }

        yield return string.Empty;
    }
}