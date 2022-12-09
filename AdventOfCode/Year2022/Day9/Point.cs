namespace AdventOfCode.Year2022.Day9;

public struct Point : IEquatable<Point>
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public static Point operator +(Point a, Point b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Point operator -(Point a, Point b)
        => new(a.X - b.X, a.Y - b.Y);

    public bool Equals(Point other)
        => X == other.X && Y == other.Y;

    public override bool Equals(object? obj)
        => obj is Point other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(X, Y);

    public static bool operator ==(Point left, Point right)
        => left.Equals(right);

    public static bool operator !=(Point left, Point right)
        => !(left == right);
}