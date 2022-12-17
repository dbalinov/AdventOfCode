namespace AdventOfCode.Year2022.Day16;

/// <summary>
/// Proboscidea Volcanium
/// </summary>
internal sealed class Day16 : IDay
{
    private const string FileName = "Year2022/Day16/input.txt";
    private const string StartValve = "AA";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var valves = await GetValves(lines);

        var cache = new Dictionary<(int, string, int), int>();

        var maxFlowRate = GetFlowRate(valves, 30, 0, cache);
        
        Console.WriteLine(maxFlowRate);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var valves = await GetValves(lines);
        
        var nonEmpty = GetNonEmpty(valves);

        var cache = new Dictionary<(int, string, int), int>();

        var bitmask = (1 << nonEmpty.Count) - 1;

        var maxFlowRate = 0;

        foreach (var i in Enumerable.Range(0, (bitmask + 1) / 2))
        {
            maxFlowRate = Math.Max(maxFlowRate, GetFlowRate(valves, 26, i, cache) + GetFlowRate(valves, 26, bitmask ^ i, cache));
        }

        Console.WriteLine(maxFlowRate);
    }

    private static int GetFlowRate(Dictionary<string, Node> valves, int time, int bitmask, IDictionary<(int, string, int), int> cache)
    {
        var nonEmpty = GetNonEmpty(valves);
        var distances = GetDistances(valves);

        int Dfs(int time, string valve, int bitmask)
        {
            if (cache.ContainsKey((time, valve, bitmask)))
            {
                return cache[(time, valve, bitmask)];
            }

            var maxValue = 0;
            foreach (var neighbor in distances[valve].Keys)
            {
                if (!nonEmpty.Contains(neighbor))
                {
                    continue;
                }

                var index = nonEmpty.IndexOf(neighbor);
                var bit = 1 << index;
                if ((bitmask & bit) != 0)
                {
                    continue;
                }

                var remainingTime = time - distances[valve][neighbor] - 1;
                if (remainingTime <= 0)
                {
                    continue;
                }

                maxValue = Math.Max(maxValue,
                    Dfs(remainingTime, neighbor, bitmask | bit) + valves[neighbor].FlowRate * remainingTime);
            }

            cache[(time, valve, bitmask)] = maxValue;

            return maxValue;
        }

        var value = Dfs(time, StartValve, bitmask);

        return value;
    }

    private static IList<string> GetNonEmpty(Dictionary<string, Node> valves)
    {
        var nonEmpty = new List<string>();
        foreach (var (name, valve) in valves)
        {
            if (name != StartValve && valve.FlowRate == 0)
            {
                continue;
            }

            if (name != StartValve)
            {
                nonEmpty.Add(name);
            }
        }

        return nonEmpty;
    }

    private static IDictionary<string, Dictionary<string, int>> GetDistances(Dictionary<string, Node> valves)
    {
        var distances = new Dictionary<string, Dictionary<string, int>>();

        foreach (var (name, valve) in valves)
        {
            if (name != StartValve && valve.FlowRate == 0)
            {
                continue;
            }
            
            distances[name] = new Dictionary<string, int>
            {
                {StartValve, 0}
            };

            if (!distances[name].ContainsKey(name))
            {
                distances[name].Add(name, 0);
            }

            var visited = new List<string> { name };

            var queue = new Queue<(int distance, string valve)>();

            queue.Enqueue((0, name));

            while (queue.Any())
            {
                var (distance, position) = queue.Dequeue();

                foreach (var neighbor in valves[position].TunnelsTo)
                {
                    if (visited.Contains(neighbor))
                    {
                        continue;
                    }

                    visited.Add(neighbor);

                    if (valves.ContainsKey(neighbor))
                    {
                        distances[name][neighbor] = distance + 1;
                    }

                    queue.Enqueue((distance + 1, neighbor));
                }
            }

            distances[name].Remove(name);

            if (name != StartValve)
            {
                distances[name].Remove(StartValve);
            }
        }

        return distances;
    }

    private static async Task<Dictionary<string, Node>> GetValves(IAsyncEnumerable<string> lines)
    {
        var valves = new Dictionary<string, Node>();

        await foreach (var line in lines)
        {
            var node = Parse(line);

            valves.Add(node.Name, node);
        }

        return valves;
    }

    private static Node Parse(ReadOnlySpan<char> line)
    {
        var name = line.Slice(6, 2).ToString();

        var indexOfSemicolon = line.IndexOf(';');
        var rate = int.Parse(line.Slice(23, indexOfSemicolon - 23));

        var tunnelsTo = new List<string>();
        
        var i = indexOfSemicolon + (line[indexOfSemicolon + 23] == 's' ? 25 : 24);
        for (; i < line.Length;)
        {
            tunnelsTo.Add(line.Slice(i, 2).ToString());
            i += 4;
        }

        return new Node(name, rate, tunnelsTo);
    }
}