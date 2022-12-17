namespace AdventOfCode.Year2022.Day16;

public class Node
{
    public Node(string name, int flowRate, IReadOnlyCollection<string> tunnelsTo)
    {
        Name = name;
        FlowRate = flowRate;
        TunnelsTo = tunnelsTo;
    }

    public string Name { get; }
    public int FlowRate { get; }
    public IReadOnlyCollection<string> TunnelsTo { get; }
}