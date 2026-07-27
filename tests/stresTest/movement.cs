using System.Numerics;
using Raylib_cs;
using DoveCanvas;
using DoveCanvas.Abstract;

namespace TestDoveCanvas;

/**
    * @class MovementComponent
    * @brief A component that represents the movement of an entity in 2D space.
*/
public class MovementComponent : Component { }

/**
    * @class VelocityComponent
    * @brief A component that represents the velocity of an entity in 2D space.
*/
public class VelocityComponent : Component
{
    public Vector2 Velocity;
}

/**
    * @class TransformComponent
    * @brief A component that represents the position of an entity in 2D space.
*/
public class TransformComponent : Component
{
    public Vector2 Position;
    public Color Color;
}

/**
    * @class MovementSystem
    * @brief A system that moves entities based on their velocity.
*/
public class MovementSystem : ISimulationSystem
{
    private readonly WorldQuery query = Services.World
        .Query()
        .With<MovementComponent>()
        .With<TransformComponent>()
        .With<VelocityComponent>();

    /**
        * @brief Updates entity positions using velocity.
        * @param dt The time elapsed since the last frame.
    */
    public override void Tick(float dt)
    {
        foreach (Entity entity in query.Fetch())
        {
            TransformComponent transform = Services.World.GetComponent<TransformComponent>(entity);
            VelocityComponent velocity = Services.World.GetComponent<VelocityComponent>(entity);

            transform.Position += velocity.Velocity * dt;
        }
    }
}

/**
    * @class RandomMovementSystem
    * @brief A system that randomly changes entity movement direction and keeps entities inside the screen.
*/
public class RandomMovementSystem : ISimulationSystem
{
    private readonly WorldQuery query = Services.World
        .Query()
        .With<TransformComponent>()
        .With<VelocityComponent>();

    private readonly Dictionary<Entity, float> timers = new();
    private readonly Random random = new();

    private int ScreenWidth = 1920;
    private int ScreenHeight = 1080;
    private const int EntitySize = 50;

    /**
        * @brief Changes entity velocity randomly and keeps it inside screen bounds.
        * @param dt The time elapsed since the last frame.
    */
    public override void Tick(float dt)
    {
        var screenResolution = Services.Window.GetWindowResolution();
        ScreenWidth = (int)screenResolution.X;
        ScreenHeight = (int)screenResolution.Y;

        foreach (Entity entity in query.Fetch())
        {
            TransformComponent transform = Services.World.GetComponent<TransformComponent>(entity);
            VelocityComponent velocity = Services.World.GetComponent<VelocityComponent>(entity);

            if (!timers.ContainsKey(entity))
            {
                timers[entity] = 0;
            }

            timers[entity] -= dt;

            if (timers[entity] <= 0)
            {
                float angle = (float)(random.NextDouble() * Math.PI * 2);
                float speed = 100f;

                velocity.Velocity = new Vector2(
                    MathF.Cos(angle) * speed,
                    MathF.Sin(angle) * speed
                );

                timers[entity] = 1f + (float)random.NextDouble() * 2f;
            }

            if (transform.Position.X <= 0 || transform.Position.X >= ScreenWidth - EntitySize)
            {
                velocity.Velocity.X *= -1;
            }

            if (transform.Position.Y <= 0 || transform.Position.Y >= ScreenHeight - EntitySize)
            {
                velocity.Velocity.Y *= -1;
            }

            transform.Position.X = Math.Clamp(transform.Position.X, 0, 1870);
            transform.Position.Y = Math.Clamp(transform.Position.Y, 0, 1030);
        }
    }
}

/**
    * @class MovementRenderSystem
    * @brief A system that renders entities with a TransformComponent.
*/
public class MovementRenderSystem : IRenderSystem
{
    private readonly WorldQuery query = Services.World
        .Query()
        .With<TransformComponent>();


    /**
        * @brief Draws the entities as circles at their positions.
    */
    public override void Draw()
    {
        foreach (Entity entity in query.Fetch())
        {
            TransformComponent transform = Services.World.GetComponent<TransformComponent>(entity);
            Raylib.DrawRectangle((int)transform.Position.X, (int)transform.Position.Y, 50, 50, transform.Color);
        }
    }
}
