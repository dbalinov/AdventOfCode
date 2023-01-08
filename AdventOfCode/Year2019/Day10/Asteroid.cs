namespace AdventOfCode.Year2019.Day10;

public struct Asteroid : IEquatable<Asteroid>
{
    public Asteroid(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public double GetDistance(Asteroid other)
        => Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));

    public double GetAngle(Asteroid other)
    {
        var radians = Math.Atan2(Y - other.Y, X - other.X);
        return (radians / Math.PI * 180) + (radians > 0 ? 0 : 360);
    }

    public bool Equals(Asteroid other)
        => X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object? obj)
        => obj is Asteroid other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(X, Y);
    
    public override string ToString()
        => $"({X},{Y})";
}