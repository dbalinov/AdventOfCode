namespace AdventOfCode.Year2019.Day13;

public class Tile
{
    public Tile(int x, int y, TileId tileId)
    {
        X = x;
        Y = y;
        TileId = tileId;
    }

    /// <summary>
    /// distance from the left
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// distance from the top
    /// </summary>
    public int Y { get; set; }

    public TileId TileId { get; set; }
}