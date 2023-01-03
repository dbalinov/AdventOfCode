namespace AdventOfCode.Year2019.Day6;

internal sealed class Day6 : IDay
{
    private const string FileName = "Year2019/Day6/input.txt";
    
    public string Name => "Universal Orbit Map";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);
        
        var tree = await ParseTreeAsync(lines);

        var sum = tree.Keys.Sum(node => GetPath(tree, node).Count);

        Console.WriteLine(sum);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var tree = await ParseTreeAsync(lines);

        var pathYou = GetPath(tree, "YOU");
        var pathSan = GetPath(tree, "SAN");

        var index = 0;
        while (pathYou[index] == pathSan[index])
        {
            index++;
        }

        var pathLengthYouToSan = pathYou.Count - index - 1 + pathSan.Count - index - 1;

        Console.WriteLine(pathLengthYouToSan);
    }
    
    private static IList<string> GetPath(IDictionary<string, List<string>> tree, string pathTo)
    {
        const string current = "COM";

        var path = new List<string>();

        FillPath(tree, current, pathTo, path);

        path.Reverse();

        return path;
    }

    private static bool FillPath(IDictionary<string, List<string>> tree, string current, string search, IList<string> path)
    {
        foreach (var branch in tree[current])
        {
            if (branch == search || FillPath(tree, branch, search, path))
            {
                path.Add(branch);
                return true;
            }
        }

        return false;
    }

    private static async Task<Dictionary<string, List<string>>> ParseTreeAsync(IAsyncEnumerable<string> lines)
    {
        var tree = new Dictionary<string, List<string>>();

        await foreach (var line in lines)
        {
            var index = line.IndexOf(')');
            var left = line[..index];
            var right = line[(index + 1)..];

            if (tree.ContainsKey(left))
            {
                tree[left].Add(right);
            }
            else
            {
                tree.Add(left, new List<string> { right });
            }

            if (!tree.ContainsKey(right))
            {
                tree.Add(right, new List<string>());
            }
        }

        return tree;
    }
}