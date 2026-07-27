using System.Runtime.InteropServices;
using System.Numerics;
using rlImGui_cs;
using Raylib_cs;

namespace DoveCanvas;


/**
    * @struct WindowConfiguration

    * @brief A structure that holds configuration settings for the window.
*/
public struct WindowConfiguration()
{
    public string Title = "DoveCanvas Application";

    public Vector2 Resolution = new Vector2(1920, 1080);

    public bool IsResizable = true;
    public bool IsFullscreen = false;

    public bool IsVSyncEnabled = false;
    public int TargetFPS = 60;
}


/**
    * @class WindowService
    * @brief A service class that manages the window and its settings.
    * This class is a singleton, meaning there will only be one instance of it throughout the application.
*/
[Service]
public class WindowService : Singleton<WindowService>
{
    private bool IsGameRunning = true;

    /**
        * @brief Initializes the window service.
        * @return The instance of the window service.
    */
    internal void Initialize(WindowConfiguration settings)
    {
        if (settings.IsResizable)
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

        Raylib.SetTraceLogLevel(TraceLogLevel.Fatal);
        Raylib.InitWindow((int)settings.Resolution.X, (int)settings.Resolution.Y, settings.Title);
        EnableDarkTitleBar();
        rlImGui.Setup(true);

        if (settings.IsVSyncEnabled)
            Raylib.SetTargetFPS(settings.TargetFPS);

        if (settings.IsFullscreen)
            Raylib.ToggleFullscreen();
    }

    /**
        * @brief Shuts down the window service.
        * @returns void
    */
    internal void ShutdownWindow()
    {
        Raylib.CloseWindow();
    }

    /**
        * @brief Checks if the application is still running.
        * @returns A boolean indicating whether the application is running or not.
    */
    internal bool IsApplicationRunning()
    {
        return !Raylib.WindowShouldClose() && IsGameRunning;
    }

    /**
        * @brief Gets the handle of the active window.
        * @returns The handle of the active window.
    */
    [DllImport("user32.dll")]
    extern static IntPtr GetActiveWindow();

    /**
        * @brief Sets a window attribute.
        * @param hwnd The handle to the window.
        * @param attr The attribute to set.
        * @param attrValue The value of the attribute.
        * @param attrSize The size of the attribute value.
        * @returns An integer indicating success or failure.
    */
    [DllImport("dwmapi.dll")]
    extern static int DwmSetWindowAttribute(
        IntPtr hwnd,
        int attr,
        ref int attrValue,
        int attrSize
    );

    /**
        * @brief Enables the dark title bar for the active window.
        * @returns void
     */
    internal static void EnableDarkTitleBar()
    {
        IntPtr hwnd = GetActiveWindow();
        int dark = 1;

        DwmSetWindowAttribute(hwnd, 20, ref dark, sizeof(int));
    }

    /**
        * @brief Shuts down the game.
        * @returns void
    */
    public void Shutdown()
    {
        IsGameRunning = false;
    }

    /**
        * @brief Sets the window title.
        * @param title The new title for the window.
        * @returns void
    */
    public void SetWindowTitle(string title)
    {
        Raylib.SetWindowTitle(title);
    }

    /**
        * @brief Changes the window resolution.
        * @param resolution The new resolution for the window.
        * @returns void
    */
    public void ChangeWindowResolution(Vector2 resolution)
    {
        Raylib.SetWindowSize((int)resolution.X, (int)resolution.Y);
    }

    /**
        * @brief Gets the current window resolution.
        * @returns A Vector2 representing the current window resolution.
    */
    public Vector2 GetWindowResolution()
    {
        return new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    }

    /**
        * @brief Toggles the fullscreen mode of the window.
        * @returns void
    */
    public void ToggleFullscreen()
    {
        Raylib.ToggleFullscreen();
    }

    /**
        * @brief Sets the VSync state of the window.
        * @param enabled A boolean indicating whether VSync should be enabled or disabled.
        * @returns void
    */
    public void SetVSync(bool enabled)
    {
        if (enabled)
            Raylib.SetTargetFPS(60);
        else
            Raylib.SetTargetFPS(0);
    }
}
