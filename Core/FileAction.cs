namespace DirectoryCleaner.Core;

public class FileAction
{
    public string SourcePath { get; }
    public string DestinationPath { get; }
    
    public FileAction(string sourcePath, string destinationPath)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }
}