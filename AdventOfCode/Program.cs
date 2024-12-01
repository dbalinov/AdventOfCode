using AdventOfCode;

var factory = new DayFactory();

var day = factory.Create(2024, 1);

Console.Title = day.Name;

await day.SolvePart1();
await day.SolvePart2();