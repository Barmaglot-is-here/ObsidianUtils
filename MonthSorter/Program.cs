using Utils;
using static Utils.Command;

namespace MonthSorter;

internal class Program
{
    static void Main()
    {
        GetPaths(out string notesPath, out string outputPath);

        var paths           = Directory.EnumerateFiles(notesPath);
        var datePathPair    = GetValidDates(paths);

        foreach (var pair in datePathPair)
        {
            ValidateDirectoryExists(pair.Key, outputPath, out string destinationPath);

            string fileName = Path.GetFileName(pair.Value);
            string newPath  = Path.Combine(destinationPath, fileName);
            string oldPath  = pair.Value;

            Console.WriteLine(oldPath);

            File.Copy(oldPath, newPath);
        }
    }

    private static void GetPaths(out string notesPath, out string outputPath)
    {
        notesPath   = GetString("Enter notes path");
        outputPath  = GetString("Enter output path");
    }

    private static void ValidateDirectoryExists(DateTime date, string outputPath, 
                                                out string destinationPath)
    {
        string mounthName = date.GetMonthName();

        destinationPath = Path.Combine(outputPath, mounthName);

        if (!Directory.Exists(destinationPath))
            Directory.CreateDirectory(destinationPath);
    }

    private static IEnumerable<KeyValuePair<DateTime, string>> GetValidDates(
        IEnumerable<string> paths)
    {
        foreach (var path in paths)
        {
            string date = Path.GetFileNameWithoutExtension(path);

            if (DateTime.TryParse(date, out var result))
                yield return new(result, path);
            else
                Console.WriteLine($"Wrong date: {path}");
        }
    }
}