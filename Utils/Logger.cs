namespace DirectoryCleaner.Utils;

/// <summary>
/// Provides centralized logging functionality with console output and optional file logging.
/// Supports different log levels with color-coded console output.
/// </summary>
public class Logger
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

    private string? _logFilePath;
    private bool _enableFileLogging = false;

    /// <summary>
    /// Enable logging to a file.
    /// </summary>
    /// <param name="logFilePath">Path to the log file. Will be created if it doesn't exist.</param>
    public void EnableFileLogging(string logFilePath)
    {
        _logFilePath = logFilePath;
        _enableFileLogging = true;

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
    public void DisableFileLogging()
    {
        _enableFileLogging = false;
        _logFilePath = null;
    }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    public void Info(string message) => Log(message, LogLevel.Info);

    /// <summary>
    /// Logs a success message.
    /// </summary>
    public void Success(string message) => Log(message, LogLevel.Success);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    public void Warning(string message) => Log(message, LogLevel.Warning);

    /// <summary>
    /// Logs an error message.
    /// </summary>
    public void Error(string message) => Log(message, LogLevel.Error);

    /// <summary>
    /// Internal logging method that handles both console and file output.
    /// </summary>
    private void Log(string message, LogLevel level)
    {
        LogToConsole(message, level);

        if (_enableFileLogging && !string.IsNullOrEmpty(_logFilePath))
            LogToFile(message, level);
    }

    /// <summary>
    /// Logs a message to the console with color-coded output.
    /// </summary>
    private void LogToConsole(string message, LogLevel level)
    {
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = level switch
        {
            LogLevel.Info => ConsoleColor.Cyan,
            LogLevel.Success => ConsoleColor.Green,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.White
        };

        string prefix = level switch
        {
            LogLevel.Info => "[INFO]",
            LogLevel.Success => "[✓]",
            LogLevel.Warning => "[!]",
            LogLevel.Error => "[✗]",
            _ => "[LOG]"
        };

        Console.Write(prefix + " ");
        Console.ForegroundColor = originalColor;
        Console.WriteLine(message);
    }

    /// <summary>
    /// Logs a message to the configured log file with timestamp.
    /// </summary>
    private void LogToFile(string message, LogLevel level)
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
            File.AppendAllText(_logFilePath!, logLine);
        }
        catch
        {
            // Silently fail if we can't write to log file
        }
    }

    /// <summary>
    /// Prints a separator line to console and log file.
    /// </summary>
    public void Separator()
    {
        Console.WriteLine(new string('-', 60));

        if (_enableFileLogging && !string.IsNullOrEmpty(_logFilePath))
        {
            try { File.AppendAllText(_logFilePath, new string('-', 60) + Environment.NewLine); }
            catch { }
        }
    }

    /// <summary>
    /// Prints an empty line to console and log file.
    /// </summary>
    public void EmptyLine()
    {
        Console.WriteLine();

        if (_enableFileLogging && !string.IsNullOrEmpty(_logFilePath))
        {
            try { File.AppendAllText(_logFilePath, Environment.NewLine); }
            catch { }
        }
    }
}