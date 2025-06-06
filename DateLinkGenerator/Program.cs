using Utils;
using static Utils.Command;

namespace DateSorter;

internal class Program
{
    static void Main()
    {
        GetPaths(out string sourceFolder, out string destinationFolder);

        var files           = Directory.EnumerateFiles(sourceFolder, "*", 
                                                       SearchOption.AllDirectories);
        var validFiles      = GetValidFiles(files);
        validFiles          = validFiles.OrderBy(f => f.date);

        string sourceFolderName = Path.GetFileName(sourceFolder)!;
        destinationFolder       = Path.Combine(destinationFolder, sourceFolderName);

        CopyFiles(validFiles, destinationFolder);
    }

    private static void GetPaths(out string sourceFolder, out string destinationFolder)
    {
        sourceFolder        = GetString("Enter source path");
        destinationFolder   = GetString("Enter destionation path");
    }

    private static IEnumerable<(DateTime date, string path)> GetValidFiles(IEnumerable<string> paths)
    {
        foreach (var path in paths)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);

            if (DateTime.TryParse(fileName, out var date))
                yield return new(date, path);
            else
                Console.WriteLine($"Isn't date file: {path}");
        }
    }

    private static void CopyFiles(IEnumerable<(DateTime date, string path)> files, string destinationFolder)
    {
        Directory.CreateDirectory(destinationFolder);

        CopyByYear(files, destinationFolder);
    }

    private static void CopyByYear(IEnumerable<(DateTime date, string path)> files, string destinationFolder)
    {
        var currentYear = DateTime.Today.Year;
        var yearGroup   = files.GroupBy(f => f.date.Year);

        Queue<string> links = new();

        foreach (var year in yearGroup.Reverse())
        {
            string yearFolder = destinationFolder;

            if (year.Key != currentYear)
                yearFolder = Path.Combine(destinationFolder, year.Key.ToString());

            CopyByMonth(year, yearFolder);
            GenerateYearLinks(year.Key, yearFolder, links);
        }

        //Сохраняем всё в файл, исключая последнюю пустую строку
        SaveLinks(links.SkipLast(1), destinationFolder);
    }

    private static void GenerateYearLinks(int year, string folder, Queue<string> links)
    {
        string header = FormatHelper.CreateHeader(year.ToString()!);
        links.Enqueue(header);

        InsertMonthLinks(folder, links);

        links.Enqueue("");
    }

    private static void InsertMonthLinks(string yearFolder, Queue<string> insertIn)
    {
        var files   = Directory.EnumerateFiles(yearFolder);
        files       = from s in files
                      let i = MonthConverter.FromString(
                            Path.GetFileNameWithoutExtension(s))
                      where i != -1
                      orderby i descending
                      select s;

        foreach (var file in files)
        {
            string link = LinkGenerator.Generate(yearFolder, file);

            insertIn.Enqueue(link);
        }
    }

    private static void CopyByMonth(IEnumerable<(DateTime date, string path)> files, string destinationFolder)
    {
        var monthGroup = files.GroupBy(f => f.date.Month);

        foreach (var month in monthGroup)
        {
            if (month.Key == DateTime.Now.Month)
            {
                foreach (var file in month)
                    CopyFile(destinationFolder, file.path, out var _);

                continue;
            }

            string monthName    = MonthConverter.ToString(month.Key);
            string monthFolder  =  Path.Combine(destinationFolder, monthName);

            Directory.CreateDirectory(monthFolder);

            Queue<string> links = new();

            foreach (var file in month)
            {
                CopyFile(monthFolder, file.path, out var newFilePath);

                string link = LinkGenerator.Generate(monthFolder, newFilePath);

                links.Enqueue(link);
            }

            SaveLinks(links, monthFolder);
        }
    }

    private static void CopyFile(string destinationFolder, string currentFilePath, 
                                 out string newFilePath)
    {
        string fileName     = Path.GetFileName(currentFilePath);
        newFilePath         = Path.Combine(destinationFolder, fileName);

        File.Copy(currentFilePath, newFilePath, true);
    }

    private static void SaveLinks(IEnumerable<string> links, string destinationFolder)
    {
        string filePath = destinationFolder += ".md";

        SaveText(links, filePath);
    }
}