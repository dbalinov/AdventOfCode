namespace AdventOfCode.Year2022.Day17;

/// <summary>
/// Pyroclastic Flow
/// </summary>
internal class Day17 : IDay
{
    private const string FileName = "Year2022/Day17/test-input.txt";
    private const char Left = '<';
    private const char Right = '>';

    public string Name => "Pyroclastic Flow";

    public async Task SolvePart1()
    {
        var input = await File.ReadAllTextAsync(FileName);
        input = input.Trim();
        
        using var nextRockEnumerator = GetNextRock().GetEnumerator();
        using var nextDirectionEnumerator = GetNextDirection(input).GetEnumerator();

        var chamber = new char[4000, 7];

        var stoppedRocks = 0;

        int[,]? rock = null;
        
        var rowOffset = 0;
        var colOffset = 0;
        var maxHeight = 0;

        while (stoppedRocks < 2022)
        {
            if (rock == null)
            {
                rowOffset = maxHeight + 3;
                colOffset = 2;

                nextRockEnumerator.MoveNext();
                rock = nextRockEnumerator.Current;
            }

            nextDirectionEnumerator.MoveNext();
            var direction = nextDirectionEnumerator.Current;
            
            if (direction == Right && colOffset + rock.GetLength(1) + 1 < 8 &&
                CanMove(chamber, rock, rowOffset, colOffset, (row: 0, col: 1)))
            {
                colOffset++;
            }
            else if (direction == Left && colOffset - 1 >= 0 &&
                     CanMove(chamber, rock, rowOffset, colOffset, (row: 0, col: -1)))
            {
                colOffset--;
            }

            if (rowOffset - 1 >= 0 &&
                CanMove(chamber, rock, rowOffset, colOffset, (row: -1, col: 0)))
            {
                rowOffset--;
            }
            else
            {
                for (var row = 0; row < rock.GetLength(0); row++)
                {
                    for (var col = 0; col < rock.GetLength(1); col++)
                    {
                        if (rock[row, col] == 1)
                        {
                            chamber[rowOffset + row, colOffset + col] = '#';
                        }
                    }
                }

                var height = rowOffset + rock.GetLength(0);
                if (height > maxHeight)
                {
                    maxHeight = height;
                }

                stoppedRocks++;
                rock = null;
            }
        }

        Console.WriteLine(maxHeight);
    }

    public Task SolvePart2()
    {
        throw new NotImplementedException();
    }

    private static bool CanMove(char[,] chamber, int[,] rock, int rowOffset, int colOffset, (int row, int col) vector)
    {
        for (var row = 0; row < rock.GetLength(0); row++)
        {
            for (var col = 0; col < rock.GetLength(1); col++)
            {
                if (rock[row, col] == 0)
                {
                    continue;
                }

                if (chamber[rowOffset + row + vector.row, colOffset + col + vector.col] == '#')
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static IEnumerable<int[,]> GetNextRock()
    {
        var rockTypes = new[]
        {
            new[,]
            {
                {1, 1, 1, 1}
            },
            new[,]
            {
                {0, 1, 0},
                {1, 1, 1},
                {0, 1, 0}
            },
            new[,]
            {
                {1, 1, 1},
                {0, 0, 1},
                {0, 0, 1},
                //{1, 1, 1}
            },
            new[,]
            {
                {1},
                {1},
                {1},
                {1}
            },
            new[,]
            {
                {1, 1},
                {1, 1},
            }
        };

        var i = 0;
        while (true)
        {
            yield return rockTypes[i];
            i = (i + 1) % rockTypes.Length;
        }
    }

    private static IEnumerable<char> GetNextDirection(string input)
    {
        var i = 0;
        while (true)
        {
            var ch = input[i];
            if (ch is Left or Right)
            {
                yield return ch;
            }

            i = (i + 1) % input.Length;
        }
    }
}