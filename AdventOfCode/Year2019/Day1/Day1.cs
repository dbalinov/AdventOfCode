namespace AdventOfCode.Year2019.Day1;

internal sealed class Day1 : IDay
{
    private const string FileName = "Year2019/Day1/input.txt";

    public string Name => "The Tyranny of the Rocket Equation";

    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var sum = 0;
        await foreach (var line in lines)
        {
            var mass = int.Parse(line);

            var fuel = CalculateFuel(mass);

            sum += fuel;
        }

        Console.WriteLine(sum);
    }

    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var sum = 0;
        await foreach (var line in lines)
        {
            var mass = int.Parse(line);
            var fuel = mass;

            while ((fuel = CalculateFuel(fuel)) > 0)
            {
                sum += fuel;
            }
        }

        Console.WriteLine(sum);
    }

    private static int CalculateFuel(int mass)
        => mass / 3 - 2;
}