namespace AdventOfCode.Year2024.Day2;

public class Day2 : IDay
{
    private const string FileName = "Year2024/Day2/input.txt";

    public string Name => "Red-Nosed Reports";
    
    public async Task SolvePart1()
    {
        var count = 0;
        await foreach (var levels in ReadAllLevelsAsync())
        {
            if (IsSafe(levels))
            {
                count++;
            }
        }
        
        Console.WriteLine(count);
    }

    public async Task SolvePart2()
    {
        var count = 0;
        await foreach (var levels in ReadAllLevelsAsync())
        {
            if (GenerateSubsequences(levels).Any(IsSafe))
            {
                count++;
            }
        }
        
        Console.WriteLine(count);
    }
    
    private static bool IsSafe(int[] levels)
    {
        var isIncreasing = true;
        var isDecreasing = true;
        for (var i = 0; i < levels.Length - 1; i++)
        {
            if (levels[i] > levels[i + 1])
            {
                isIncreasing = false;
            }
            else if (levels[i] < levels[i + 1])
            {
                isDecreasing = false;
            }
            else if (levels[i] == levels[i + 1])
            {
                isIncreasing = false;
                isDecreasing = false;
                break;
            }

            var diff = Math.Abs(levels[i] - levels[i + 1]);
            if (diff is < 1 or > 3)
            {
                return false;
            }
        }

        var isSafe = isIncreasing || isDecreasing;
        return isSafe;
    }

    private static IEnumerable<int[]> GenerateSubsequences(int[] array)
    {
        yield return array;
        
        for (var i = 0; i < array.Length; i++)
        {
            yield return GetSubsequence(array, i);
        }
    }

    private static int[] GetSubsequence(int[] array, int skipIndex)
    {
        var subsequence = new int[array.Length - 1];
        for (var j = 0; j < array.Length; j++)
        {
            if (j < skipIndex)
            {
                subsequence[j] = array[j];
            }
            else if (j > skipIndex)
            {
                subsequence[j - 1] = array[j];
            }
        }

        return subsequence;
    }

    private static async IAsyncEnumerable<int[]> ReadAllLevelsAsync()
    {
        var lines = File.ReadLinesAsync(FileName);

        await foreach (var line in lines)
        {
            yield return line.Split(' ').Select(int.Parse).ToArray();
        }
    }
}