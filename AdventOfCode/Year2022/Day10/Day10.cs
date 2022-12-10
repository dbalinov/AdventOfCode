namespace AdventOfCode.Year2022.Day10;

internal sealed class Day10 : IDay
{
    private const string FileName = "Year2022/Day10/input.txt";

    public async Task SolvePart1()
    {
        using var streamReader = new StreamReader(FileName);
        var line = await streamReader.ReadLineAsync();

        const int spaceIndex = 4;
        var cycle = 0;
        var x = 1;
        var strength = 0;
        var m = 20;
        var i = 0;
        
        while (true)
        {
            if (line == null)
            {
                break;
            }

            if (line[0] == 'a') // addx
            {
                if (i == 2)
                {
                    i = 0;
                    x += int.Parse(line.AsSpan(spaceIndex));

                    line = await streamReader.ReadLineAsync();
                }
            }
            else // noop
            {
                if (i == 1)
                {
                    i = 0;

                    line = await streamReader.ReadLineAsync();
                }
            }

            i++;
            cycle++;

            if (cycle == m)
            {
                strength += x * m;
                m += 40;
            }
        }
        
        Console.WriteLine(strength);
    }

    public async Task SolvePart2()
    {
        const int width = 40;
        const int height = 6;
        var pixels = Enumerable.Repeat('.', width * height).ToArray();

        using var streamReader = new StreamReader(FileName);
        var line = await streamReader.ReadLineAsync();

        const int spaceIndex = 4;
        var cycle = 0;
        var x = 1;
        var i = 0;

        while (true)
        {
            if (line == null)
            {
                break;
            }

            if (line[0] == 'a') // addx
            {
                if (i == 2)
                {
                    i = 0;
                    x += int.Parse(line.AsSpan(spaceIndex));
                    
                    line = await streamReader.ReadLineAsync();
                }
            }
            else // noop
            {
                if (i == 1)
                {
                    i = 0;
                    
                    line = await streamReader.ReadLineAsync();
                }
            }

            var pixel = cycle % 400;
            var col = pixel % 40;
            if (col >= x - 1 && col <= x + 1)
            {
                pixels[pixel] = '#';
            }

            i++;
            cycle++;
        }

        DrawScreen(pixels, width);
    }

    private static void DrawScreen(IReadOnlyList<char> pixels, int width)
    {
        for (var j = 0; j < pixels.Count; j++)
        {
            Console.Write(pixels[j]);

            if (j % width == width - 1)
            {
                Console.WriteLine();
            }
        }
    }
}