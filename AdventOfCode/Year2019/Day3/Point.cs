namespace AdventOfCode.Year2019.Day3;

public struct Point
{
    public Point(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public int Row { get; set; }
    public int Col { get; set; }

    public int GetManhattanDistance(Point p)
        => Math.Abs(Col - p.Col) + Math.Abs(Row - p.Row);
}