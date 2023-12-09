namespace AdventOfCode.Year2023.Day9;

/// <summary>
/// Mirage Maintenance
/// </summary>
internal class Day9 : IDay
{
    private const string FileName = "Year2023/Day9/input.txt";
    
    public string Name => "Mirage Maintenance";

    public Task SolvePart1() => Solve(forward: true);

    public Task SolvePart2() => Solve(forward: false);

    private static async Task Solve(bool forward)
    {
        var lines = File.ReadLinesAsync(FileName);
        
        var result = 0;
        await foreach (var line in lines)
        {
            var seq = ParseLine(line);

            if (!forward) {
                seq.Reverse();
            }

            ExtrapolateSequence(seq);
            
            result += seq[^1];
        }

        Console.WriteLine(result);
    }
    
    private static void ExtrapolateSequence(List<int> seq)
    {
        if (seq.All(x => x == 0))
        {
            return;
        }
        
        var nextSeq = GetNextSequence(seq);

        ExtrapolateSequence(nextSeq);
      
        seq.Add(seq[^1] + nextSeq[^1]);
        
    }
    
    private static List<int> GetNextSequence(IList<int> seq)
    {
        var nextSeq = new List<int>();

        for(var i = 0; i < seq.Count - 1; i++)
        {
            var number = seq[i];
            var nextNumber = seq[i + 1];
            
            nextSeq.Add(nextNumber - number);
        }

        return nextSeq;
    }
    
    private static List<int> ParseLine(ReadOnlySpan<char> line)
    {
        var numbers = new List<int>();

        var multiplier = 1;
        var number = 0;
        foreach (var ch in line)
        {
            if (ch == '-')
            {
                multiplier *= -1;
            }
            else if (char.IsNumber(ch))
            {
                number *= 10;
                number += ch - '0';
            }
            else
            {
                numbers.Add(multiplier * number);
                multiplier = 1;
                number = 0;
            }
        }

        numbers.Add(multiplier * number);

        return numbers;
    }
}