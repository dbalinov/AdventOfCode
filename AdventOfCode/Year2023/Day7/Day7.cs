namespace AdventOfCode.Year2023.Day7;

/// <summary>
/// Camel Cards
/// </summary>
internal class Day7 : IDay
{
    private const string FileName = "Year2023/Day7/input.txt";
    
    public string Name => "Camel Cards";

    public async Task SolvePart1()
    {
        var cardValues = new Dictionary<char, int>
        {
            { '2', 2 }, { '3', 3 }, { '4', 4 },
            { '5', 5 }, { '6', 6 }, { '7', 7 },
            { '8', 8 }, { '9', 9 }, { 'T', 10 },
            { 'J', 11 }, { 'Q', 12 }, { 'K', 13 }, { 'A', 14 },
        };
        
        var lines = File.ReadLinesAsync(FileName);

        var games = await ParseGamesAsync(lines);

        games = games
            .OrderBy(x => GetType1(x.Hand))
            .ThenBy(x => cardValues[x.Hand[0]])
            .ThenBy(x => cardValues[x.Hand[1]])
            .ThenBy(x => cardValues[x.Hand[2]])
            .ThenBy(x => cardValues[x.Hand[3]])
            .ThenBy(x => cardValues[x.Hand[4]]);
        
        var result = 0;
        var index = 1;
        foreach (var game in games)
        {
            result += game.Bid * index;
            
            index++;
        }
        
        Console.WriteLine(result);
    }
    
    public async Task SolvePart2()
    {
        var cardValues = new Dictionary<char, int>
        {
            { 'J', 1 },
            { '2', 2 }, { '3', 3 }, { '4', 4 },
            { '5', 5 }, { '6', 6 }, { '7', 7 },
            { '8', 8 }, { '9', 9 }, { 'T', 10 },
            { 'Q', 12 },  { 'K', 13 }, { 'A', 14 },
        };
    
        var lines = File.ReadLinesAsync(FileName);

        var games = await ParseGamesAsync(lines);

        games = games
            .OrderBy(x => GetType2(x.Hand))
            .ThenBy(x => cardValues[x.Hand[0]])
            .ThenBy(x => cardValues[x.Hand[1]])
            .ThenBy(x => cardValues[x.Hand[2]])
            .ThenBy(x => cardValues[x.Hand[3]])
            .ThenBy(x => cardValues[x.Hand[4]]);
        
        var result = 0;
        var index = 1;
        foreach (var game in games)
        {
            result += game.Bid * index;
            
            index++;
        }
        
        Console.WriteLine(result);
    }
    
    private static int GetType1(string hand)
    {
        var cards = new Dictionary<int, int>();
        
        foreach (var card in hand)
        {
            if (!cards.TryAdd(card, 1))
            {
                cards[card]++;
            }
        }

        return GetType(cards);
    }
    
    private static int GetType2(string hand)
    {
        var cards = new Dictionary<int, int>();
        
        foreach (var card in hand)
        {
            if (!cards.TryAdd(card, 1))
            {
                cards[card]++;
            }
        }

        if (hand.Count(x => x == 'J') == 5)
        {
            return GetType(cards);
        }
        
        var maxValue = cards.Where(c => c.Key != 'J').Max(c => c.Value);
        var maxCard = cards.FirstOrDefault(c => c.Key != 'J' && c.Value == maxValue);
        
        var jCard = cards.FirstOrDefault(c => c.Key == 'J');
        cards[maxCard.Key] += jCard.Value;
        cards.Remove(jCard.Key);

        return GetType(cards);
    }

    private static int GetType(IDictionary<int, int> cards)
    {
                
        if (cards.Count == 1)
        {
            return 6; // Five of a kind
        }
        
        if(cards.Values.Any(c => c == 4))
        {
            return 5; // Four of a kind
        }
        
        if (cards.Values.Any(c => c == 3))
        {
            if (cards.Values.Any(c => c == 2))
            {
                return 4; // Full house
            }
            return 3; // Three of a kind
        }
        
        if (cards.Values.Count(c => c == 2) == 2)
        {
            return 2; //Two pair
        }

        if (cards.Values.Count(c => c == 2) == 1 && cards.Values.Count(c => c == 1) == 3)
        {
            return 1; // One pair
        }
        
        if (cards.Values.Count == 5)
        {
            return 0; // High card
        }

        throw new InvalidOperationException("invalid hand");
    }
    
    private static async Task<IEnumerable<Game>> ParseGamesAsync(IAsyncEnumerable<string> lines)
    {
        var games = new List<Game>();
        
        await foreach (var line in lines)
        {
            var lineParts = line.Split(' ');
            
            var game = new Game
            {
                Hand = lineParts[0],
                Bid = int.Parse(lineParts[1])
            };

            games.Add(game);
        }

        return games;
    }
}