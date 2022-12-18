namespace AdventOfCode.Year2022.Day18;

public struct Point3D : IEquatable<Point3D>
{
    public Point3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public static Point3D operator +(Point3D left, Point3D right)
        => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public static Point3D operator -(Point3D left, Point3D right)
        => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    public static bool operator ==(Point3D left, Point3D right)
        => left.Equals(right);

    public static bool operator !=(Point3D left, Point3D right)
        => !(left == right);

    public bool Equals(Point3D other)
        => X == other.X && Y == other.Y && Z == other.Z;

    public override bool Equals(object? obj)
        => obj is Point3D other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(X, Y, Z);
}