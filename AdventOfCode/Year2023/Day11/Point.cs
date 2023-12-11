namespace AdventOfCode.Year2023.Day11;

public readonly struct Point(int row, int col)
{
    public int Row { get; } = row;
    public int Col { get; } = col;

    public int GetManhattanDistance(Point p)
        => Math.Abs(Col - p.Col) + Math.Abs(Row - p.Row);
}