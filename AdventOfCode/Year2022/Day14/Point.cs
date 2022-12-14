namespace AdventOfCode.Year2022.Day14;

public struct Point
{
    public Point(int col, int row)
    {
        Col = col;
        Row = row;
    }

    public int Col { get; set; }
    public int Row { get; set; }
}