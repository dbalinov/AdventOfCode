using System.Collections.Concurrent;

namespace AdventOfCode.Year2019;

public class IntcodeComputer
{
    private readonly IList<int> _instructions;

    public IntcodeComputer(IList<int> instructions)
        => _instructions = instructions;

    public static IntcodeComputer FromCode(string code)
    {
        var instructions = Parse(code);

        return new IntcodeComputer(instructions);
    }

    public Task RunAsync(BlockingCollection<int> input, BlockingCollection<int> output)
        => Task.Run(() => Run(input, output));

    private void Run(BlockingCollection<int> input, BlockingCollection<int> output)
    {
        for (var i = 0; i < _instructions.Count;)
        {
            var (a, b, c, de) = GetOpCode(_instructions[i]);

            var param1 = (de is >= 1 and <= 8) && c == 0 ? _instructions[i + 1] : i + 1;
            var param2 = (de is 1 or 2 or >= 5 and <= 8) && b == 0 ? _instructions[i + 2] : i + 2;
            var resultIndex = (de is 1 or 2 or >= 5 and <= 8) && a == 0 ? _instructions[i + 3] : i + 3;

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
                    //yield return _instructions[param1];
                    i += 2;
                    break;
                case 5: // jump -if-true
                    i = _instructions[param1] != 0 ? _instructions[param2] : i + 3;
                    break;
                case 6: // jump -if-false
                    i = _instructions[param1] == 0 ? _instructions[param2] : i + 3;
                    break;
                case 7: // less then
                    _instructions[resultIndex] = (_instructions[param1] < _instructions[param2]) ? 1 : 0;
                    i += 4;
                    break;
                case 8: // equals
                    _instructions[resultIndex] = (_instructions[param1] == _instructions[param2]) ? 1 : 0;
                    i += 4;
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

    public static IList<int> Parse(ReadOnlySpan<char> line)
    {
        var numbers = new List<int>();

        var multiplier = 1;
        var number = 0;
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