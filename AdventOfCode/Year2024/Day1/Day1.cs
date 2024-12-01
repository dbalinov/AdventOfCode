namespace AdventOfCode.Year2024.Day1;

public class Day1 : IDay
{
    private const string FileName = "Year2024/Day1/input.txt";

    public string Name => "Historian Hysteria";

    public async Task SolvePart1()
    {
        var (locations1, locations2) = await ParseLocations();
        
        locations1.Sort();
        locations2.Sort();

        var totalDistance = 0;
        for (var i = 0; i < locations1.Count; i++)
        {
            totalDistance += Math.Abs(locations1[i] - locations2[i]);
        }
        
        Console.WriteLine(totalDistance);
    }

    public async Task SolvePart2()
    {
        var (locations1, locations2) = await ParseLocations();
        
        var similarityScore = 0;
        foreach (var location1 in locations1)
        {
            var count = 0;
            foreach (var location2 in locations2)
            {
                if (location1 == location2)
                {
                    count++;
                }
            }
            
            similarityScore += location1 * count;
        }
        
        Console.WriteLine(similarityScore);
    }

    private static async Task<(List<int> locations1, List<int> locations2)> ParseLocations()
    {
        var lines = await File.ReadAllLinesAsync(FileName);
        
        var locations1 = new List<int>();
        var locations2 = new List<int>();
        
        foreach (var line in lines)
        {
            var span = line.AsSpan();
            var spaceIndex = span.IndexOf(' ');

            while (spaceIndex >= 0 && span[spaceIndex] == ' ')
            {
                spaceIndex++;
            }

            var firstNumber = int.Parse(span.Slice(0, spaceIndex));
            var secondNumber = int.Parse(span.Slice(spaceIndex));

            locations1.Add(firstNumber);
            locations2.Add(secondNumber);
        }
        
        return (locations1, locations2);
    }
}