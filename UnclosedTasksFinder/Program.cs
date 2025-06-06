using System.Text.RegularExpressions;
using static Utils.Command;

namespace UnclosedTasksFinder;

internal class Program
{
    private static readonly Regex _unclosedTaskRegex = new(@"\s*-\s\[\s\]\s+.+", RegexOptions.Compiled);

    static void Main()
    {
        GetPaths(out string searchDirectory, out string outFilePath);

        var files = GetValidFiles(searchDirectory);
        files     = files.OrderBy(f => f.date);

        IEnumerable<string> unclosedTasks = new Queue<string>();

        foreach (var file in files)
        {
            var fileUnclosedTasks = GetUnclosedTasks(file.path);

            unclosedTasks = unclosedTasks.Concat(fileUnclosedTasks);
        }

        unclosedTasks = unclosedTasks.Distinct();

        Save(unclosedTasks, outFilePath);
    }

    private static void GetPaths(out string searchDirectory, out string outFilePath)
    {
        searchDirectory = GetString("Enter search directory");
        outFilePath     = GetString("Enter output file path");
    }

    private static IEnumerable<(DateTime date, string path)> GetValidFiles(string directory)
    {
        foreach (var file in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            if (DateTime.TryParse(fileName, out var date))
                yield return (date, file);
            else
                Console.WriteLine($"Wrong date file: {file}");
        }
    }

    private static IEnumerable<string> GetUnclosedTasks(string file)
    {
        var lines = File.ReadAllLines(file);

        return lines.Select(line => line.TrimEnd())
                    .Where(line => _unclosedTaskRegex.IsMatch(line));
    }

    private static void Save(IEnumerable<string> unclosedTasks, string saveDirectory)
    {
        saveDirectory   = Path.Combine(saveDirectory, "Unclosed tasks.md");
        var stream      = File.Create(saveDirectory);

        using StreamWriter writer = new(stream);

        Console.WriteLine(unclosedTasks.Count());

        foreach (var line in unclosedTasks)
            writer.WriteLine(line);
    }
}