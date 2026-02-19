using DirectoryCleaner.Arguments;
using DirectoryCleaner.Models;
using DirectoryCleaner.Utils;

namespace DirectoryCleaner.Services;


/// <summary>
/// Scans directories for files to be processed by the DirectoryCleaner.
/// Supports recursive scanning and automatic filtering of hidden/temporary files.
/// </summary>

public class FileScanner
{
    private readonly Options _options;
    private readonly Logger _logger;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public FileScanner(Options options, Logger logger)
    {
        _options = options;
        _logger = logger;
    }
    
    public IEnumerable<FileItem> Scan()
    {
        var searchOption = _options.Recursive
            ? SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;
        
        if (_options.Verbose)
            _logger.Info($"Scanning with option: {searchOption}");
        
        IEnumerable<string> files;
        try
        {
            files = Directory.EnumerateFiles(
                _options.TargetPath,
                "*",
                searchOption);
        }
        catch (UnauthorizedAccessException)
        {
            _logger.Error($"Access denied to {_options.TargetPath}");
            yield break;
        }
        catch (DirectoryNotFoundException)
        {
            _logger.Error($"Directory not found: {_options.TargetPath}");
            yield break;
        }
        
        int count = 0;
        int skipped = 0;
        
        foreach (var filePath in files)
        {
            // Skip hidden/temp files
            if (ShouldSkip(filePath))
            {
                skipped++;
                if (_options.Verbose)
                    _logger.Info($"Skipping: {Path.GetFileName(filePath)}");
                continue;
            }
            
            FileItem? item = null;
            try
            {
                item = new FileItem(filePath);
                count++;
                
                if (_options.Verbose && count % 10 == 0)
                    _logger.Info($"Scanned {count} files...");
            }
            catch (Exception ex)
            {
                _logger.Warning($"Could not process {filePath}: {ex.Message}");
                continue;
            }
            
            if (item != null)
                yield return item;
        }
        
        if (_options.Verbose)
        {
            _logger.Success($"Scan complete: {count} total files");
            if (skipped > 0)
                _logger.Info($"Skipped {skipped} hidden/temp files");
        }
    }
    private bool ShouldSkip(string path)
    {
        var fileName = Path.GetFileName(path);
        // Skip hidden files on Linux
        if (fileName.StartsWith("."))
            return true;
        
        // Skip system/temp files
        if (fileName.EndsWith("~") || fileName.EndsWith(".tmp"))
            return true;
        
        return false;
    }


}