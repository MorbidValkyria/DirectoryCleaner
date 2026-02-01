using DirectoryCleaner.Arguments;
using DirectoryCleaner.Core;

namespace DirectoryCleaner.Services;

public class FileMover
{
    private readonly Options _options;
    
    public FileMover(Options options)
    {
        _options = options;
    }
    
    public void Move(FileAction action)
    {
        // Get the destination directory
        string? destinationDir = Path.GetDirectoryName(action.DestinationPath);
        
        if (string.IsNullOrEmpty(destinationDir))
            throw new InvalidOperationException($"Invalid destination path: {action.DestinationPath}");
        
        // Create destination directory if it doesn't exist
        if (!Directory.Exists(destinationDir))
        {
            Directory.CreateDirectory(destinationDir);
        }
        
        // Handle file name conflicts
        string finalDestination = HandleConflict(action.DestinationPath);
        
        // Move the file
        File.Move(action.SourcePath, finalDestination);
    }
    
    private string HandleConflict(string destinationPath)
    {
        // If no conflict, return as-is
        if (!File.Exists(destinationPath))
            return destinationPath;
        
        // File exists - append a number to make it unique
        string directory = Path.GetDirectoryName(destinationPath) ?? "";
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(destinationPath);
        string extension = Path.GetExtension(destinationPath);
        
        int counter = 1;
        string newPath;
        
        do
        {
            string newFileName = $"{fileNameWithoutExt} ({counter}){extension}";
            newPath = Path.Combine(directory, newFileName);
            counter++;
        }
        while (File.Exists(newPath));
        
        return newPath;
    }
}