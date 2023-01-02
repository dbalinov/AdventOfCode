namespace AdventOfCode.Year2022.Day20;

internal sealed class Day20 : IDay
{
    private const string FileName = "Year2022/Day20/input.txt";

    public string Name => "Grove Positioning System";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var list = await GetList(lines);
        
        foreach (var current in list)
        {
            MoveNode(current, list.Count);
        }

        var zeroElement = list.First(x => x.Value == 0);

        var indexes = new List<int> {1000, 2000, 3000};

        var sum = indexes.Select(index => ElementAt(zeroElement, index, list.Count)?.Value).Sum();

        Console.WriteLine(sum);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var list = await GetList(lines);

        const int decryptionKey = 811589153;
        foreach (var node in list)
        {
            node.Value *= decryptionKey;
        }

        for (var i = 0; i < 10; i++)
        {
            foreach (var current in list)
            {
                MoveNode(current, list.Count);
            }
        }

        var zeroElement = list.First(x => x.Value == 0);

        var indexes = new List<int> { 1000, 2000, 3000 };

        var sum = indexes.Select(index => ElementAt(zeroElement, index, list.Count)?.Value).Sum();

        Console.WriteLine(sum);
    }

    private static void MoveNode(Node current, int nodeCount)
    {
        if (current.Value == 0)
        {
            return;
        }

        current.Next.Prev = current.Prev;
        current.Prev.Next = current.Next;

        var moveCount = Mod(current.Value, nodeCount - 1);

        var prev = current;
        for (var i = 0; i < moveCount; i++)
        {
            prev = prev.Next;
        }

        var  next = prev.Next;

        current.Prev = prev;
        current.Next = next;
        prev.Next = current;
        next.Prev = current;
    }

    private static Node? ElementAt(Node node, int index, int nodeCount)
    {
        index %= nodeCount;

        var current = node;
        for (var i = 0; i < index; i++)
        {
            current = current?.Next;
        }

        return current;
    }

    private static long Mod(long a, long n)
    {
        if (n == 0)
            throw new ArgumentOutOfRangeException("n", "(a mod 0) is undefined.");

        //puts a in the [-n+1, n-1] range using the remainder operator
        var remainder = a % n;

        //if the remainder is less than zero, add n to put it in the [0, n-1] range if n is positive
        //if the remainder is greater than zero, add n to put it in the [n-1, 0] range if n is negative
        if ((n > 0 && remainder < 0) ||
            (n < 0 && remainder > 0))
            return remainder + n;
        return remainder;
    }

    private static async Task<IList<Node>> GetList(IAsyncEnumerable<string> lines)
    {
        var list = new List<Node>();

        Node? head = null;
        Node? current = null;

        await foreach (var line in lines)
        {
            var number = int.Parse(line);
            if (current == null)
            {
                current = new Node { Value = number };
                head = current;

            }
            else
            {
                var prev = current;

                current = new Node
                {
                    Value = number,
                    Prev = prev
                };

                prev.Next = current;
            }

            list.Add(current);
        }

        if (current != null)
        {
            current.Next = head;
            head!.Prev = current;
        }

        return list;
    }
}