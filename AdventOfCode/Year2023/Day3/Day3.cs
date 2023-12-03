namespace AdventOfCode.Year2023.Day3;

/// <summary>
/// Gear Ratios
/// </summary>
internal class Day3 : IDay
{
    private const string FileName = "Year2023/Day3/input.txt";
    
    public string Name => "Gear Ratios";
    
    public async Task SolvePart1()
    {
        var result = 0;

        var lines = await File.ReadAllLinesAsync(FileName);

        for (var row = 0; row < lines.Length; row++)
        {
            var line = lines[row];
            
            var startCol = -1;
            var endCol = -1;

            for (var col = 0; col < line.Length; col++)
            {
                if (char.IsDigit(line[col]))
                {
                    if (startCol == -1)
                    {
                        startCol = col;
                    }
        
                    endCol = col;
                    
                    if (col != line.Length - 1)
                    {
                        continue;
                    }
                }

                if (startCol == -1 || endCol == -1)
                {
                    continue;
                }

                var isPartNumber = IsPartNumber(lines, row, startCol, endCol);
                if (isPartNumber)
                {
                    var length = endCol - startCol + 1;
                    var number = int.Parse(line.AsSpan().Slice(startCol, length));

                    result += number;
                }
                
                startCol = -1;
                endCol = -1;
            }
        }
        
        Console.WriteLine(result);
    }

    private static bool IsPartNumber(IReadOnlyList<string> lines, int currentRow, int startCol, int endCol)
    {
        var fromRow = currentRow == 0 ? 0 : currentRow - 1;
        var toRow = currentRow == lines.Count - 1 ? lines.Count - 1 : currentRow + 1;
        
        var fromCol = startCol == 0 ? 0 : startCol - 1;
        var toCol = endCol == lines[currentRow].Length - 1 ? lines[currentRow].Length - 1 : endCol + 1;
        
        for (var row = fromRow; row <= toRow; row++)
        {
            for (var col = fromCol; col <= toCol; col++)
            {
                if (row == currentRow && (startCol <= col && col <= endCol))
                {
                    continue;
                }
                
                var ch = lines[row][col];
                if (ch != '.')
                {
                    return true;
                }
            }
        }

        return false;
    }

    public async Task SolvePart2()
    {
        var gears = new Dictionary<(int row, int col), IList<int>>();
        var lines = await File.ReadAllLinesAsync(FileName);

        for (var row = 0; row < lines.Length; row++)
        {
            var line = lines[row];
            
            var startCol = -1;
            var endCol = -1;

            for (var col = 0; col < line.Length; col++)
            {
                if (char.IsDigit(line[col]))
                {
                    if (startCol == -1)
                    {
                        startCol = col;
                    }
        
                    endCol = col;
                    
                    if (col != line.Length - 1)
                    {
                        continue;
                    }
                }

                if (startCol == -1 || endCol == -1)
                {
                    continue;
                }

                var gear = GetGear(lines, row, startCol, endCol);
                
                if (gear is not null)
                {
                    var length = endCol - startCol + 1;
                    var number = int.Parse(line.AsSpan().Slice(startCol, length));

                    if (gears.ContainsKey(gear.Value))
                    {
                        gears[gear.Value].Add(number);
                    }
                    else
                    {
                        gears.Add(gear.Value, new List<int> { number });
                    }
                }
                
                startCol = -1;
                endCol = -1;
            }
        }
        
        
        var result = 0;
        foreach (var numbers in gears.Values)
        {
            if (numbers.Count == 2)
            {
                result += numbers[0] * numbers[1];
            }
        }

        Console.WriteLine(result);
    }
    
    private static (int row, int col)? GetGear(IReadOnlyList<string> lines, int currentRow, int startCol, int endCol)
    {
        var fromRow = currentRow == 0 ? 0 : currentRow - 1;
        var toRow = currentRow == lines.Count - 1 ? lines.Count - 1 : currentRow + 1;
        
        var fromCol = startCol == 0 ? 0 : startCol - 1;
        var toCol = endCol == lines[currentRow].Length - 1 ? lines[currentRow].Length - 1 : endCol + 1;
        
        for (var row = fromRow; row <= toRow; row++)
        {
            for (var col = fromCol; col <= toCol; col++)
            {
                if (row == currentRow && (startCol <= col && col <= endCol))
                {
                    continue;
                }
                
                var ch = lines[row][col];
                if (ch == '*')
                {
                    return (row, col);
                }
            }
        }

        return null;
    }
}