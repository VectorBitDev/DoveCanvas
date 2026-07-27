using DoveCanvas.Abstract;
using System.Numerics;
using Raylib_cs;

namespace DoveCanvas;

/**
    * @class SpriteRenderComponent
    * @brief A component that represents a sprite to be rendered in the scene.
    * @details This component holds information about the texture and color for rendering.
    * @property {Texture2D} texture - The texture of the sprite to render.
    * @property {Color} color - The color of the sprite to render.
*/
public class SpriteRenderComponent : Component
{
    public Texture2D texture;
    public Color color = Color.White;
}


/**
    * @class DrawSpriteSystem
    * @brief A system that draws sprites in the scene.
    * @details This system fetches all entities with a Transform2dComponent and a SpriteRenderComponent, and draws the corresponding sprites on the screen.
*/
public class DrawSpriteSystem : IRenderSystem
{
    /**
        * @brief A query that fetches all entities with a Transform2dComponent and a SpriteRenderComponent.
    */
    private WorldQuery query2d = Services.World.Query()
        .With<Transform2dComponent>()
        .With<SpriteRenderComponent>();

    private WorldQuery query3d = Services.World.Query()
        .With<Transform3dComponent>()
        .With<SpriteRenderComponent>();

    /**
        * @brief Draws all sprites in the scene.
        * @returns void
    */
    public override void Draw()
    {
        foreach (var entity in query2d.Fetch())
        {
            var SpriteComponent = Services.World.GetComponent<SpriteRenderComponent>(entity);
            var TransformComponent = Services.World.GetComponent<Transform2dComponent>(entity);

            Raylib.DrawTextureV(SpriteComponent.texture, TransformComponent.Position, SpriteComponent.color);
        }

        foreach (var entity in query3d.Fetch())
        {
            var SpriteComponent = Services.World.GetComponent<SpriteRenderComponent>(entity);
            var TransformComponent = Services.World.GetComponent<Transform3dComponent>(entity);

            // Convert 3D position to 2D screen position
            Vector2? screenPosition = Services.Camera.WorldToScreen(TransformComponent.Position);
            if (!screenPosition.HasValue)
                continue;

            Raylib.DrawTextureV(SpriteComponent.texture, screenPosition.Value, SpriteComponent.color);
        }
    }
}
