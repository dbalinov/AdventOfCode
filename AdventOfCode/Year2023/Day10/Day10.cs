namespace AdventOfCode.Year2023.Day10;

/// <summary>
/// Pipe Maze
/// </summary>
internal class Day10 : IDay
{
     private const string FileName = "Year2023/Day10/input.txt";

     public string Name => "Pipe Maze";

     public async Task SolvePart1()
     {
         var lines = await File.ReadAllLinesAsync(FileName);

         var start = GetStartLocation(lines);
         var adjacentStartLocations = GetAdjacentStartLocations(lines, start).ToList();

         var pathFunc = GetPathFunc(lines, adjacentStartLocations[0]);
         
         var pathLength = 1 + pathFunc(adjacentStartLocations[1]).Count;
         
         Console.WriteLine(Math.Ceiling(pathLength / 2.0));
     }
     
     /// <summary>
     /// Use ray casting algorithm to determine if a point is inside a polygon
     /// https://en.wikipedia.org/wiki/Point_in_polygon#Ray_casting_algorithm
     /// </summary>
     public async Task SolvePart2()
     {
         var lines = await File.ReadAllLinesAsync(FileName);

         var start = GetStartLocation(lines);
         var adjacentStartLocations = GetAdjacentStartLocations(lines, start).ToList();

         var pathFunc = GetPathFunc(lines, adjacentStartLocations[0]);
         var path = pathFunc(adjacentStartLocations[1]);
         path.Insert(0, start);

         var result = 0;
         for (var row = 0; row < lines.Length; row++)
         {
             var inLoop = false;
             char? horizontalCh = null;
             
             for(var col = 0; col < lines[row].Length; col++)
             {
                 var current = new Location(row, col);

                 if (path.Contains(current))
                 {
                     var ch = lines[row][col];
                     if (ch == 'S')
                         ch = MapStartChar(lines, current);
                     
                     if (ch == 'L' || ch == 'F')
                     {
                         horizontalCh = lines[row][col];
                     }
                     else if (horizontalCh == 'L' && ch == '7' ||
                         horizontalCh == 'F' && ch == 'J' ||
                         lines[row][col] == '|')
                     {
                         inLoop = !inLoop;
                     }
                 }
                 else if (inLoop)
                 {
                     result++;
                 }
             }
         }
         Console.WriteLine(result);
     }

     private static IEnumerable<Location> GetAdjacentStartLocations(IReadOnlyList<string> lines, Location l)
     {
         var ch = MapStartChar(lines, l);

         return GetNextLocations(ch, l)
             .Where(x => x.Row >= 0 && x.Row < lines.Count &&
                         x.Col >= 0 && x.Col < lines[0].Length);
     }

     private static char MapStartChar(IReadOnlyList<string> lines, Location l)
     {
         char ch;
         
         if (lines[l.Row][l.Col + 1] == '-' && lines[l.Row + 1][l.Col] == '|')
         {
             ch = 'F';
         }
         else if (lines[l.Row][l.Col - 1] == '-' && lines[l.Row - 1][l.Col] == '7')
         {
             ch = 'J';
         }
         else if (lines[l.Row + 1][l.Col] == 'J' && lines[l.Row][l.Col + 1] == '7')
         {
             ch = 'F';
         }
         else if (lines[l.Row + 1][l.Col] == '|' && lines[l.Row][l.Col - 1] == 'F')
         {
             ch = '7';
         }
         else
         {
             // TODO: implement other cases for new input
             throw new NotImplementedException();
         }

         return ch;
     }

     private static IEnumerable<Location> GetNextLocations(IReadOnlyList<string> lines, Location l)
     {
         var ch = lines[l.Row][l.Col];

         return GetNextLocations(ch, l)
             .Where(x => x.Row >= 0 && x.Row < lines.Count &&
                         x.Col >= 0 && x.Col < lines[0].Length);
     }

     private static IEnumerable<Location> GetNextLocations(char ch, Location l)
     {
         var north = new Location(l.Row - 1, l.Col);
         var east = new Location(l.Row, l.Col + 1);
         var south = new Location(l.Row + 1, l.Col);
         var west = new Location(l.Row, l.Col - 1);
         
         var nextLocations = ch switch
         {
             '|' => new[] { north, south },
             '-' => new[] { east, west },
             'L' => new[] { north, east },
             'J' => new[] { north, west },
             '7' => new[] { south, west },
             'F' => new[] { south, east },
             _  => Array.Empty<Location>(),
         };
         
         return nextLocations;
     }

     private static Func<Location, List<Location>> GetPathFunc(string[] lines, Location end)
     {
         var previous = new Dictionary<Location, Location>();
         
         var queue = new Queue<Location>();
         queue.Enqueue(end);

         while (queue.Count > 0)
         {
             var currentLocation = queue.Dequeue();
 
             var nextLocations = GetNextLocations(lines, currentLocation)
                 .Where(x => x.Row >= 0 && x.Row < lines.Length &&
                             x.Col >= 0 && x.Col < lines[0].Length);

             foreach (var nextLocation in nextLocations)
             {
                 if (!previous.TryAdd(nextLocation, currentLocation))
                     continue;

                 queue.Enqueue(nextLocation);
             }
         }

         return ShortestPathFn;

         List<Location> ShortestPathFn(Location v)
         {
             var path = new List<Location>();

             var current = v;
             path.Add(current);
             while (current != end || path.Count == 0) 
             {
                 previous.TryGetValue(current, out current);
                 path.Add(current);
             }

             return path;
         }
     }
     
     private static Location GetStartLocation(IReadOnlyList<string> lines)
     {
         Location? start = null;
         for (var row = 0; row < lines.Count; row++)
         {
             for (var col = 0; col < lines[0].Length; col++)
             {
                 if (lines[row][col] == 'S')
                     start = new Location(row, col);
             }
         }

         if (start is null)
             throw new Exception("Start not found");

         return start.Value;
     }
}