using DoveCanvas.Abstract;

namespace DoveCanvas;

/**
    * @class SceneService
    * @brief A service that provides functionality.
*/
public class SceneService : Singleton<SceneService>
{
    private Scene? currentScene;

    /**
        * @brief Initializes the service.
        * @return The instance of the service.
    */
    public void Initialize(Scene DefaultScene)
    {
        currentScene = DefaultScene;
        currentScene.Load();
    }

    /**
        * @brief Changes the current scene to a new scene.
        * @param newScene The new scene to switch to.
        * @param SceneTransition Whether to use a scene transition effect (default: false).
    */
    public void ChangeScene(Scene newScene, bool SceneTransition = false)
    {
        if (SceneTransition)
        {
            // Implement scene transition logic here
            // For example, you could fade out the current scene and fade in the new scene
        }
        else
        {
            currentScene?.Unload();
            Services.World.Clear();
        }
        currentScene = newScene;
        currentScene.Load();
    }

    /**
        * @brief Unloads the current scene and clears the world.
        * @returns void
    */
    internal void UnloadCurrentScene()
    {
        currentScene?.Unload();
        Services.World.Clear();
        currentScene = null;
    }

    /**
        * @brief Updates the current scene.
        * @param dt The time elapsed since the last frame.
        * @returns void
    */
    internal void Tick(float dt)
    {
        currentScene?.Tick(dt);
    }

    /**
        * @brief Draws the current scene.
        * @returns void
    */
    internal void Draw()
    {
        currentScene?.Draw();
    }
}
