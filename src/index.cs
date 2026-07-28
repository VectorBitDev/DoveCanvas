/**
    * @file index.cs
    * @brief The main entry point for the DoveCanvas framework.
    * @details This file contains the main application class and its methods for initializing, running, and shutting down the application.
*/


using Raylib_cs;
using rlImGui_cs;
using DoveCanvas.Abstract;
using DoveCanvas.Ui;

namespace DoveCanvas;

/**
    * @class DoveCanvasApplication
    * @brief The main application class for the DoveCanvas framework.
*/
public class DoveCanvasApplication
{
    private Scene? defaultScene;

    private SchedularService schedular = Services.Schedular;
    private ProfilerService profiler = Services.Profiler;
    private CameraService camera = Services.Camera;
    private UiService ui = Services.Ui;

    /**
        * @brief Initializes the DoveCanvas application.
        * @return The instance of the DoveCanvas application.
    */
    public DoveCanvasApplication Initialize(WindowConfiguration settings)
    {
        if (defaultScene == null)
        {
            throw new Exception("Default scene is not set. Please set a default scene before initializing the application.");
        }

        Services.Window.Initialize(settings);

        Services.Schedular.Initialize();
        Services.Resource.Initialize();
        Services.Ui.Initialize();

        Services.Scene.Initialize(defaultScene);

        return this;
    }

    /**
        * @brief Shuts down the DoveCanvas application.
        * @param onShutdown A callback function to be executed on application shutdown.
    */
    private void Shutdown(Action onShutdown)
    {
        onShutdown?.Invoke();
        Services.Window.ShutdownWindow();
    }

    /**
        * @brief Begins a new frame for rendering.
        * @returns void
    */
    private void BeginFrame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        rlImGui.Begin();
    }

    /**
        * @brief Ends the current frame.
        * @returns void
    */
    internal void EndFrame()
    {
        rlImGui.End();
        Raylib.EndDrawing();
    }

    /**
        * @brief Sets the default scene for the DoveCanvas application.
        * @param scene The scene to be set as the default scene.
    */
    public DoveCanvasApplication SetDefaultScene(Scene scene)
    {
        defaultScene = scene;
        return this;
    }

    /**
        * @brief Runs the DoveCanvas application.
        * @param onStartup A callback function to be executed on application startup.
        * @param onShutdown A callback function to be executed on application shutdown.
    */
    public void Run(Action onStartup, Action onShutdown, Action onFatal)
    {
        onStartup?.Invoke();

        while (Services.Window.IsApplicationRunning())
        {
            profiler.Begin("Frame", "Total Frame");
            try
            {
                schedular.Tick();

                this.BeginFrame();
                // camera.BeginCamera();
                schedular.Draw();
                // camera.EndCamera();

                ui.Update();
                ui.Draw();

                schedular.DebugDraw();
                this.EndFrame();
            }
            catch (Exception e)
            {
                Logger.Error($"Fatal error occurred: {e.Message}\n{e.StackTrace}");
                Logger.Error("Shutting down application due to fatal error.");
                onFatal?.Invoke();
                Shutdown(onShutdown);
                throw e;
            }
            profiler.End("Frame", "Total Frame");
        }

        Shutdown(onShutdown);
    }
}
