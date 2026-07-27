namespace DoveCanvas.Abstract;

/**
 * @class Scene
 * @brief Represents a scene in the DoveCanvas framework.
 */
public abstract class Scene
{
    /**
     * @brief Loads the scene.
     */
    public abstract void Load();

    /**
     * @brief Unloads the scene.
     */
    public abstract void Unload();

    /**
     * @brief Updates the scene.
     * @param dt The time delta since the last update.
     */
    public virtual void Tick(float dt) { }

    /**
     * @brief Draws the scene.
     */
    public virtual void Draw() { }
}
