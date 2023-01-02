namespace AdventOfCode.Year2022.Day7;

/// <summary>
/// No Space Left On Device
/// </summary>
public sealed class Day7 : IDay
{
    private const string FileName = "Year2022/Day7/input.txt";

    public string Name => "No Space Left On Device";

    public async Task SolvePart1()
    {
        var root = await ParseTreeAsync(FileName);

        var sum = GetTreeSizes(root)
            .Where(size => size <= 100_000)
            .Sum();

        Console.WriteLine(sum);
    }

    public async Task SolvePart2()
    {
        var root = await ParseTreeAsync(FileName);

        var availableSpace = 70000000 - root.Size;
        var spaceToCleanUp = 30000000 - availableSpace;

        var min = GetTreeSizes(root)
            .Where(size => size >= spaceToCleanUp)
            .Min();

        Console.WriteLine(min);
    }

    private static IEnumerable<int> GetTreeSizes(TreeNode root)
    {
        foreach (var child in root.Directories.Values)
        {
            yield return child.Size;

            foreach (var size in GetTreeSizes(child))
            {
                yield return size;
            }
        }
    }

    private static async Task<TreeNode> ParseTreeAsync(string path)
    {
        var lines = File.ReadLinesAsync(path);

        var root = new TreeNode();
        var currentNode = root;

        await foreach (var line in lines)
        {
            var lineParts = line.Split(' ');
            if (lineParts[0] == "$")
            {
                if (lineParts[1] == "cd")
                {
                    if (lineParts[2] == "/")
                    {
                        currentNode = root;
                    }
                    else if (lineParts[2] == ".." && currentNode.Parent != null)
                    {
                        currentNode = currentNode.Parent;
                    }
                    else
                    {
                        var name = lineParts[2];
                        if (!currentNode.Directories.ContainsKey(name))
                        {
                            var directory = new TreeNode { Parent = currentNode };

                            currentNode.AddDirectory(name, directory);
                        }
                 
                        currentNode = currentNode.Directories[name];
                    }
                }
            }
            else if (lineParts[0] != "dir")
            {
                currentNode.FilesSize += int.Parse(lineParts[0]);
            }
        }

        return root;
    }
}