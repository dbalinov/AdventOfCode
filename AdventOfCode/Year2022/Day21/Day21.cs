using System.Diagnostics;

namespace AdventOfCode.Year2022.Day21;

internal sealed class Day21 : IDay
{
    private const string FileName = "Year2022/Day21/input.txt";

    private const string RootNode = "root";
    private const string HumanNode = "humn";

    public string Name => "Monkey Math";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var nodes = new Dictionary<string, Node>();
        await foreach (var line in lines)
        {
            var node = ParseNode(line);

            nodes.Add(node.Name, node);
        }

        var value = GetValue(nodes, RootNode);

        Console.WriteLine(value);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var nodes = new Dictionary<string, Node>();
        await foreach (var line in lines)
        {
            var node = ParseNode(line);

            nodes.Add(node.Name, node);
        }

        var root = nodes["root"];

        var leftNodeContainsHuman = ContainsNode(nodes, root.LeftNodeName, HumanNode);

        var humanTree = leftNodeContainsHuman ? root.LeftNodeName : root.RightNodeName;
        var nonHumanTree = !leftNodeContainsHuman ? root.LeftNodeName : root.RightNodeName;

        var targetValue = GetValue(nodes, nonHumanTree);
        var humanValue = GetHumanValue(nodes, humanTree, targetValue);

        Debug.Assert(humanValue == 3272260914328);
        Console.WriteLine(humanValue);
    }

    private static long? GetHumanValue(IDictionary<string, Node> nodes, string? name, long targetValue)
    {
        if (name == HumanNode)
        {
            return targetValue;
        }

        var node = nodes[name];
        if (node.Operation == null) {
            return node.Value;
        }

        var leftNodeContainsHuman = ContainsNode(nodes, node.LeftNodeName, HumanNode);

        if (leftNodeContainsHuman)
        {
            var rightValue = GetValue(nodes, node.RightNodeName);

            targetValue = node.Operation switch
            {
                '+' => targetValue - rightValue,
                '-' => targetValue + rightValue,
                '*' => targetValue / rightValue,
                '/' => targetValue * rightValue,
                _ => throw new NotSupportedException("operation not supported")
            };

            return GetHumanValue(nodes, node.LeftNodeName, targetValue);
        }

        var leftValue = GetValue(nodes, node.LeftNodeName);

        targetValue = node.Operation switch
        {
            '+' => targetValue - leftValue,
            '-' => leftValue - targetValue,
            '*' => targetValue / leftValue,
            '/' => targetValue * leftValue,
            _ => throw new NotSupportedException("operation not supported")
        };

        return GetHumanValue(nodes, node.RightNodeName, targetValue);
    }

    private static bool ContainsNode(IDictionary<string, Node> nodes, string? root, string name)
    {
        if (root == null)
        {
            return false;
        }

        if (root == name)
        {
            return true;
        }

        var node = nodes[root];
        if (node.LeftNodeName == name || node.RightNodeName == name)
        {
            return true;
        }

        return ContainsNode(nodes, node.LeftNodeName, name) || ContainsNode(nodes, node.RightNodeName, name);
    }
    
    private static long GetValue(IDictionary<string, Node> nodes, string? name)
    {
        var node = nodes[name];

        if (node.Value.HasValue)
        {
            return node.Value.Value;
        }

        var leftValue = GetValue(nodes, node.LeftNodeName);
        var rightValue = GetValue(nodes, node.RightNodeName);

        checked
        {
            var value = node.Operation switch
            {
                '+' => leftValue + rightValue,
                '-' => leftValue - rightValue,
                '*' => leftValue * rightValue,
                '/' => leftValue / rightValue,
                _ => throw new NotSupportedException("operation not supported")
            };

            node.Value = value;

            return value;
        }
    }

    private static Node ParseNode(string line)
    {
        var parts = line.Split(':');

        var name = parts[0];
        var operation = parts[1].Trim();

        var node = new Node(name);

        if (long.TryParse(operation, out var value))
        {
            node.Value = value;
        }
        else
        {
            node.LeftNodeName = operation[..4];
            node.Operation = operation[5];
            node.RightNodeName = operation[7..];
        }

        return node;
    }
}