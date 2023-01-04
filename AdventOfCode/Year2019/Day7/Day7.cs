using System.Collections.Concurrent;

namespace AdventOfCode.Year2019.Day7;

public sealed class Day7 : IDay
{
    private const string FileName = "Year2019/Day7/input.txt";

    public string Name => "Amplification Circuit";

    public async Task SolvePart1()
    {
        var code = await File.ReadAllTextAsync(FileName);

        var phasesList = new[] { 0, 1, 2, 3, 4 }.GetPermutations(); ;

        var maxOutput = 0;

        foreach (var phases in phasesList)
        {
            var input = 0;
            var output = 0;
            for (var i = 0; i < 5; i++)
            {
                var inputs = new BlockingCollection<int> { phases[i], input };
                var outputs = new BlockingCollection<int>();

                var amplifier = IntcodeComputer.FromCode(code);
                
                await amplifier.RunAsync(inputs, outputs);

                output = outputs.Take();

                input = output;
            }

            if (output > maxOutput)
            {
                maxOutput = output;
            }
        }

        Console.WriteLine(maxOutput);
    }
    
    public async Task SolvePart2()
    {
        var code = await File.ReadAllTextAsync(FileName);

        var phasesList =  new[] { 5, 6, 7, 8, 9 }.GetPermutations();

        var maxOutput = 0;
        
        foreach (var phases in phasesList)
        {
            var amplifiers = new List<IntcodeComputer>();
            var inputs = new List<BlockingCollection<int>>();
            var tasks = new List<Task>();

            for (var i = 0; i < 5; i++)
            {
                inputs.Add(new BlockingCollection<int> { phases[i] });
                if (i == 0)
                {
                    inputs[i].Add(0);
                }

                amplifiers.Add(IntcodeComputer.FromCode(code));
            }

            for (var i = 0; i < 5; i++)
            {
                tasks.Add(amplifiers[i].RunAsync(inputs[i], inputs[(i + 1) % 5]));
            }
            
            await Task.WhenAll(tasks);

            var output = inputs[0].Take();
            if (output > maxOutput)
            {
                maxOutput = output;
            }
        }

        Console.WriteLine(maxOutput);
    }
}