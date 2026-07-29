using System.Diagnostics;
using DoveCanvas.Ui;

namespace DoveCanvas;


/**
    * Services class is a static class that holds references to all the services in the engine.
    * This allows for easy access to services from anywhere in the engine.
*/
public static class Services
{
    public static SchedularService Schedular = SchedularService.Instance;
    public static ProfilerService Profiler = ProfilerService.Instance;
    public static ResourceService Resource = ResourceService.Instance;
    public static CameraService Camera = CameraService.Instance;
    public static WindowService Window = WindowService.Instance;
    public static WorldService World = WorldService.Instance;
    public static AudioService Audio = AudioService.Instance;
    public static SceneService Scene = SceneService.Instance;
    public static MapService Map = MapService.Instance;
    public static UiService Ui = UiService.Instance;
}


/**
    * Logger class is a static class that provides logging functionality for the engine.
    * This allows for easy logging from anywhere in the engine.
*/
public static class Logger
{
    private static readonly object lockObject = new();

    /**
        * @brief Logs a trace message.
        * @param message The message to log.
    */
    public static void Trace(string message)
    {
        Write("TRACE", ConsoleColor.DarkGray, message);
    }

    /**
        * @brief Logs an information message.
        * @param message The message to log.
    */
    public static void Info(string message)
    {
        Write("INFO", ConsoleColor.Cyan, message);
    }

    /**
        * @brief Logs a success message.
        * @param message The message to log.
    */
    public static void Success(string message)
    {
        Write("SUCCESS", ConsoleColor.Green, message);
    }

    /**
        * @brief Logs a warning message.
        * @param message The message to log.
    */
    public static void Warning(string message)
    {
        Write("WARNING", ConsoleColor.Yellow, message);
    }

    /**
        * @brief Logs an error message.
        * @param message The message to log.
    */
    public static void Error(string message)
    {
        Write("ERROR", ConsoleColor.Red, message);
    }

    /**
        * @brief Logs a fatal error message.
        * @param message The message to log.
    */
    public static void Fatal(string message)
    {
        Write("FATAL", ConsoleColor.DarkRed, message);
    }

    /**
        * @brief Writes a formatted log message.
        * @param level The log level.
        * @param color The color of the log level.
        * @param message The message to log.
    */
    private static void Write(string level, ConsoleColor color, string message)
    {
        StackFrame? frame = GetCallerFrame();

        string file = "Unknown";
        int line = 0;
        string method = "Unknown";

        if (frame != null)
        {
            method = frame.GetMethod()?.DeclaringType?.FullName + "." + frame.GetMethod()?.Name;
            line = frame.GetFileLineNumber();

            string? filePath = frame.GetFileName();

            if (!string.IsNullOrEmpty(filePath))
            {
                file = Path.GetFileName(filePath);
            }
        }

        lock (lockObject)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"[{DateTime.Now:HH:mm:ss.fff}] ");

            Console.ForegroundColor = color;
            Console.Write($"[{level}] ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message);

            Console.ResetColor();
            Console.WriteLine();
        }
    }

    /**
        * @brief Finds the first stack frame outside the logger.
        * @return The caller stack frame.
    */
    private static StackFrame? GetCallerFrame()
    {
        StackTrace trace = new(true);

        foreach (StackFrame frame in trace.GetFrames() ?? [])
        {
            var method = frame.GetMethod();

            if (method == null)
            {
                continue;
            }

            if (method.DeclaringType == typeof(Logger))
            {
                continue;
            }

            return frame;
        }

        return null;
    }
}
