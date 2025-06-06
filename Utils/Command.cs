namespace Utils;

public static class Command
{
    public static string GetString(string message)
    {
        Console.WriteLine(message);

        return Console.ReadLine()!;
    }

    public static ConsoleKeyInfo GetKey(string message)
    {
        Console.WriteLine(message);

        return Console.ReadKey();
    }

    public static void SaveText(IEnumerable<string> lines, string filePath)
    {
        var fs = File.CreateText(filePath);

        foreach (string line in lines)
            fs.WriteLine(line);

        fs.Dispose();
    }
}