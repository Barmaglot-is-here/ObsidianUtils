namespace Utils;

public static class FormatHelper
{
    public static string CreateLink(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);

        return $"- [[{filePath}|{fileName}]]";
    }

    public static string CreateHeader(string text) => $"# {text}";
}