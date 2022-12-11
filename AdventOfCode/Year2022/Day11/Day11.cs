namespace AdventOfCode.Year2022.Day11;

internal sealed class Day11 : IDay
{
    private const string FileName = "Year2022/Day11/input.txt";

    public async Task SolvePart1()
    {
        var (items, operations, arguments, modules, trueReceivers, falseReceivers) = await ParseAsync(FileName);

        var monkeyBusiness = GetMonkeyBusiness(
            items, operations, arguments, modules, trueReceivers, falseReceivers, 20,
            x => (long)Math.Floor(x / 3d));

        Console.WriteLine(monkeyBusiness);
    }

    public async Task SolvePart2()
    {
        var (items, operations, arguments, modules, trueReceivers, falseReceivers) = await ParseAsync(FileName);

        var moduleProduct = modules.Aggregate(1, (acc, val) => acc * val);

        var monkeyBusiness = GetMonkeyBusiness(
            items, operations, arguments, modules, trueReceivers, falseReceivers, 10_000,
            x => x % moduleProduct);

        Console.WriteLine(monkeyBusiness);
    }

    private static double GetMonkeyBusiness(IList<List<long>> items, IList<char> operations, IList<int> arguments,
        IList<int> modules, IList<int> trueReceivers, IList<int> falseReceivers, int rounds, Func<long, long> func)
    {
        var inspections = items.Select(x => 0d).ToArray();

        for (var round = 0; round < rounds; round++)
        {
            for (var monkey = 0; monkey < items.Count; monkey++)
            {
                for (var i = 0; i < items[monkey].Count; i++)
                {
                    inspections[monkey]++;

                    items[monkey][i] = operations[monkey] switch
                    {
                        '*' => items[monkey][i] * arguments[monkey],
                        '^' => items[monkey][i] * items[monkey][i],
                        '+' => items[monkey][i] + arguments[monkey]
                    };

                    items[monkey][i] = func(items[monkey][i]);

                    var receiving = items[monkey][i] % modules[monkey] == 0
                        ? trueReceivers[monkey]
                        : falseReceivers[monkey];

                    items[receiving].Add(items[monkey][i]);
                    items[monkey].RemoveAt(i);
                    i--;
                }
            }
        }

        var monkeyBusiness = inspections
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate(1d, (acc, val) => acc * val);

        return monkeyBusiness;
    }

    private static async Task<(List<List<long>>, IList<char>, IList<int>, IList<int>, IList<int>, IList<int>)> ParseAsync(string path)
    {
        var text = await File.ReadAllTextAsync(path);
        var monkeys = text.Split("\r\n\r\n");

        var items = new List<List<long>>();

        var operations = new List<char>();
        var arguments = new List<int>();

        var modules = new List<int>();

        var trueReceivers = new List<int>();
        var falseReceivers = new List<int>();

        foreach (var monkey in monkeys)
        {
            var lines = monkey.Split("\r\n");

            var monkeyItems = lines[1]
                .Split(":")[^1]
                .Split(",")
                .Select(x => long.Parse(x.Trim()))
                .ToList();

            items.Add(monkeyItems);

            var operationLine = lines[2];
            if (operationLine.Contains("* old"))
            {
                operations.Add('^');
                arguments.Add(0);
            }
            else if (operationLine.Contains('*'))
            {
                operations.Add('*');
                arguments.Add(int.Parse(operationLine.Split('*')[^1].Trim()));
            }
            else
            {
                operations.Add('+');
                arguments.Add(int.Parse(operationLine.Split('+')[^1].Trim()));
            }

            modules.Add(int.Parse(lines[3].Split("by")[^1].Trim()));
            
            trueReceivers.Add(int.Parse(lines[4].Split("monkey")[^1].Trim()));
            
            falseReceivers.Add(int.Parse(lines[5].Split("monkey")[^1].Trim()));
        }

        return (items, operations, arguments, modules, trueReceivers, falseReceivers);
    }
}