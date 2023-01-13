namespace AdventOfCode.Year2019.Day12;

internal sealed class Day12 : IDay
{
    public string Name => "The N-Body Problem";

    public Task SolvePart1()
    {
        var moons = InitialState;

        for (var step = 1; step <= 1000; step++)
        {
            ApplyGravity(moons);
            ApplyVelocity(moons);
        }

        var totalEnergy = moons.Sum(moon => moon.TotalEnergy);

        Console.WriteLine(totalEnergy);

        return Task.CompletedTask;
    }

    public Task SolvePart2()
    {
        var targetState = InitialState;
        
        var periodX = GetPeriod(
            InitialState, targetState,
            (pos1, pos2) => pos1.x == pos2.x);

        var periodY = GetPeriod(
            InitialState, targetState,
            (pos1, pos2) => pos1.y == pos2.y);

        var periodZ = GetPeriod(
            InitialState, targetState,
            (pos1, pos2) => pos1.z == pos2.z);
        
        var repeatAt = LeastCommonMultiple(periodX,
            LeastCommonMultiple(periodY, periodZ));
        
        Console.WriteLine(repeatAt);

        return Task.CompletedTask;
    }

    public static IList<Moon> InitialState => new List<Moon>
    {
        new() { Position = (-16, 15, -9), Velocity = (0, 0, 0) },
        new() { Position = (-14, 5, 4), Velocity = (0, 0, 0) },
        new() { Position = (2, 0, 6), Velocity = (0, 0, 0) },
        new() { Position = (-3, 18, 9), Velocity = (0, 0, 0) }
    };

    private static long GetPeriod(IList<Moon> moons, IList<Moon> targetState,
        Func<(int x, int y, int z), (int x, int y, int z), bool> comparePosition)
    {
        var period = 1L;

        while (true)
        {
            ApplyGravity(moons);
            ApplyVelocity(moons);

            var allRepeat = true;
            for (var i = 0; i < moons.Count; i++)
            {
                allRepeat &= comparePosition(moons[i].Position, targetState[i].Position);
            }

            period++;

            if (allRepeat)
            {
                break;
            }
        }

        return period;
    }

    private static void ApplyVelocity(IEnumerable<Moon> moons)
    {
        foreach (var moon in moons)
        {
            moon.Position = (
                moon.Position.x + moon.Velocity.x,
                moon.Position.y + moon.Velocity.y,
                moon.Position.z + moon.Velocity.z);
        }
    }

    private static void ApplyGravity(IList<Moon> moons)
    {
        foreach (var current in moons)
        {
            var newVelocity = (current.Velocity.x, current.Velocity.y, current.Velocity.z);

            foreach (var other in moons)
            {
                if (current.Equals(other))
                {
                    continue;
                }

                newVelocity.x -= current.Position.x.CompareTo(other.Position.x);
                newVelocity.y -= current.Position.y.CompareTo(other.Position.y);
                newVelocity.z -= current.Position.z.CompareTo(other.Position.z);
            }

            current.Velocity = newVelocity;
        }
    }

    public static long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static long LeastCommonMultiple(long a, long b)
        => a * b / GreatestCommonDivisor(a, b);
}