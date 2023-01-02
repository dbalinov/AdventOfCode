namespace AdventOfCode;

public interface IDay
{
    string Name { get; }
    Task SolvePart1();
    Task SolvePart2();
}