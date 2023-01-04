using System.Collections.Concurrent;

namespace AdventOfCode.Year2019.Day5;

public sealed class Day5 : IDay
{
    private const string FileName = "Year2019/Day5/input.txt";

    public string Name => "Sunny with a Chance of Asteroids";

    public async Task SolvePart1()
    {
        var line = await File.ReadAllTextAsync(FileName);

        var numbers = IntcodeComputer.Parse(line);

        var output = ExecuteIntProgram(numbers, 1);

        Console.WriteLine(output);
    }

    public async Task SolvePart2()
    {
        var code = await File.ReadAllTextAsync(FileName);

        var inputs = new BlockingCollection<int> { 5 };
        var outputs = new BlockingCollection<int>();

        var amplifier = IntcodeComputer.FromCode(code);

        await amplifier.RunAsync(inputs, outputs);

        var output = outputs.Take();

        Console.WriteLine(output);
    }

    private static int ExecuteIntProgram(IList<int> numbers, int input)
    {
        var output = 0;
        for (var i = 0; i < numbers.Count;)
        {
            var opCode = GetOpCode(numbers[i]);

            var param1 = (opCode.de is 1 or 2 or 3 or 4) && opCode.c == 0 ? numbers[i + 1] : i + 1;
            var param2 = (opCode.de is 1 or 2) && opCode.b == 0 ? numbers[i + 2] : i + 2;
            var resultIndex = (opCode.de is 1 or 2) && opCode.a == 0 ? numbers[i + 3] : i + 3;

            switch (opCode.de)
            {
                case 1: // add
                    numbers[resultIndex] = numbers[param1] + numbers[param2];
                    i += 4;
                    break;
                case 2: // multiply
                    numbers[resultIndex] = numbers[param1] * numbers[param2];
                    i += 4;
                    break;
                case 3: // input
                    numbers[param1] = input;
                    i += 2;
                    break;
                case 4: // output
                    output = numbers[param1];
                    i += 2;
                    break;
                case 99:
                    return output;
                default:
                    throw new NotImplementedException("Unknown opcode.");
            }
        }

        return output;
    }

    private static (int a, int b, int c, int de) GetOpCode(int number)
    {
        var opCode = new int[5];

        for (var j = opCode.Length - 1; j >= 0; j--)
        {
            opCode[j] = number % 10;
            number /= 10;
        }

        var a = opCode[0];
        var b = opCode[1];
        var c = opCode[2];
        var de = 10 * opCode[3] + opCode[4];

        return (a, b, c, de);
    }
}