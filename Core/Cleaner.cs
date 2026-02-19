using DirectoryCleaner.Arguments;
using DirectoryCleaner.Services;
using DirectoryCleaner.Utils;

namespace DirectoryCleaner.Core;

public class Cleaner
{
    private readonly Options _options;
    private readonly Logger _logger;

    public Cleaner(Options options, Logger logger)
    {
        _options = options;
        _logger = logger;
    }

    public void Run()
    {
        _logger.Info($"Cleaning: {_options.TargetPath} [{(_options.DryRun ? "DRY RUN" : "LIVE")}{(_options.Recursive ? ", recursive" : "")}]");
        _logger.EmptyLine();

        var scanner = new FileScanner(_options, _logger);
        var files = scanner.Scan().ToList();

        if (files.Count == 0)
        {
            _logger.Warning("No files found to scan");
            return;
        }

        _logger.Info($"Found {files.Count} files");
        _logger.EmptyLine();

        var ruleEngine = new RuleEngine(_options.TargetPath);
        var actions = files
            .Select(f => ruleEngine.DetermineAction(f))
            .Where(a => a != null)
            .Cast<FileAction>()
            .ToList();

        if (actions.Count > 0)
            _logger.Success($"{actions.Count} files will be organized");
        else
            _logger.Warning("No files found to organize");

        _logger.EmptyLine();

        if (_options.DryRun)
            PreviewActions(actions);
        else
            ExecuteActions(actions);

        _logger.EmptyLine();
        _logger.Success("Done!");
    }

    private void PreviewActions(List<FileAction> actions)
    {
        _logger.Separator();
        _logger.Info("DRY RUN - Preview of changes");
        _logger.Separator();
        _logger.EmptyLine();

        foreach (var action in actions)
        {
            _logger.Info($"WOULD MOVE: {action.SourcePath}");
            _logger.Info($"        TO: {action.DestinationPath}");
            _logger.EmptyLine();
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
                _logger.Success($"Moved: {action.SourcePath}");
            }
            catch (Exception ex)
            {
                failCount++;
                _logger.Error($"Failed: {action.SourcePath} - {ex.Message}");
            }
        }

        _logger.EmptyLine();

        if (failCount == 0)
            _logger.Success($"All {successCount} files moved successfully");
        else
            _logger.Warning($"Success: {successCount}, Failed: {failCount}");
    }
}