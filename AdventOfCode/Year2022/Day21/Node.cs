namespace AdventOfCode.Year2022.Day21;

public sealed class Node
{
    public Node(string name)
        => Name = name;

    public string Name { get; }
    public long? Value { get; set; }
    public char? Operation { get; set; }
    public string? LeftNodeName { get; set; }
    public string? RightNodeName { get; set; }
}