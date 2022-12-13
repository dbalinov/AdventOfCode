using System.Collections;

namespace AdventOfCode.Year2022.Day13;

internal class Day13 : IDay, IComparer<IList>
{
    private const string FileName = "Year2022/Day13/input.txt";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);
        var pairs = GetPairs(lines);
        
        var sum = 0;
        var i = 0;
        await foreach (var (first, second) in pairs)
        {
            i++;
            if (Compare(first, second) == -1)
            {
                sum += i;
            }
        }

        Console.WriteLine(sum);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var dividerPackets = new[]
            {
                "[[2]]",
                "[[6]]"
            }
            .Select(x => Parse(x))
            .ToList();

        var packets = new List<IList>();
        packets.AddRange(dividerPackets);

        await foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            packets.Add(Parse(line));
        }

        packets.Sort(this);

        var decoderKey = dividerPackets
            .Select(divider => packets.IndexOf(divider) + 1)
            .Aggregate(1, (acc, val) => acc * val);

        Console.WriteLine(decoderKey);
    }

    public int Compare(IList first, IList second)
    {
        var length = Math.Max(first.Count, second.Count);

        for (var i = 0; i < length; i++)
        {
            if (i == first.Count && first.Count < second.Count)
            {
                return -1;
            }

            if (i == second.Count && second.Count < first.Count)
            {
                return 1;
            }

            var number1 = first[i] as int?;
            var number2 = second[i] as int?;

            if (number1.HasValue && number2.HasValue)
            {
                if (number1 < number2)
                {
                    return -1;
                }
                if (number1 > number2)
                {
                    return 1;
                }
            }
            else
            {
                var list1 = first[i] as IList;
                var list2 = second[i] as IList;

                if (number1.HasValue && list2 != null || number2.HasValue && list1 != null)
                {
                    list1 ??= new ArrayList { number1 };
                    list2 ??= new ArrayList { number2 };
                }

                var compare = Compare(list1, list2);
                if (compare != 0)
                {
                    return compare;
                }
            }
        }

        return 0;
    }

    private static async IAsyncEnumerable<(IList, IList)> GetPairs(IAsyncEnumerable<string> lines)
    {
        IList? first = null;
        IList? second = null;

        await foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (first == null)
            {
                first = Parse(line);
            }
            else if (second == null)
            {
                second = Parse(line);
                yield return (first, second);

                first = null;
                second = null;
            }
        }
    }

    private static IList Parse(ReadOnlySpan<char> line)
    {
        var i = 1;
        return Parse(line, ref i);
    }

    private static IList Parse(ReadOnlySpan<char> line, ref int i)
    {
        var array = new ArrayList();
        var number = 0;
        var isNumber = false;
        for (; i < line.Length; i++)
        {
            var ch = line[i];
            if (ch == '[')
            {
                i++;
                var nested = Parse(line, ref i);
                array.Add(nested);
            }
            else if (char.IsDigit(ch))
            {
                isNumber = true;
                number *= 10;
                number += ch - '0';
            }
            else if (ch is ',' or ']')
            {
                if (isNumber)
                {
                    array.Add(number);
                    number = 0;
                    isNumber = false;
                }

                if (ch == ']')
                {
                    return array;
                }
            }
        }

        return array;
    }
}