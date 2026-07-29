using DoveCanvas;

namespace DoveCanvas.Tests.TestDoveCanvas;

/**
    * @class Program
    * @brief The main entry point for the application.
*/
public class Program
{
    /**
        * @brief The main entry point for the application.
        * @param args Command line arguments.
    */
    public static void Main(string[] args)
    {
        var WindowConfig = new WindowConfiguration()
        {
            Title = "Stress Test Application",
        };


        var app = new DoveCanvasApplication();
        app.SetDefaultScene(new StressTestScene());
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
