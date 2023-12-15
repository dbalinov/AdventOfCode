namespace AdventOfCode.Year2023.Day15;

/// <summary>
/// Lens Library
/// </summary>
internal class Day15 : IDay
{
     private const string FileName = "Year2023/Day15/input.txt";

     public string Name => "Lens Library";

     public async Task SolvePart1()
     {
         var text = await File.ReadAllTextAsync(FileName);

         var result = text
             .Split(',')
             .Sum(GetHashCode);

         Console.WriteLine(result);
     }

     public async Task SolvePart2()
     {
         var text = await File.ReadAllTextAsync(FileName);

         var hashMap = new List<(string label, int focalLength)>?[256];
         foreach (var item in text.Split(','))
         {
             var indexOfEquals = item.IndexOf('=');
             if (indexOfEquals > -1)
             {
                 var label = item[..indexOfEquals];
                 var hashCode = GetHashCode(label);
                 hashMap[hashCode] ??= [];
                 var indexOfItem = hashMap[hashCode]!.FindIndex(x => x.label == label);
                 
                 var focalLength = int.Parse(item[(indexOfEquals + 1)..]);
                 if (indexOfItem > -1)
                 {
                     hashMap[hashCode]![indexOfItem] = (label, focalLength);
                 }
                 else
                 {
                     hashMap[hashCode]!.Add((label, focalLength));
                 }
             }
             else
             {
                 var indexOfDash = item.IndexOf('-');
                 var label = item[..indexOfDash];
                 var hashCode = GetHashCode(label);

                 if (hashMap[hashCode] is not null)
                 {
                     var indexOfItem = hashMap[hashCode]!.FindIndex(x => x.label == label);
                     if (indexOfItem > -1)
                     {
                         hashMap[hashCode]!.RemoveAt(indexOfItem);
                     }
                 }
             }
         }

         var result = 0;
         for (var box = 0; box < hashMap.Length; box++)
         {
             var slots = hashMap[box];
             if (slots is null)
             {
                 continue;
             }
             
             for(var slot = 0; slot < slots.Count; slot++)
             {
                 var item = slots[slot];
                 result += (box + 1) * (slot + 1) * item.focalLength;
             }
         }

         Console.WriteLine(result);
     }
     
     private static int GetHashCode(IEnumerable<char> item)
     {
         var currentValue = 0;
         foreach (var ch in item)
         {
             currentValue = (currentValue + ch) * 17 % 256;
         }

         return currentValue;
     }
}