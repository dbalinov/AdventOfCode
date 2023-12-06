using System.Numerics;

namespace AdventOfCode.Year2023.Day6;

/// <summary>
/// Wait For It
/// </summary>
internal class Day6 : IDay
{
    private const string FileName = "Year2023/Day6/input.txt";
    
    public string Name => "Wait For It";
    
    public async Task SolvePart1()
    {
        var lines = await File.ReadAllLinesAsync(FileName);
        
        var times = ParseLine1(lines[0]);
        var distances = ParseLine1(lines[1]);

        var result = 1;
        for (var i = 0; i < times.Length; i++)
        {
            var time = times[i];
            var minDistance = distances[i];

            var valid = 0;
            for (var holdTime = 0; holdTime <= time; holdTime++)
            {
                var travelTime = time - holdTime;
                var speed = holdTime;
                var distance = speed * travelTime;
                
                if (distance > minDistance)
                {
                    valid++;
                }
            }

            result *= valid;
        }
        
        Console.WriteLine(result);
    }

    public async Task SolvePart2()
    {
        var lines = await File.ReadAllLinesAsync(FileName);
        
        var time = ParseLine2(lines[0]);
        var minDistance = ParseLine2(lines[1]);
        
        var valid = 0;
        for (var holdTime = 0; holdTime <= time; holdTime++)
        {
            var travelTime = time - holdTime;
            var speed = holdTime;
            var distance = speed * travelTime;
            
            if (distance > minDistance)
            {
                valid++;
            }
        }
        
        Console.WriteLine(valid);
    }
    
    private static int[] ParseLine1(string line)
    {
        var lineParts = line.Split(':');

        return lineParts[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
    }
    
    private static BigInteger ParseLine2(string line)
    {
        var lineParts = line.Split(':');
        return BigInteger.Parse(lineParts[1].Replace(" ", ""));
    }
}