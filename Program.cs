using DirectoryCleaner.Arguments;
using DirectoryCleaner.Core;
using DirectoryCleaner.Utils;

var options = Options.Parse(args);

if (options.GetHelp || !options.IsValid)
{
    Options.PrintHelp();
    return options.IsValid ? 0 : 1;
}

var logger = new Logger();
if (options.LogFile != null)
    logger.EnableFileLogging(options.LogFile);

var cleaner = new Cleaner(options, logger);


return 0;



// logger.Info($"Using target directory: {options.TargetPath}");

