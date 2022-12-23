namespace AdventOfCode.Year2022.Day23;

public struct Elf : IEquatable<Elf>
{
    public Elf(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public int Row { get; set; }
    public int Col { get; set; }

    public static Elf operator +(Elf a, Elf b)
        => new(a.Row + b.Row, a.Col + b.Col);

    public static Elf operator -(Elf a, Elf b)
        => new(a.Row - b.Row, a.Col - b.Col);

    public bool Equals(Elf other)
        => Row == other.Row && Col == other.Col;

    public override bool Equals(object? obj)
        => obj is Elf other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Row, Col);

    public static bool operator ==(Elf left, Elf right)
        => left.Equals(right);

    public static bool operator !=(Elf left, Elf right)
        => !(left == right);
}