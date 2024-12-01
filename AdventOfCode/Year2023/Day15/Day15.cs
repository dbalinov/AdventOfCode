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

         var boxes = new Box?[256];
         foreach (var item in text.Split(','))
         {
             var indexOfEquals = item.IndexOf('=');
             if (indexOfEquals > -1)
             {
                 var label = item[..indexOfEquals];
                 var hashCode = GetHashCode(label);
                 
                 var focalLength = int.Parse(item[(indexOfEquals + 1)..]);
                 
                 boxes[hashCode] ??= new(hashCode);
                 var box = boxes[hashCode]!;
                 box.SetLens(label, focalLength);
             }
             else
             {
                 var label = item[..^1];
                 var hashCode = GetHashCode(label);
                 
                 boxes[hashCode] ??= new(hashCode);
                 var box = boxes[hashCode]!;
                 
                 box.RemoveLens(label);
             }
         }
         
         var result = boxes
            .Where(box => box is not null)
            .Sum(box => box!.GetFocusingPower());
         
         Console.WriteLine(result);
     }

     public async Task SolvePart2_2()
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

     private class Box(int box)
     {
         private readonly Dictionary<string, Node> _lenses = new();
         private Node? _head;
         private Node? _tail;
            
         public void SetLens(string label, int focalLength)
         {
             if (_lenses.TryGetValue(label, out var node))
             {
                 node.FocalLength = focalLength;
             }
             else
             {
                 var newNode = new Node
                 {
                     Label = label,
                     FocalLength = focalLength
                 };
                 
                 _lenses[label] = newNode;
                 
                 if (_head is null)
                 {
                     _head = _tail = newNode;
                 }
                 else
                 {
                     _tail!.Next = newNode;
                     newNode.Previous = _tail;
                     _tail = newNode;
                 }
             }
         }

         public void RemoveLens(string label)
         {
             if (!_lenses.TryGetValue(label, out var node))
             {
                 return;
             }
             
             if (node == _head)
             {
                 _head = _head.Next;
             }
             if (node == _tail)
             {
                 _tail = _tail.Previous;
             }
                 
             if (node.Previous is not null)
             {
                 node.Previous.Next = node.Next;
             }
             if (node.Next is not null)
             {
                 node.Next.Previous = node.Previous;
             }
             
             _lenses.Remove(label);
         }

         public int GetFocusingPower()
         {
             var sum = 0;

             var current = _head;
             var index = 1;
             while (current is not null)
             {
                 sum += (box + 1) * index * current.FocalLength;
                 current = current.Next;
                 index++;   
             }

             return sum;
         }
     }

     private class Node
     {
         public Node? Next { get; set; }
         public Node? Previous { get; set; }
         public required string Label { get; set; }
         public int FocalLength { get; set; }
     }
}