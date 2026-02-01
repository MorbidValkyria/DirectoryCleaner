using DirectoryCleaner.Arguments;
using DirectoryCleaner.Core;

var options = Options.Parse(args);

if (options.GetHelp || !options.IsValid)
{
    Options.PrintHelp();
    return options.IsValid ? 0 : 1;
}

var cleaner = new Cleaner(options);
cleaner.Run();

return 0;



// logger.Info($"Using target directory: {options.TargetPath}");




/*
DirectoryCleaner/
├── DirectoryCleaner.sln
├── DirectoryCleaner.csproj
├── Program.cs
├── Arguments/
│   └── Options.cs
├── Core/
│   ├── Cleaner.cs
│   ├── RuleEngine.cs
│   └── FileAction.cs
├── Services/
│   ├── FileScanner.cs
│   └── FileMover.cs
├── Utils/
│   ├── Logger.cs
│   └── PathHelper.cs
└── Models/
    └── FileItem.cs
└── README.md
*/