namespace AdventOfCode.Year2022.Day2;

/// <summary>
/// Rock Paper Scissors
/// </summary>
internal sealed class Day2 : IDay
{
    private const string FileName = "Year2022/Day2/input.txt";
    private static readonly IReadOnlyDictionary<char, int> Shapes = new Dictionary<char, int>
    {
        {'A', 1}, // Rock
        {'B', 2}, // Paper
        {'C', 3}, // Scissors
        {'X', 1}, // Rock
        {'Y', 2}, // Paper
        {'Z', 3}  // Scissors
    };

    public async Task SolvePart1()
    {
        var score = 0;

        var lines = File.ReadLinesAsync(FileName);

        await foreach(var line in lines)
        {
            score += GetScore(line[0], line[2]);
        }

        Console.WriteLine(score);
    }

    public async Task SolvePart2()
    {
        var score = 0;

        var lines = File.ReadLinesAsync(FileName);
        await foreach (var line in lines)
        {
            var p2 = GetAction(line[0], line[2]);
            score += GetScore(line[0], p2);
        }

        Console.WriteLine(score);
    }

    private static int GetScore(char p1, char p2)
    {
        var roundScore = 0;

        if (Shapes[p1] == Shapes[p2])
        {
            roundScore = 3;
        }
        else if (Shapes[p1] % 3 == Shapes[p2] - 1)
        {
            roundScore = 6;
        }

        var shapeScore = Shapes[p2];
        return shapeScore + roundScore;
    }

    private static char GetAction(char p1, char p2)
    {
        if (p2 == 'Y')
        {
            return (char)(p1 + 23);
        }
        if (p2 == 'X')
        {
            // lose
            return p1 switch
            {
                'A' => 'Z', // Rock => Scissors
                'B' => 'X', // Paper => Rock
                'C' => 'Y' // Scissors => paper
            };
        }

        // win
        return p1 switch
        {
            'A' => 'Y', // Rock => Paper
            'B' => 'Z', // Paper => Scissors
            'C' => 'X' // Scissors => Rock
        };
    }
}