namespace AdventOfCode.Year2019.Day8;

internal sealed class Day8 : IDay
{
    private const string FileName = "Year2019/Day8/input.txt";

    public string Name => "Space Image Format";

    public async Task SolvePart1()
    {
        var text = await File.ReadAllTextAsync(FileName);

        var image = text.Trim();

        const int width = 25;
        const int height = 6;

        var layers = GetLayers(image, width * height);

        var layerWithFewestZeros = layers.MinBy(x => x.Count(y => y == '0'));

        var countOfOnes = layerWithFewestZeros!.Count(x => x == '1');
        var countOfTwos = layerWithFewestZeros!.Count(x => x == '2');

        var result = countOfOnes * countOfTwos;

        Console.WriteLine(result);
    }

    public async Task SolvePart2()
    {
        var text = await File.ReadAllTextAsync(FileName);
        
        var image = text.Trim();

        const int width = 25;
        const int height = 6;

        const int layerSize = width * height;

        var layers = GetLayers(image, layerSize);
        
        // 0 is black, 1 is white, 2 is transparent
        var mergedLayer = MergeLayers(layers, layerSize);

        for (var i = 0; i < layerSize; i++)
        {
            Console.Write(mergedLayer[i] == '1' ? '#' : ' ');

            if ((i + 1) % width == 0)
            {
                Console.WriteLine();
            }
        }
    }

    private static IList<char> MergeLayers(IList<IList<char>> layers, int layerSize)
    {
        var mergedLayer = new List<char>();
        for (var i = 0; i < layerSize; i++)
        {
            foreach (var layer in layers)
            {
                if (layer[i] != '2')
                {
                    mergedLayer.Add(layer[i]);
                    break;
                }
            }
        }

        return mergedLayer;
    }

    private static IList<IList<char>> GetLayers(IEnumerable<char> image, int layerSize)
    {
        var layers = new List<IList<char>>();

        while (image.Any())
        {
            var layer = image.Take(layerSize).ToList();

            layers.Add(layer);

            image = image.Skip(layerSize).ToList();
        }

        return layers;
    }
}