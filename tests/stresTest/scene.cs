using DoveCanvas.Abstract;
using DoveCanvas;
using System.Numerics;
using Raylib_cs;

namespace TestDoveCanvas;

/**
    * @class TestScene
    * @brief A test scene for the DoveCanvas framework.
*/
public class StressTestScene : Scene
{
    private Vector2 getRandomPositionInScreen(Random rng)
    {
        var screenResolution = Services.Window.GetWindowResolution();
        return new Vector2(rng.Next(0, (int)screenResolution.X), rng.Next(0, (int)screenResolution.Y));
    }

    /**
        * @brief Loads the resources and initializes the scene.
    */
    public override void Load()
    {
        Random rng = new Random();

        // Logger.Info("TestScene loaded.");
        for (int i = 0; i < 5000; i++)
        {
            Entity entity = Services.World.CreateEntity();
            Services.World.AddComponent(entity, new TransformComponent
            {

                Position = getRandomPositionInScreen(rng),
                Color = new Color(rng.Next(0, 256), rng.Next(0, 256), rng.Next(0, 256), 255)
            });
            Services.World.AddComponent(entity, new VelocityComponent { Velocity = new Vector2(rng.Next(0, 100), rng.Next(0, 100)) });
            Services.World.AddComponent(entity, new MovementComponent());
        }
    }

    /**
        * @brief Unloads the resources and cleans up the scene.
    */
    public override void Unload()
    {
    }

    /**
        * @brief Updates the scene logic.
        * @param dt The time elapsed since the last frame.
    */
    public override void Tick(float dt)
    {
    }

    /**
        * @brief Draws the scene objects.
    */
    public override void Draw()
    {
    }
}
