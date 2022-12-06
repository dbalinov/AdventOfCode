using AdventOfCode.Year2022.Day1;

namespace AdventOfCode;

internal class DayFactory
{
    public IDay Create(int day)
        => day switch
        {
            1 => new Day1(),
            _ => throw new NotImplementedException(
                $"Solution for day: {day} is not implemented")
        };
}