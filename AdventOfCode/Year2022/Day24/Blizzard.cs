namespace AdventOfCode.Year2022.Day24;

public class Blizzard
{
    public char Value { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }

    public void Move(int maxRow, int maxCol)
    {
        switch (Value)
        {
            case '>':
                Col = (Col + 1 > maxCol - 1) ? 1 : Col + 1;
                break;
            case '<':
                Col = (Col - 1 < 1) ? maxCol - 1 : Col - 1;
                break;
            case 'v':
                Row = (Row + 1 > maxRow - 1) ? 1 : Row + 1;
                break;
            case '^':
                Row = (Row - 1 < 1) ? maxRow - 1 : Row - 1;
                break;
            case '#':
                break;
            default:
                throw new NotSupportedException($"Not supported value: {Value}");
        }
    }
}