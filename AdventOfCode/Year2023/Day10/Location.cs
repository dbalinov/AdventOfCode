namespace AdventOfCode.Year2023.Day10;

public readonly struct Location(int row, int col): IEquatable<Location>
{
    public int Row { get; } = row;
    public int Col { get; } = col;

    public bool Equals(Location other)
        => Row == other.Row && Col == other.Col;
    
    public override bool Equals(object? obj)
        => obj is Location other && Equals(other);
    
    public override int GetHashCode()
        => HashCode.Combine(Row, Col);

    public static bool operator ==(Location left, Location right)
        => left.Equals(right);

    public static bool operator !=(Location left, Location right)
        => !(left == right);
}