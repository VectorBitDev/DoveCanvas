using DoveCanvas;
using Raylib_cs;

namespace DoveCanvas.Tests.Canvas3d;

/**
    * @brief The main entry point for the application.
    * @param args The command-line arguments.
*/
public class Program
{
    public static void Main(string[] args)
    {
        var WindowConfig = new WindowConfiguration()
        {
            Title = "Stress Test Application",
        };


        var app = new DoveCanvasApplication();
        app.SetDefaultScene(new Canvas3dScene());
        app.SetBackgroundColor(Color.SkyBlue);
        app.Initialize(WindowConfig);
        app.Run(() =>
        {
            Logger.Info("Application is running.");
        },
        () =>
        {
            Logger.Info("Application is shutting down.");
        },
        () =>
        {
            Logger.Info("Appliaction Crashed!.");
        });
    }
}
