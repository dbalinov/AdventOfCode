using System.Buffers;

namespace AdventOfCode.Year2023.Day4;

/// <summary>
/// Scratchcards
/// </summary>
internal class Day4 : IDay
{
    private const string FileName = "Year2023/Day4/input.txt";
    
    public string Name => "Scratchcards";
    
    public async Task SolvePart1()
    {
        var result = 0d;
        var lines = File.ReadLinesAsync(FileName);
        
        await foreach (var line in lines)
        {
            var count = GetMatches(line);
            result += (int)Math.Pow(2, count - 1);
        }
        
        Console.WriteLine(result);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var cardMatches = new List<int>();
        var cardCopies = new Dictionary<int, int>();
        var cardNumber = 1;
        await foreach (var line in lines)
        {
            var countOfMatches = GetMatches(line);
            cardMatches.Add(countOfMatches);
            
            cardCopies.Add(cardNumber, 1);
            cardNumber++;
        }

        cardNumber = 1;
        foreach (var matches in cardMatches)
        {
            for (var i = cardNumber + 1; i <= cardNumber + matches && i <= cardCopies.Count; i++)
            {
                cardCopies[i] += cardCopies[cardNumber];
            }
            
            cardNumber++;
        }

        var result = cardCopies.Values.Sum();
        
        Console.WriteLine(result);
    }
    
    private static int GetMatches(string line)
    {
        var indexOfColon = line.IndexOf(':');
        var indexOfVerticalLine = line.IndexOf('|');

        var winningNumbers = line
            .Substring(indexOfColon + 2, indexOfVerticalLine - indexOfColon - 3)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var allNumbers = line[(indexOfVerticalLine + 2)..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        var count = allNumbers.Count(x => winningNumbers.Contains(x));

        return count;
    }
}