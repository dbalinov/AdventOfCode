using AdventOfCode.Year2022.Day1;
using AdventOfCode.Year2022.Day4;
using AdventOfCode.Year2022.Day6;
using AdventOfCode.Year2022.Day7;
using AdventOfCode.Year2022.Day8;
using AdventOfCode.Year2022.Day9;
using AdventOfCode.Year2022.Day10;
using AdventOfCode.Year2022.Day2;

namespace AdventOfCode;

internal class DayFactory
{
    public IDay Create(int day)
        => day switch
        {
            1 => new Day1(),
            2 => new Day2(),
            4 => new Day4(),
            6 => new Day6(),
            7 => new Day7(),
            8 => new Day8(),
            9 => new Day9(),
            10 => new Day10(),
            _ => throw new NotImplementedException(
                $"Solution for day: {day} is not implemented")
        };
}