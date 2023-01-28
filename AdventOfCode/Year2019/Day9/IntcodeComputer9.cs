using System.Collections.Concurrent;

namespace AdventOfCode.Year2019.Day9;

public class IntcodeComputer9
{
    private readonly IList<long> _instructions;

    public IntcodeComputer9(IList<long> instructions)
        => _instructions = instructions;

    public static IntcodeComputer9 FromCode(string code)
    {
        var instructions = Parse(code);

        return new IntcodeComputer9(instructions);
    }

    public Task RunAsync(BlockingCollection<long> input, BlockingCollection<long> output)
        => Task.Run(() => Run(input, output));

    private void Run(BlockingCollection<long> input, BlockingCollection<long> output)
    {
        var relativeBase = 0L;

        for (var i = 0; i < _instructions.Count;)
        {
            var (a, b, c, de) = GetOpCode(_instructions[(int)i]);

            var param1 = (de is >= 1 and <= 9) && c == 0
                    ? (int)_instructions[(int)i + 1]
                    : c == 1
                        ? i + 1
                        : (int)(relativeBase + _instructions[(int)i + 1]);

            var param2 = (de is 1 or 2 or >= 5 and <= 9) && b == 0
                    ? (int)_instructions[(int)i + 2]
                    : b == 1
                        ? i + 2
                        : (int)(relativeBase + _instructions[(int)i + 2]);

            var resultIndex = (de is 1 or 2 or >= 5 and <= 9) && a == 0
                    ? (int)_instructions[(int)i + 3]
                    : a == 1
                        ? i + 3
                        : (int)(relativeBase + _instructions[(int)i + 3]);

            if (param1 > _instructions.Count)
            {
                ExtendMemory(param1 - _instructions.Count);
            }

            if (param2 > _instructions.Count)
            {
                ExtendMemory(param2 - _instructions.Count);
            }

            if (resultIndex > _instructions.Count)
            {
                ExtendMemory(resultIndex - _instructions.Count);
            }

            switch (de)
            {
                case 1: // add
                    _instructions[resultIndex] = _instructions[param1] + _instructions[param2];
                    i += 4;
                    break;
                case 2: // multiply
                    _instructions[resultIndex] = _instructions[param1] * _instructions[param2];
                    i += 4;
                    break;
                case 3: // input
                    _instructions[param1] = input.Take();
                    i += 2;
                    break;
                case 4: // output
                    output.Add(_instructions[param1]);
                    i += 2;
                    break;
                case 5: // jump -if-true
                    i = _instructions[param1] != 0 ? (int)_instructions[param2] : i + 3;
                    break;
                case 6: // jump -if-false
                    i = _instructions[param1] == 0 ? (int)_instructions[param2] : i + 3;
                    break;
                case 7: // less then
                    _instructions[resultIndex] = (_instructions[param1] < _instructions[param2]) ? 1 : 0;
                    i += 4;
                    break;
                case 8: // equals
                    _instructions[resultIndex] = (_instructions[param1] == _instructions[param2]) ? 1 : 0;
                    i += 4;
                    break;
                case 9: // adjust relative base
                    relativeBase += _instructions[param1];
                    i += 2;
                    break;
                case 99:
                    output.CompleteAdding();
                    return;
                default:
                    throw new NotImplementedException("Unknown opcode.");
            }
        }

        output.CompleteAdding();
    }

    private void ExtendMemory(int count)
    {
        for (var i = 0; i < count; i++)
        {
            _instructions.Add(0L);
        }
    }

    private static (int a, int b, int c, int de) GetOpCode(long number)
    {
        var opCode = new int[5];

        for (var j = opCode.Length - 1; j >= 0; j--)
        {
            opCode[j] = (int)(number % 10);
            number /= 10;
        }

        var a = opCode[0];
        var b = opCode[1];
        var c = opCode[2];
        var de = 10 * opCode[3] + opCode[4];

        return (a, b, c, de);
    }

    public static IList<long> Parse(ReadOnlySpan<char> line)
    {
        var numbers = new List<long>();

        var multiplier = 1L;
        var number = 0L;
        foreach (var ch in line)
        {
            if (ch == '-')
            {
                multiplier *= -1;
            }
            else if (char.IsNumber(ch))
            {
                number *= 10;
                number += ch - '0';
            }
            else
            {
                numbers.Add(multiplier * number);
                multiplier = 1;
                number = 0;
            }
        }

        numbers.Add(multiplier * number);

        return numbers;
    }
}