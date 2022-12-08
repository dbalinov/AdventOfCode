using AdventOfCode.Year2022.Day1;
using AdventOfCode.Year2022.Day7;
using AdventOfCode.Year2022.Day8;

namespace AdventOfCode;

internal class DayFactory
{
    public IDay Create(int day)
        => day switch
        {
            1 => new Day1(),
            7 => new Day7(),
            8 => new Day8(),
            _ => throw new NotImplementedException(
                $"Solution for day: {day} is not implemented")
        };
}