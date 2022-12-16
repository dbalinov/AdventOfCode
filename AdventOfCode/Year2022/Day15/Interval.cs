namespace AdventOfCode.Year2022.Day15;
public readonly struct Interval
{
    public Interval(int start, int end)
    {
        Start = start;
        End = end;
    }

    public int Start { get; init; }
    public int End { get; init; }

    public bool Overlaps(Interval other)
        => other.IsInRange(Start) || other.IsInRange(End);

    public bool IsInRange(int item)
        => item >= Start && item <= End;
}