using System.Diagnostics;

namespace otus.task;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var stopwatch = Stopwatch.StartNew();
        await CalculateSpaceInDirectoryAsync("C:\\otus");

        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
    }

    private static async Task CalculateSpaceInDirectoryAsync(string path)
    {
        var tasks = Directory.GetFiles(path).Select(file => CalculateSpaceInFileAsync(file)).ToList();
        var spaseInFiles = await Task.WhenAll(tasks);

        Console.WriteLine($"Прочитано {tasks.Count} файлов");

        for (var i = 0; i < spaseInFiles.Length; i++)
        {
            Console.WriteLine($"Файл {i + 1}: количество пробелов {spaseInFiles[i]}");
        }

        Console.WriteLine($"Суммарное количество пробелов {spaseInFiles.Sum(s => s)}");
    }

    private static async Task<int> CalculateSpaceInFileAsync(string path, CancellationToken cancellationToken = default) 
    {
        var fileSpaceCount = 0;
        using (var reader = new StreamReader(path))
        {
            while (await reader.ReadLineAsync(cancellationToken) is { } line)
            {
                fileSpaceCount += line.Count(c => c == ' ');
            }
        }

        return fileSpaceCount;
    }
}