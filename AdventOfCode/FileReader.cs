using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class FileReader
{
    public static async IAsyncEnumerable<string> GetAllLinesAsync(string path,
        [EnumeratorCancellation]CancellationToken cancellationToken = default)
    {
        using var streamReader = new StreamReader(path);

        string? line;
        while ((line = await streamReader.ReadLineAsync(cancellationToken)) != null)
        {
            yield return line;
        }
    }
}