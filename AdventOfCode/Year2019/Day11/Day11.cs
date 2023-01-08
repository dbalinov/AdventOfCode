using AdventOfCode.Year2019.Day9;
using System.Collections.Concurrent;
using System.Numerics;

namespace AdventOfCode.Year2019.Day11;

internal sealed class Day11 : IDay
{
    private const string FileName = "Year2019/Day11/input.txt";

    public string Name => "Space Police";

    public async Task SolvePart1()
    {
        var code = await File.ReadAllTextAsync(FileName);

        var computer = IntcodeComputer9.FromCode(code);

        var inputs = new BlockingCollection<long>();
        var outputs = new BlockingCollection<long>();
        var computerTask = computer.RunAsync(inputs, outputs);

        var black = new HashSet<Complex>();
        var white = new HashSet<Complex>();
        var paintTask = Task.Run(() => PaintAsync(inputs, outputs, white, black));

        await Task.WhenAll(computerTask, paintTask);

        var countOfPainted = white.Count + black.Count;

        Console.WriteLine(countOfPainted);
    }

    public async Task SolvePart2()
    {
        var code = await File.ReadAllTextAsync(FileName);

        var computer = IntcodeComputer9.FromCode(code);

        var inputs = new BlockingCollection<long>();
        var outputs = new BlockingCollection<long>();
        var computerTask = computer.RunAsync(inputs, outputs);

        var black = new HashSet<Complex>();
        var white = new HashSet<Complex> { new(0, 0) };
        var paintTask = Task.Run(() => PaintAsync(inputs, outputs, white, black));

        await Task.WhenAll(computerTask, paintTask);

        Print(white);
    }

    private static void PaintAsync(BlockingCollection<long> inputs,
        BlockingCollection<long> outputs, ISet<Complex> white, ISet<Complex> black)
    {
        var start = new Complex(0, 0);

        var currentPosition = start;
        var currentDirection = new Complex(0, 1);
        while (!outputs.IsCompleted)
        {
            try
            {
                var input = white.Contains(currentPosition)
                    ? (int)Color.White
                    : (int)Color.Black;

                inputs.Add(input);

                var color = outputs.Take();
                var dir = outputs.Take();

                if (color == (int)Color.White)
                {
                    black.Remove(currentPosition);
                    white.Add(currentPosition);
                }
                else
                {
                    white.Remove(currentPosition);
                    black.Add(currentPosition);
                }

                currentDirection *= dir switch
                {
                    0 => new Complex(0, 1),
                    1 => new Complex(0, -1),
                    _ => throw new InvalidOperationException("Unknown direction.")
                };

                currentPosition += currentDirection;
            }
            catch (InvalidOperationException)
            {
                break;
            }
        }
    }

    private static void Print(ICollection<Complex> white)
    {
        Console.SetCursorPosition(0, 0);
        var minX = white.Min(x => x.Real) - 1;
        var maxX = white.Max(x => x.Real) + 1;
        var minY = white.Min(x => x.Imaginary) - 1;
        var maxY = white.Max(x => x.Imaginary) + 1;

        for (var i = minY; i <= maxY; i++)
        {
            for (var j = minX; j <= maxX; j++)
            {
                Console.Write('.');
            }

            Console.WriteLine();
        }

        foreach (var item in white)
        {
            Console.SetCursorPosition(
                -1 * (int)minX + (int)item.Real,
                ((int)maxY - (int)item.Imaginary));

            Console.Write('█');
        }
    }
}