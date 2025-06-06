namespace Utils;

public static class LinkGenerator
{
    public static string Generate(string directory, string filePath)
    {
        filePath = Path.GetRelativePath(directory, filePath);

        return FormatHelper.CreateLink(filePath);
    }
}