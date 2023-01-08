namespace AdventOfCode.Year2019.Day10;

internal sealed class Day10 : IDay
{
    private const string FileName = "Year2019/Day10/input.txt";

    public string Name => "Monitoring Station";
    
    public async Task SolvePart1()
    {
        var lines = File.ReadLinesAsync(FileName);

        var asteroids = await GetAsteroids(lines);

        var angles = new Dictionary<Asteroid, Dictionary<Asteroid, double>>();

        foreach (var asteroidFrom in asteroids)
        {
            foreach (var asteroidTo in asteroids)
            {
                if (asteroidFrom.Equals(asteroidTo))
                {
                    continue;
                }

                if (!angles.ContainsKey(asteroidFrom))
                {
                    angles.Add(asteroidFrom, new Dictionary<Asteroid, double>());
                }

                var angle = asteroidFrom.GetAngle(asteroidTo);

                angles[asteroidFrom].Add(asteroidTo, angle);
            }
        }

        Asteroid stationAsteroid = default;
        var max = 0;

        foreach (var (asteroidFrom, asteroidsTo) in angles)
        {
            var count = asteroidsTo.GroupBy(x => x.Value).Count();

            if (max < count)
            {
                stationAsteroid = asteroidFrom;
                max = count;
            }
        }

        Console.WriteLine(max);
        Console.WriteLine("Station asteroid ({0},{1})", stationAsteroid.X, stationAsteroid.Y);
    }
    
    public async Task SolvePart2()
    {
        var lines = File.ReadLinesAsync(FileName);

        var asteroids = await GetAsteroids(lines);

        var stationAsteroid = new Asteroid(22, 28);

        var angles = new Dictionary<Asteroid, (double Angle, double Distance)>();

        foreach (var asteroidTo in asteroids.Where(x => !x.Equals(stationAsteroid)))
        {
            var angle = stationAsteroid.GetAngle(asteroidTo);
            var distance = stationAsteroid.GetDistance(asteroidTo);

            angles.Add(asteroidTo, (angle, distance));
        }

        var queues = angles
            .OrderBy(x => x.Value.Angle >= 90
                ? x.Value.Angle
                : x.Value.Angle + 360)
            .GroupBy(x => x.Value.Angle)
            .Select(g => new Queue<Asteroid>(g
                .OrderBy(x => x.Value.Distance)
                .Select(x => x.Key)))
            .ToList();

        var i = 0;
        Asteroid vaporized200;
        while (true)
        {
            foreach (var queue in queues)
            {
                if (queue.Count == 0 || !queue.TryDequeue(out var vaporized))
                {
                    continue;
                }
                
                vaporized200 = vaporized;
                
                i++;
                
                if (i == 200)
                {
                    goto result;
                }
            }
        }

        result:  
        var result = 100 * vaporized200.X + vaporized200.Y;

        Console.WriteLine(result);
    }

    private static async Task<IList<Asteroid>> GetAsteroids(IAsyncEnumerable<string> lines)
    {
        var asteroids = new List<Asteroid>();
        var y = 0;
        await foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == '#')
                {
                    asteroids.Add(new Asteroid(x, y));
                }

                x++;
            }

            y++;
        }

        return asteroids;
    }
}