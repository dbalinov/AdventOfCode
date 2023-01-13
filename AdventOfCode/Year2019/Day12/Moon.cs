namespace AdventOfCode.Year2019.Day12;

public class Moon : IEquatable<Moon>
{
    public (int x, int y, int z) Position { get; set; }

    public (int x, int y, int z) Velocity { get; set; }

    public int PotentialEnergy
        => Math.Abs(Position.x) + Math.Abs(Position.y) + Math.Abs(Position.z);

    public int KineticEnergy
        => Math.Abs(Velocity.x) + Math.Abs(Velocity.y) + Math.Abs(Velocity.z);

    public int TotalEnergy => PotentialEnergy * KineticEnergy;

    public bool Equals(Moon other)
        => Position.Equals(other.Position) &&
           Velocity.Equals(other.Velocity);

    public override bool Equals(object? obj)
        => obj is Moon other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Position, Velocity);
}