using DirectoryCleaner.Arguments;
using DirectoryCleaner.Services;
using DirectoryCleaner.Core;

namespace DirectoryCleaner.Core;

public class Cleaner
{
    private readonly Options _options;
    
    public Cleaner(Options options)
    {
        _options = options;
    }
    
    public void Run()
    {
        Console.WriteLine($"Cleaning directory: {_options.TargetPath}");
        Console.WriteLine($"Mode: {(_options.DryRun ? "DRY RUN (no changes will be made)" : "LIVE")}");
        Console.WriteLine($"Recursive: {_options.Recursive}");
        Console.WriteLine();
        
        // 1. Scan files
        var scanner = new FileScanner(_options);
        var files = scanner.Scan().ToList();
        
        Console.WriteLine($"Found {files.Count} files");
        Console.WriteLine();
        
        // 2. Apply rules to determine actions
        var ruleEngine = new RuleEngine(_options.TargetPath);
        var actions = new List<FileAction>();
        
        foreach (var file in files)
        {
            var action = ruleEngine.DetermineAction(file);
            if (action != null)
            {
                actions.Add(action);
            }
        }
        
        Console.WriteLine($"{actions.Count} files will be organized");
        Console.WriteLine();
        
        // 3. Execute or preview actions
        if (_options.DryRun)
        {
            PreviewActions(actions);
        }
        else
        {
            ExecuteActions(actions);
        }
        
        Console.WriteLine();
        Console.WriteLine("Done!");
    }
    
    private void PreviewActions(List<FileAction> actions)
    {
        Console.WriteLine("=== DRY RUN - Preview of changes ===");
        foreach (var action in actions)
        {
            Console.WriteLine($"WOULD MOVE: {action.SourcePath}");
            Console.WriteLine($"        TO: {action.DestinationPath}");
            Console.WriteLine();
        }
    }
    
    private void ExecuteActions(List<FileAction> actions)
    {
        var mover = new FileMover(_options);
        int successCount = 0;
        int failCount = 0;
        
        foreach (var action in actions)
        {
            try
            {
                mover.Move(action);
                successCount++;
                Console.WriteLine($"✓ Moved: {action.SourcePath}");
            }
            catch (Exception ex)
            {
                failCount++;
                Console.WriteLine($"✗ Failed: {action.SourcePath} - {ex.Message}");
            }
        }
        
        Console.WriteLine();
        Console.WriteLine($"Success: {successCount}, Failed: {failCount}");
    }
}