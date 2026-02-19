namespace DirectoryCleaner.Utils;

public static class PathHelper
{
    public static string Normalize(string path)
    {
        return Path.GetFullPath(path);
    }

    public static bool IsUnderRoot(string path, string rootPath)
    {
        var fullPath = Path.GetFullPath(path)
            .TrimEnd(Path.DirectorySeparatorChar);

        var root = Path.GetFullPath(rootPath)
            .TrimEnd(Path.DirectorySeparatorChar);

        return fullPath == root ||
               fullPath.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.Ordinal); //fullPath.StartsWith(root + Path.DirectorySeparatorChar); 
    }

    public static string SanitizeFolderName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');

        return name.Trim();
    }
    public static string Combine(params string[] parts)
    {
        return Normalize(Path.Combine(parts));
    }
    public static bool IsDirectory(string path)
    {
        return Directory.Exists(path);
    }
    public static bool IsFile(string path)
    {
        return File.Exists(path);
    }
    public static bool IsSymlink(string path)
    {
        var info = new FileInfo(path);
        return info.Attributes.HasFlag(FileAttributes.ReparsePoint);
    }
    public static bool IsDirectoryEmpty(string path)
    {
        return !Directory.EnumerateFileSystemEntries(path).Any();
    }
}