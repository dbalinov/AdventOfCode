namespace AdventOfCode.Year2024.Day4;

public class Day4 : IDay
{
    private const string FileName = "Year2024/Day4/input.txt";

    public string Name => "Ceres Search";
    
    public async Task SolvePart1()
    {
        var lines = await File.ReadAllLinesAsync(FileName);

        const string word = "XMAS";
    
        var directions = new int[][]
        {
            [0, 1], // left to right
            [1, 0], // top to bottom
            [1, 1], // diagonal 1
            [1, -1], // diagonal 2
            [-1, 0], // bottom to top
            [0, -1], // right to left
            [-1, -1], // diagonal 3
            [-1, 1] // diagonal 4
        };

        var count = 0;
        foreach (var direction in directions)
        {
            var dx = direction[0];
            var dy = direction[1];

            for(var i = 0; i < lines.Length; i++)
            {
                for(var j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == word[0])
                    {
                        var found = true;
                        for (var k = 1; k < word.Length; k++)
                        {
                            if (i + k * dx >= lines.Length ||
                                j + k * dy >= lines[i].Length ||
                                i + k * dx < 0 ||
                                j + k * dy < 0 ||
                                lines[i + k * dx][j + k * dy] != word[k])
                            {
                                found = false;
                                break;
                            }
                        }
                    
                        if (found)
                        {
                            count++;
                        }
                    }
                }
            }
        }
        
        Console.WriteLine(count);
    }

    public async Task SolvePart2()
    {
        var lines = await File.ReadAllLinesAsync(FileName);

        const string word = "MAS";
    
        var directions = new int[][]
        {
            [1, 1], // diagonal 1
            [1, -1], // diagonal 2
            [-1, -1], // diagonal 3
            [-1, 1] // diagonal 4
        };

        var dictionary = new Dictionary<(int x, int y), int>();
        foreach (var direction in directions)
        {
            var dx = direction[0];
            var dy = direction[1];

            for(var i = 0; i < lines.Length; i++)
            {
                for(var j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == word[0])
                    {
                        var found = true;
                        for (var k = 1; k < word.Length; k++)
                        {
                            if (i + k * dx >= lines.Length ||
                                j + k * dy >= lines[i].Length ||
                                i + k * dx < 0 ||
                                j + k * dy < 0 ||
                                lines[i + k * dx][j + k * dy] != word[k])
                            {
                                found = false;
                                break;
                            }
                        }
                    
                        if (found)
                        {
                            if (!dictionary.ContainsKey((i + 1 * dx, j + 1 * dy)))
                            {
                                dictionary[(i + 1 * dx, j + 1 * dy)] = 1;
                            }
                            else
                            {
                                dictionary[(i + 1 * dx, j + 1 * dy)]++;
                            }
                        }
                    }
                }
            }
        }
        
        var count = dictionary.Values.Count(x => x == 2);
        Console.WriteLine(count);
    }
}