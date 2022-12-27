namespace AdventOfCode.Year2022.Day24;

public struct Location : IEquatable<Location>
{
    public Location(int row, int col, int g)
    {
        Row = row;
        Col = col;
        G = g;
    }

    public int Row { get; set; }
    public int Col { get; set; }
    public int G { get; set; }

    public bool Equals(Location other)
        => Row == other.Row && Col == other.Col && G == other.G;

    public override bool Equals(object? obj)
        => obj is Location other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Row, Col, G);
}