namespace AdventOfCode.Year2019.Day4;

public sealed class Day4 : IDay
{
    public string Name => "Secure Container";

    public Task SolvePart1()
    {
        const string input = "234208-765869";
        var numbers = input.Split('-').Select(int.Parse).ToList();

        var count = 0;
        for (var i = numbers[0]; i <= numbers[1]; i++)
        {
            if (IsValidPart1(i.ToString()))
            {
                count++;
            }
        }
        
        Console.WriteLine(count);

        return Task.CompletedTask;
    }

    public Task SolvePart2()
    {
        const string input = "234208-765869";
        var numbers = input.Split('-').Select(int.Parse).ToList();

        var count = 0;
        for (var i = numbers[0]; i <= numbers[1]; i++)
        {
            if (IsValidPart2(i.ToString()))
            {
                count++;
            }
        }

        Console.WriteLine(count);

        return Task.CompletedTask;
    }

    private static bool IsValidPart1(string password)
    {
        var hasTwoAdjacentDigits = false;
        var digitsIncrease = true;
        for (var i = 0; i < password.Length - 1; i++)
        {
            hasTwoAdjacentDigits |= password[i] == password[i + 1];

            digitsIncrease &= password[i] - '0' <= password[i + 1] - '0';
        }

        return hasTwoAdjacentDigits && digitsIncrease;
    }

    private static bool IsValidPart2(string password)
    {
        var dictionary = new Dictionary<char, int>();

        foreach (var c in password)
        {
            if (!dictionary.ContainsKey(c))
            {
                dictionary.Add(c, 1);
            }
            else
            {
                dictionary[c]++;
            }
        }

        var hasTwoAdjacentDigits = dictionary.Values.Any(x => x == 2);

        var digitsIncrease = true;
        for (var i = 0; i < password.Length - 1; i++)
        {
            digitsIncrease &= password[i] - '0' <= password[i + 1] - '0';
        }

        return hasTwoAdjacentDigits && digitsIncrease;
    }
}