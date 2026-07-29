using System.Numerics;
using Raylib_cs;

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


public abstract class Scene3d : Scene
{
    private Entity cameraEntity;

    /**
     * @brief Gets the camera entity associated with the scene.
     * @return The camera entity.
     */
    protected Entity GetCameraEntity()
    {
        return this.cameraEntity;
    }

    /**
     * @brief Loads the scene.
     */
    public override void Load()
    {
        Logger.Info("Loading 3D Scene");
        this.cameraEntity = Services.World.CreateEntity();
        Services.World.AddComponent(this.cameraEntity, new Transform3dComponent()
        {
            Position = new Vector3(0, 2, 50),
            Scale = new Vector3(1, 1, 1)
        });
        Services.World.AddComponent(this.cameraEntity, new Camera3dComponent());
        Services.World.AddComponent(this.cameraEntity, new DontSaveComponent());

        Logger.Info("Registering Camera Entity");
        Services.Camera.RegisterCameraEntity(this.cameraEntity, CameraType.Camera3D);
    }

    /**
     * @brief Unloads the scene.
     */
    public override void Unload()
    {
        Logger.Info("Unregistering Camera Entity");
        Services.Camera.UnRegisterCameraEntity();
        Services.World.DestroyEntity(this.cameraEntity);
    }

    /**
     * @brief Updates the scene.
     * @param dt The time delta since the last update.
     */
    public override void Tick(float dt) { }

    /**
     * @brief Draws the scene.
     */
    public override void Draw() { }
}


public abstract class Scene2d : Scene
{
    private Entity cameraEntity;

    /**
     * @brief Gets the camera entity associated with the scene.
     * @return The camera entity.
     */
    protected Entity GetCameraEntity()
    {
        return this.cameraEntity;
    }

    /**
     * @brief Loads the scene.
     */
    public override void Load()
    {
        this.cameraEntity = Services.World.CreateEntity();
        Services.World.AddComponent(this.cameraEntity, new Transform2dComponent());
        Services.World.AddComponent(this.cameraEntity, new Camera2dComponent());

        Services.Camera.RegisterCameraEntity(this.cameraEntity, CameraType.Camera2D);
    }

    /**
     * @brief Unloads the scene.
     */
    public override void Unload()
    {
        Services.Camera.UnRegisterCameraEntity();
        Services.World.DestroyEntity(this.cameraEntity);
    }

    /**
     * @brief Updates the scene.
     * @param dt The time delta since the last update.
     */
    public override void Tick(float dt) { }

    /**
     * @brief Draws the scene.
     */
    public override void Draw() { }
}
