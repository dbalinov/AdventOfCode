namespace AdventOfCode.Year2019.Day2;

internal class Day2 : IDay
{
    private const string FileName = "Year2019/Day2/input.txt";

    public string Name => "Program Alarm";

    public async Task SolvePart1()
    {
        var program = await File.ReadAllTextAsync(FileName);

        var numbers = Parse(program);

        var output = ExecuteIntProgram(numbers, 12, 2);

        Console.WriteLine(output);
    }

    public async Task SolvePart2()
    {
        var program = await File.ReadAllTextAsync(FileName);
        
        for (var noun = 0; noun <= 99; noun++)
        {
            for (var verb = 0; verb <= 99; verb++)
            {
                var numbers = Parse(program);

                var output = ExecuteIntProgram(numbers, noun, verb);

                if (output == 19690720)
                {
                    var result = 100 * noun + verb;

                    Console.WriteLine(result);

                    return;
                }
            }
        }
    }

    private static int ExecuteIntProgram(IList<int> numbers, int noun, int verb)
    {
        numbers[1] = noun;
        numbers[2] = verb;

        for (var i = 0; i < numbers.Count; i += 4)
        {
            var result = numbers[i] switch
            {
                1 => numbers[numbers[i + 1]] + numbers[numbers[i + 2]],
                2 => numbers[numbers[i + 1]] * numbers[numbers[i + 2]],
                99 => -1, // halt
                _ => throw new NotImplementedException("Unknown opcode.")
            };

            if (result == -1)
            {
                break;
            }

            numbers[numbers[i + 3]] = result;
        }

        return numbers[0];
    }

    private static IList<int> Parse(ReadOnlySpan<char> line)
    {
        var numbers = new List<int>();

        var number = 0;
        foreach (var ch in line)
        {
            if (char.IsNumber(ch))
            {
                number *= 10;
                number += ch - '0';
            }
            else
            {
                numbers.Add(number);
                number = 0;
            }
        }

        numbers.Add(number);

        return numbers;
    }
}