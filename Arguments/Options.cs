namespace DirectoryCleaner.Arguments;

public class Options
{
    public string TargetPath { get; private set; } = "";
    public bool DryRun {get; private set;}
    public bool Recursive {get; private set;}
    public bool IsValid {get; private set;}
    public bool GetHelp {get; private set;}
    public bool Verbose {get; private set;}
    public string? LogFile {get; private set;}

public static Options Parse(string[] args)
{
    var options = new Options();
    
    for (int i = 0; i < args.Length; i++)
    {
        var arg = args[i];
        
        if (arg == "--help" || arg == "-h")
            options.GetHelp = true;
        else if (arg == "--dry-run")
            options.DryRun = true;
        else if (arg == "--recursive")
            options.Recursive = true;
        else if (arg == "--verbose" || arg == "-v")
            options.Verbose = true;
        else if (arg == "--log" && i + 1 < args.Length)
        {
            options.LogFile = args[i + 1];
            i++; 
        }
        else if (!arg.StartsWith("-") && string.IsNullOrEmpty(options.TargetPath))
            options.TargetPath = arg;
    }
    
    // If help was requested, return early
    if (options.GetHelp)
    {
        options.IsValid = false;
        return options;
    }
    
    // Default to current directory if not provided
    if (string.IsNullOrEmpty(options.TargetPath))
        options.TargetPath = Directory.GetCurrentDirectory();
    
    // Normalize and validate
    options.TargetPath = Path.GetFullPath(options.TargetPath);
    options.IsValid = Directory.Exists(options.TargetPath);
    
    return options;
}

    public static void PrintHelp()
    {
        Console.WriteLine("DirectoryCleaner - Organize your files automatically");
        Console.WriteLine();
        Console.WriteLine("Usage: DirectoryCleaner [path] [options]");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        Console.WriteLine("  path              Directory to clean (default: current directory)");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --dry-run         Preview changes without moving files");
        Console.WriteLine("  --recursive       Scan subdirectories");
        Console.WriteLine("  --verbose, -v     Show detailed information");
        Console.WriteLine("  -h, --help        Show this help message");
    }
}
