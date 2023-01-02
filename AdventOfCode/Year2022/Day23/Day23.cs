namespace AdventOfCode.Year2022.Day23;

internal sealed class Day23 : IDay
{
    private const string FileName = "Year2022/Day23/input.txt";

    private static readonly IReadOnlyList<Elf> North = new Elf[]
    {
        new(-1, -1),
        new(-1, 0),
        new(-1, 1)
    };

    private static readonly IReadOnlyList<Elf> South = new Elf[]
    {
        new(1, -1),
        new(1, 0),
        new(1, 1)
    };

    private static readonly IReadOnlyList<Elf> West = new Elf[]
    {
        new(-1, -1),
        new(0, -1),
        new(1, -1)
    };

    private static readonly IReadOnlyList<Elf> East = new Elf[]
    {
        new(-1, 1),
        new(0, 1),
        new(1, 1)
    };

    public string Name => "Unstable Diffusion";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var elves = await ParseElvesAsync(lines);

        var strategies = new Queue<IReadOnlyList<Elf>>();
        strategies.Enqueue(North);
        strategies.Enqueue(South);
        strategies.Enqueue(West);
        strategies.Enqueue(East);

        for (var round = 0; round < 10; round++)
        {
            elves = GetNextPositions(elves, strategies);

            var first = strategies.Dequeue();
            strategies.Enqueue(first);
        }

        var count = GetEmptyTilesCount(elves);

        Console.WriteLine(count);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var elves = await ParseElvesAsync(lines);

        var strategies = new Queue<IReadOnlyList<Elf>>();
        strategies.Enqueue(North);
        strategies.Enqueue(South);
        strategies.Enqueue(West);
        strategies.Enqueue(East);
        
        var round = 0;
        while (true)
        {
            var oldElves = elves;
            elves = GetNextPositions(elves, strategies);
            
            var first = strategies.Dequeue();
            strategies.Enqueue(first);

            round++;
            if (elves.SetEquals(oldElves))
            {
                break;
            }
        }

        Console.WriteLine(round);
    }

    private static int GetEmptyTilesCount(ICollection<Elf> elves)
    {
        var minRow = elves.Min(x => x.Row);
        var minCol = elves.Min(x => x.Col);
        var maxRow = elves.Max(x => x.Row);
        var maxCol = elves.Max(x => x.Col);

        var count = 0;
        for (var row = minRow; row <= maxRow; row++)
        {
            for (var col = minCol; col <= maxCol; col++)
            {
                if (!elves.Contains(new Elf(row, col)))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static ISet<Elf> GetNextPositions(ICollection<Elf> oldPositions, Queue<IReadOnlyList<Elf>> strategies)
    {
        var newPositions = new HashSet<Elf>();

        var proposals = new Dictionary<Elf, IList<Elf>>();

        foreach (var elf in oldPositions)
        {
            var canMove = new List<bool>();
            foreach (var strategy in strategies)
            {
                var hasNeighbors = strategy
                    .Select(x => elf + x)
                    .Any(oldPositions.Contains);

                canMove.Add(!hasNeighbors);
            }

            if (canMove.All(x => x) || canMove.All(x => !x))
            {
                newPositions.Add(elf);

                if (!proposals.ContainsKey(elf))
                {
                    proposals[elf] = new List<Elf>();
                }

                proposals[elf].Add(elf);

                continue;
            }

            var i = 0;
            foreach (var strategy in strategies)
            {
                if (canMove[i])
                {
                    var direction = strategy[1];
                    var newPosition = elf + direction;

                    if (!proposals.ContainsKey(newPosition))
                    {
                        proposals[newPosition] = new List<Elf>();
                    }

                    proposals[newPosition].Add(elf);
                    break;
                }

                i++;
            }
        }

        foreach (var (newElf, oldElves) in proposals)
        {
            if (oldElves.Count > 1)
            {
                foreach (var oldElf in oldElves)
                {
                    newPositions.Add(oldElf);
                }
            }
            else
            {
                newPositions.Add(newElf);
            }
        }

        return newPositions;
    }

    private static async Task<ISet<Elf>> ParseElvesAsync(IAsyncEnumerable<string> lines)
    {
        var elves = new HashSet<Elf>();

        var row = 0;
        await foreach (var line in lines)
        {
            for (var col = 0; col < line.Length; col++)
            {
                if (line[col] == '#')
                {
                    elves.Add(new Elf(row, col));
                }
            }

            row++;
        }

        return elves;
    }
}