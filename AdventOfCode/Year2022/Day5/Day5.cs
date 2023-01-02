namespace AdventOfCode.Year2022.Day5;

internal sealed class Day5 : IDay
{
    private const string FileName = "Year2022/Day5/input.txt";

    public string Name => "Supply Stacks";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);
        var enumerator = lines.GetAsyncEnumerator();

        var stacks = await ParseStacks(enumerator);

        while (await enumerator.MoveNextAsync())
        {
            var line = enumerator.Current;
            if (line == string.Empty)
            {
                continue;
            }

            var lineParts = line.Split(' ');

            var count = int.Parse(lineParts[1]);
            var from = int.Parse(lineParts[3]);
            var to = int.Parse(lineParts[5]);

            var fromStack = stacks[from - 1];
            var toStack = stacks[to - 1];

            for (var i = 0; i < count; i++)
            {
                var item = fromStack[0];
                fromStack.RemoveAt(0);
                toStack.Insert(0, item);
            }
        }

        foreach (var stack in stacks)
        {
            Console.Write(stack[0]);
        }

        Console.WriteLine();
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);
        var enumerator = lines.GetAsyncEnumerator();

        var stacks = await ParseStacks(enumerator);

        while (await enumerator.MoveNextAsync())
        {
            var line = enumerator.Current;
            if (line == string.Empty)
            {
                continue;
            }

            var lineParts = line.Split(' ');

            var count = int.Parse(lineParts[1]);
            var from = int.Parse(lineParts[3]);
            var to = int.Parse(lineParts[5]);

            var fromStack = stacks[from - 1];
            var toStack = stacks[to - 1];


            for (var i = 0; i < count; i++)
            {
                var item = fromStack[count - 1 - i];
                fromStack.RemoveAt(count - 1 - i);
                toStack.Insert(0, item);
            }
        }

        foreach (var stack in stacks)
        {
            Console.Write(stack[0]);
        }

        Console.WriteLine();
    }
    
    private static async Task<List<List<char>>> ParseStacks(IAsyncEnumerator<string> enumerator)
    {
        var stacks = new List<List<char>>();

        while (await enumerator.MoveNextAsync())
        {
            var line = enumerator.Current;

            var stackIndex = 0;
            for (var crateIndex = 1; crateIndex < line.Length; crateIndex += 4)
            {
                var character = line[crateIndex];
                if (character is >= '1' and <= '9')
                {
                    return stacks;
                }

                if (stacks.Count < stackIndex + 1)
                {
                    stacks.Add(new List<char>());
                }

                if (character != ' ')
                {
                    stacks[stackIndex].Add(character);
                }

                stackIndex++;
            }
        }

        return stacks;
    }
}