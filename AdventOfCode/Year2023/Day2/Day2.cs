namespace AdventOfCode.Year2023.Day2;

/// <summary>
/// Cube Conundrum
/// </summary>
internal class Day2 : IDay
{
    private const string FileName = "Year2023/Day2/input.txt";
    
    public string Name => "Cube Conundrum";
    
    public async Task SolvePart1()
    {
        var games = new Dictionary<int, IList<(int red, int green, int blue)>>();
        
        var possibleSubset = (red: 12, green: 13, blue: 14);
        
        var lines = File.ReadLinesAsync(FileName);
        await foreach (var line in lines)
        {
            var lineParts = line.Split(':');
            var subsetsString = lineParts[1].Split(';');

            var isPossible = true;
            var game = new List<(int red, int green, int blue)>();
            foreach (var subsetString in subsetsString)
            {
                var (red, green, blue) = ParseSubset(subsetString);

                isPossible = red <= possibleSubset.red &&
                             green <= possibleSubset.green &&
                             blue <= possibleSubset.blue;
                if (!isPossible)
                {
                    break;
                }
            }

            if (isPossible)
            {
                var gameIndex = int.Parse(lineParts[0].Replace("Game ", ""));
                games.Add(gameIndex, game);
            }
        }
        
        var result = 0;
        foreach (var (index, subsets) in games)
        {
            if (subsets.All(x => x.red <= possibleSubset.red &&
                                 x.green <= possibleSubset.green &&
                                 x.blue <= possibleSubset.blue))
            {
                result += index;
            }
        }
        
        Console.WriteLine(result);
    }

    public async Task SolvePart2()
    {
        var result = 0;

        var lines = File.ReadLinesAsync(FileName);
        await foreach (var line in lines)
        {
            var lineParts = line.Split(':');
            
            var subsetsString = lineParts[1].Split(';');
            
            var maxRed = 0;
            var maxGreen = 0;
            var maxBlue = 0;
            foreach (var subsetString in subsetsString)
            {
                var (red, green, blue) = ParseSubset(subsetString);
                if (red > maxRed) { maxRed = red; }
                if (green > maxGreen) { maxGreen = green; }
                if (blue > maxBlue) { maxBlue = blue; }
            }
            
            result += maxRed * maxGreen * maxBlue;
        }
        
        Console.WriteLine(result);
    }

    private static (int red, int green, int blue) ParseSubset(string subsetString)
    {
        var subset = (red: 0, green: 0, blue: 0);
        var subsetParts = subsetString.Split(',');

        foreach (var subsetPart in subsetParts)
        {
            if (subsetPart.IndexOf("red", StringComparison.Ordinal) > -1)
            {
                subset.red = int.Parse(subsetPart.Replace("red", ""));
            }
            else if (subsetPart.IndexOf("green", StringComparison.Ordinal) > -1)
            {
                subset.green = int.Parse(subsetPart.Replace("green", ""));
            }
            else if (subsetPart.IndexOf("blue", StringComparison.Ordinal) > -1)
            {
                subset.blue = int.Parse(subsetPart.Replace("blue", ""));
            }
        }

        return subset;
    }
}