namespace AdventOfCode.Year2022.Day12;

public struct Location : IEquatable<Location>
{
    public Location(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public int Row { get; set; }
    public int Col { get; set; }

    public bool Equals(Location other)
        => Row == other.Row && Col == other.Col;

    public override bool Equals(object? obj)
        => obj is Location other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Row, Col);
}