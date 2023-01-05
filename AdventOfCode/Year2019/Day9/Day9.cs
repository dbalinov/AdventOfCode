using System.Collections.Concurrent;

namespace AdventOfCode.Year2019.Day9;

internal sealed class Day9 : IDay
{
    private const string FileName = "Year2019/Day9/input.txt";

    public string Name => "Sensor Boost";

    public Task SolvePart1()
        => Solve(1);

    public Task SolvePart2() 
        => Solve(2);

    private static async Task Solve(int input)
    {
        var code = await File.ReadAllTextAsync(FileName);

        var computer = IntcodeComputer9.FromCode(code);

        var inputs = new BlockingCollection<long> { input };
        var outputs = new BlockingCollection<long>();

        await computer.RunAsync(inputs, outputs);

        var output = outputs.Take();

        Console.WriteLine(output);
    }
}