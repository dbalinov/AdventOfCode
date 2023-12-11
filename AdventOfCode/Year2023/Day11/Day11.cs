namespace AdventOfCode.Year2023.Day11;

/// <summary>
/// Cosmic Expansion
/// </summary>
internal class Day11 : IDay
{
     private const string FileName = "Year2023/Day11/input.txt";

     public string Name => "Cosmic Expansion";

     public Task SolvePart1() => Solve(2);
     public Task SolvePart2() => Solve(1000000);

     private static async Task Solve(int expandBy)
     {
         var lines = await File.ReadAllLinesAsync(FileName);

         var galaxies = GetGalaxies(lines);
         var expandedGalaxies = Expand(galaxies, expandBy);

         var result = 0d;
         for(var i = 0; i < expandedGalaxies.Count; i++)
         {
             for(var j = i + 1; j < expandedGalaxies.Count; j++)
             {
                 var p1 = expandedGalaxies[i];
                 var p2 = expandedGalaxies[j];
                 var distance = p1.GetManhattanDistance(p2);
                 result += distance;
             }
         }

         Console.WriteLine(result);
     }
     
     private static List<Point> Expand(IList<Point> galaxies, int expandBy)
     {
         var minRow = galaxies.Min(x => x.Row);
         var maxRow = galaxies.Max(x => x.Row);

         var emptyRows = new List<int>();
         for (var row = minRow; row <= maxRow; row++)
         {
             if (galaxies.Any(x => x.Row == row))
             {
                 continue;
             }
             
             emptyRows.Add(row);
         }
         
         var minCol = galaxies.Min(x => x.Col);
         var maxCol = galaxies.Max(x => x.Col);

         var emptyCols = new List<int>();
         for(var col = minCol; col <= maxCol; col++)
         {
             if (galaxies.Any(x => x.Col == col))
             {
                 continue;
             }
             
             emptyCols.Add(col);
         }

         var expandedGalaxies = new List<Point>();
         foreach (var galaxy in galaxies)
         {
             var row = galaxy.Row;
             var col = galaxy.Col;
             
             var countOfEmptyRows = emptyRows.Count(x => x < row);
             var countOfEmptyCols = emptyCols.Count(x => x < col);
             
             row += countOfEmptyRows * (expandBy - 1);
             col += countOfEmptyCols * (expandBy - 1);
             
             expandedGalaxies.Add(new Point(row, col));
         }
         
         return expandedGalaxies;
     }

     private static List<Point> GetGalaxies(IList<string> lines)
     {
         var galaxies = new List<Point>();
         for (var row = 0; row < lines.Count; row++)
         {
             for (var col = 0; col < lines[row].Length; col++)
             {
                 if (lines[row][col] == '#')
                 {
                     galaxies.Add(new Point(row, col));
                 }
             }
         }

         return galaxies;
     }
}