namespace DirectoryCleaner.Utils;

/// <summary>
/// Provides centralized logging functionality with console output and optional file logging.
/// Supports different log levels with color-coded console output.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Defines the severity level of log messages.
    /// </summary>
    public enum LogLevel
    {
        Info,
        Success,
        Warning,
        Error
    }
    
    private static string? _logFilePath;
    private static bool _enableFileLogging = false;
    
    /// <summary>
    /// Enable logging to a file.
    /// </summary>
    /// <param name="logFilePath">Path to the log file. Will be created if it doesn't exist.</param>
    public static void EnableFileLogging(string logFilePath)
    {
        _logFilePath = logFilePath;
        _enableFileLogging = true;
        
        // Create log file with header
        try
        {
            File.WriteAllText(_logFilePath, $"=== DirectoryCleaner Log - {DateTime.Now} ==={Environment.NewLine}{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not create log file: {ex.Message}");
            _enableFileLogging = false;
        }
    }
    
    /// <summary>
    /// Disable file logging.
    /// </summary>
    public static void DisableFileLogging()
    {
        _enableFileLogging = false;
        _logFilePath = null;
    }
    
    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Info(string message)
    {
        Log(message, LogLevel.Info);
    }
    
    /// <summary>
    /// Logs a success message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Success(string message)
    {
        Log(message, LogLevel.Success);
    }
    
    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Warning(string message)
    {
        Log(message, LogLevel.Warning);
    }
    
    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Error(string message)
    {
        Log(message, LogLevel.Error);
    }
    
    /// <summary>
    /// Internal logging method that handles both console and file output.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="level">The severity level of the message.</param>
    private static void Log(string message, LogLevel level)
    {
        // Console logging (with colors)
        LogToConsole(message, level);
        
        // File logging (without colors)
        if (_enableFileLogging && !string.IsNullOrEmpty(_logFilePath))
        {
            LogToFile(message, level);
        }
    }
    
    /// <summary>
    /// Logs a message to the console with color-coded output.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="level">The severity level determining the color.</param>
    private static void LogToConsole(string message, LogLevel level)
    {
        // Save current console color
        var originalColor = Console.ForegroundColor;
        
        // Set color based on log level
        Console.ForegroundColor = level switch
        {
            LogLevel.Info => ConsoleColor.Cyan,
            LogLevel.Success => ConsoleColor.Green,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
        
        // Print prefix
        string prefix = level switch
        {
            LogLevel.Info => "[INFO]",
            LogLevel.Success => "[✓]",
            LogLevel.Warning => "[!]",
            LogLevel.Error => "[✗]",
            _ => "[LOG]"
        };
        
        Console.Write(prefix + " ");
        
        // Reset color for message
        Console.ForegroundColor = originalColor;
        Console.WriteLine(message);
    }
    
    /// <summary>
    /// Logs a message to the configured log file with timestamp.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="level">The severity level of the message.</param>
  private static void LogToFile(string message, LogLevel level)
{
    try
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string prefix = level switch
        {
            LogLevel.Info => "[INFO]",
            LogLevel.Success => "[SUCCESS]",
            LogLevel.Warning => "[WARNING]",
            LogLevel.Error => "[ERROR]",
            _ => "[LOG]"
        };
        
        string logLine = $"[{timestamp}] {prefix} {message}{Environment.NewLine}";
        File.AppendAllText(_logFilePath!, logLine);  // Add ! here
    }
    catch
    {
        // Silently fail if we can't write to log file
    }
}
    
    /// <summary>
    /// Prints a separator line to console and log file.
    /// </summary>
    public static void Separator()
    {
        Console.WriteLine(new string('-', 60));
        
        if (_enableFileLogging && !string.IsNullOrEmpty(_logFilePath))
        {
            try
            {
                File.AppendAllText(_logFilePath, new string('-', 60) + Environment.NewLine);
            }
            catch { }
        }
    }
    
    /// <summary>
    /// Prints an empty line to console and log file.
    /// </summary>
    public static void EmptyLine()
    {
        Console.WriteLine();
        
        if (_enableFileLogging && !string.IsNullOrEmpty(_logFilePath))
        {
            try
            {
                File.AppendAllText(_logFilePath, Environment.NewLine);
            }
            catch { }
        }
    }
}