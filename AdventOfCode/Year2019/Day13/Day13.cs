using AdventOfCode.Year2019.Day9;
using System.Collections.Concurrent;

namespace AdventOfCode.Year2019.Day13;

internal class Day13 : IDay
{
    private const string FileName = "Year2019/Day13/input.txt";

    public string Name => "Care Package";

    public async Task SolvePart1()
    {
        var code = await File.ReadAllTextAsync(FileName);

        var computer = IntcodeComputer9.FromCode(code);

        var inputs = new BlockingCollection<long>();
        var outputs = new BlockingCollection<long>();
        var computerTask = computer.RunAsync(inputs, outputs);

        var tilesTask = Task.Run(() => GetTiles(outputs));

        await Task.WhenAll(computerTask, tilesTask);

        var tiles = tilesTask.Result;

        var countOfBlockTiles = tiles.Count(x => x.TileId == TileId.Block);

        Console.WriteLine(countOfBlockTiles);
    }

    public async Task SolvePart2()
    {
        var code = await File.ReadAllTextAsync(FileName);

        var computer = IntcodeComputer9.FromCode(code);

        var inputs = new BlockingCollection<long>();
        var outputs = new BlockingCollection<long>();
        var computerTask = computer.RunAsync(inputs, outputs);

        var tilesTask = Task.Run(() => GetTiles(outputs));

        await Task.WhenAll(computerTask, tilesTask);

        var tiles = tilesTask.Result;

        await PlayAsync(code, tiles);
    }

    private static Task PlayAsync(string code, IList<Tile> tiles)
    {
        var instructions = IntcodeComputer9.Parse(code);

        // insert quarters
        instructions[0] = 2;

        var computer = new IntcodeComputer9(instructions);

        var inputs = new BlockingCollection<long>();
        var outputs = new BlockingCollection<long>();
        var computerTask = computer.RunAsync(inputs, outputs);

        var moveTask = Task.Run(() => Play(tiles, inputs, outputs));

        return Task.WhenAll(computerTask, moveTask);
    }

    private static void Play(IList<Tile> tiles, BlockingCollection<long> inputs, BlockingCollection<long> outputs)
    {
        var ball = tiles.First(x => x.TileId == TileId.Ball);
        var paddle = tiles.First(x => x.TileId == TileId.HorizontalPaddle);
        var score = 0;
        PrintTiles(tiles);
        PrintScore(score);

        while (!outputs.IsCompleted)
        {
            try
            {
                var x = (int)outputs.Take();
                var y = (int)outputs.Take();
                var value = (int)outputs.Take();

                if (x == -1 && y == 0)
                {
                    score = value;
                    PrintScore(score);
                    continue;
                }
                
                var tileId = (TileId) value;

                var tile = tiles.FirstOrDefault(t => t.X == x && t.Y == y);
                if (tile != null && tile.TileId != TileId.Ball && tile.TileId != TileId.HorizontalPaddle)
                {
                    tile.TileId = tileId;
                    DrawTile(tile.X, tile.Y, tile.TileId);
                }

                if (tileId == TileId.HorizontalPaddle)
                {
                    DrawTile(paddle.X, paddle.Y, TileId.Empty);
                    paddle.X = x;
                    paddle.Y = y;
                    DrawTile(paddle.X, paddle.Y, paddle.TileId);
                }
                else if (tileId == TileId.Ball)
                {
                    DrawTile(ball.X, ball.Y, TileId.Empty);
                    ball.X = x;
                    ball.Y = y;
                    DrawTile(ball.X, ball.Y, ball.TileId);

                    var compare = ball.X.CompareTo(paddle.X);
                    inputs.Add(compare);
                }
            }
            catch (InvalidOperationException)
            {
                break;
            }
        }
    }

    private static void PrintTiles(IEnumerable<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            DrawTile(tile.X, tile.Y, tile.TileId);
        }
    }

    private static void PrintScore(int score)
    {
        Console.SetCursorPosition(0, 22);
        Console.WriteLine("Score: {0}", score);
    }

    private static void DrawTile(int x, int y, TileId tile)
    {
        Console.SetCursorPosition(x, y);

        var c = tile switch
        {
            TileId.Empty => ' ',
            TileId.Wall => '█',
            TileId.Block => '#',
            TileId.HorizontalPaddle => '_',
            TileId.Ball => 'O',
            _ => throw new ArgumentOutOfRangeException(nameof(tile))
        };

        Console.Write(c);

        Console.SetCursorPosition(x, y);
    }

    private static IList<Tile> GetTiles(BlockingCollection<long> outputs)
    {
        var tiles = new List<Tile>();

        while (!outputs.IsCompleted)
        {
            try
            {
                var x = (int)outputs.Take();
                var y = (int)outputs.Take();
                var tileId = (TileId)outputs.Take();

                var tile = new Tile(x, y, tileId);

                tiles.Add(tile);
            }
            catch (InvalidOperationException)
            {
                break;
            }
        }

        return tiles;
    }
}