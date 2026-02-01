namespace DirectoryCleaner.Models;

public class FileItem
{
    public string FullPath { get; }
    public string Name { get; }
    public string Extension { get; }

    public FileItem(string path)
    {
        FullPath = path;
        Name = Path.GetFileName(path);
        Extension = Path.GetExtension(path);
    }
}