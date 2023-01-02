namespace AdventOfCode.Year2022.Day22;

public sealed class Day22 : IDay
{
    private const string FileName = "Year2022/Day22/input.txt";

    public string Name => "Monkey Map";

    public async Task SolvePart1()
    {
        var lines = await File.ReadAllLinesAsync(FileName);

        var currentRow = 0;
        var currentCol = 0;
        var currentFacing = Direction.Right;

        for (var col = 0; col < lines[currentRow].Length; col++)
        {
            if (lines[currentRow][col] != ' ')
            {
                currentCol = col;
                break;
            }
        }

        var path = ParsePath(lines[^1]);

        foreach (var command in path)
        {
            if (command is char facingCommand)
            {
                currentFacing = GetFacing(facingCommand, currentFacing);
                continue;
            }

            var moveCommand = (int) command;

            for (var i = 0; i < moveCommand; i++)
            {
                var nextRow = currentRow;
                var nextCol = currentCol;
                if (currentFacing == Direction.Right)
                {
                    if (currentCol == lines[currentRow].Length - 1 ||
                        lines[currentRow][currentCol + 1] == ' ')
                    {
                        for (var col = 0; col < lines[currentRow].Length; col++)
                        {
                            if (lines[currentRow][col] != ' ')
                            {
                                nextCol = col;
                                break;
                            }
                        }
                    }
                    else
                    {
                        nextCol = currentCol + 1;
                    }
                }
                else if(currentFacing == Direction.Down)
                {
                    if (currentRow == lines.Length - 2 - 1 ||
                        lines[currentRow + 1].Length - 1 < currentCol ||
                        lines[currentRow + 1][currentCol] == ' ')
                    {
                        for (var row = 0; row < lines.Length - 2; row++)
                        {
                            if (lines[row].Length < currentCol)
                            {
                                continue;
                            }

                            if (lines[row][currentCol] != ' ')
                            {
                                nextRow = row;
                                break;
                            }
                        }
                    }
                    else
                    {
                        nextRow = currentRow + 1;
                    }
                }
                else if (currentFacing == Direction.Left)
                {
                    if (currentCol == 0 ||
                        lines[currentRow][currentCol - 1] == ' ')
                    {
                        for (var col = lines[currentRow].Length - 1; col >= 0; col--)
                        {
                            if (lines[currentRow][col] != ' ')
                            {
                                nextCol = col;
                                break;
                            }
                        }
                    }
                    else
                    {
                        nextCol = currentCol - 1;
                    }

                }
                else if (currentFacing == Direction.Up)
                {
                    if (currentRow == 0 ||
                        lines[currentRow - 1][currentCol] == ' ')
                    {
                        for (var row = lines.Length - 2 - 1; row >= 0; row--)
                        {
                            if (lines[row].Length -1 < currentCol)
                            {
                                continue;
                            }

                            if (lines[row][currentCol] != ' ')
                            {
                                nextRow = row;
                                break;
                            }
                        }
                    }
                    else
                    {
                        nextRow = currentRow - 1;
                    }
                }

                var canMove = lines[nextRow][nextCol] != '#';
                if (!canMove)
                {
                    break;
                }

                currentRow = nextRow;
                currentCol = nextCol;
            }
        }

        var password = 1000 * (currentRow + 1) + 4 * (currentCol + 1) + (int)currentFacing;

        Console.WriteLine(password);
    }

    public Task SolvePart2()
    {
        return Task.CompletedTask;
    }

    private static Direction GetFacing(char command, Direction oldFacing)
    {
        var isClockwise = command == 'R';

        var newFacing = isClockwise ? oldFacing + 1 : oldFacing - 1;

        return (Direction)Mod((int)newFacing, 4);
    }

    private static int Mod(int a, int b) => ((a % b) + b) % b;

    private static IEnumerable<object> ParsePath(ReadOnlySpan<char> line)
    {
        var list = new List<object>();
        var number = 0;
        foreach (var c in line)
        {
            if (char.IsNumber(c))
            {
                number *= 10;
                number += c - '0';
            }
            else
            {
                list.Add(number);
                number = 0;

                list.Add(c);
            }
        }

        if (number > 0)
        {
            list.Add(number);
        }

        return list;
    }
}