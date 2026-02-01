using DirectoryCleaner.Models;

namespace DirectoryCleaner.Core;

public class RuleEngine
{
    private readonly string _baseTargetPath;
    
    public RuleEngine(string baseTargetPath)
    {
        _baseTargetPath = baseTargetPath;
    }
    
    public FileAction? DetermineAction(FileItem file)
    {
        // Determine target subfolder based on file type
        string? targetFolder = DetermineTargetFolder(file);
        
        if (targetFolder == null)
            return null;  // No action needed for this file
        
        // Create full destination path
        string destinationDir = Path.Combine(_baseTargetPath, targetFolder);
        string destinationPath = Path.Combine(destinationDir, file.Name);
        
        // Don't move if already in the correct location
        if (Path.GetDirectoryName(file.FullPath) == destinationDir)
            return null;
        
        return new FileAction(file.FullPath, destinationPath);
    }
    
    private string? DetermineTargetFolder(FileItem file)
    {
        // Rule 1: Images
        if (IsImage(file.Extension))
            return "Images";
        
        // Rule 2: Documents
        if (IsDocument(file.Extension))
            return "Documents";
        
        // Rule 3: Videos
        if (IsVideo(file.Extension))
            return "Videos";
        
        // Rule 4: Audio
        if (IsAudio(file.Extension))
            return "Audio";
        
        // Rule 5: Archives
        if (IsArchive(file.Extension))
            return "Archives";
        
        // Rule 6: Code files
        if (IsCode(file.Extension))
            return "Code";
        
        // Rule 7: Executables and scripts
        if (IsExecutable(file.Extension))
            return "Programs";
        
        // Default: no specific folder (leave in place)
        return null;
    }
    
    private bool IsImage(string extension)
    {
        string[] imageExts = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".webp", ".ico", ".tiff" };
        return imageExts.Contains(extension.ToLower());
    }
    
    private bool IsDocument(string extension)
    {
        string[] docExts = { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".odt", ".xlsx", ".xls", ".pptx", ".ppt", ".csv" };
        return docExts.Contains(extension.ToLower());
    }
    
    private bool IsVideo(string extension)
    {
        string[] videoExts = { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".m4v" };
        return videoExts.Contains(extension.ToLower());
    }
    
    private bool IsAudio(string extension)
    {
        string[] audioExts = { ".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a", ".wma" };
        return audioExts.Contains(extension.ToLower());
    }
    
    private bool IsArchive(string extension)
    {
        string[] archiveExts = { ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz", ".iso" };
        return archiveExts.Contains(extension.ToLower());
    }
    
    private bool IsCode(string extension)
    {
        string[] codeExts = { ".cs", ".py", ".js", ".java", ".cpp", ".c", ".h", ".html", ".css", ".json", ".xml", ".sql", ".sh", ".go", ".rs", ".ts" };
        return codeExts.Contains(extension.ToLower());
    }
    
    private bool IsExecutable(string extension)
    {
        string[] execExts = { ".exe", ".dll", ".so", ".AppImage", ".deb", ".rpm" };
        return execExts.Contains(extension.ToLower());
    }
}