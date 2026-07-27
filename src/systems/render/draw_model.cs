using DoveCanvas.Abstract;
using System.Numerics;
using Raylib_cs;

namespace DoveCanvas;

/**
    * @class ModelRenderComponent
    * @brief A component that represents a 3D model to be rendered in the scene.
    * @details This component holds information about the model and texture for rendering.
    * @property {Model} model - The 3D model to render.
    * @property {Texture2D} texture - The texture to apply to the model.
*/
public class ModelRenderComponent : Component
{
    public Model model;
    public Texture2D texture;
}

/**
    * @class DrawModelSystem
    * @brief A system that draws 3D models in the scene.
    * @details This system fetches all entities with a Transform3dComponent and a ModelRenderComponent, and draws the corresponding models in the 3D space.
*/
public class DrawModelSystem : IRenderSystem
{
    /**
        * @brief A query that fetches all entities with a Transform3dComponent and a ModelRenderComponent.
    */
    private WorldQuery query = Services.World.Query()
        .With<Transform3dComponent>()
        .With<ModelRenderComponent>();

    /**
        * @brief Draws all 3D models in the scene.
        * @returns void
    */
    public override void Draw()
    {
        foreach (var entity in query.Fetch())
        {
            var ModelComponent = Services.World.GetComponent<ModelRenderComponent>(entity);
            var TransformComponent = Services.World.GetComponent<Transform3dComponent>(entity);

            Raylib.DrawModel(ModelComponent.model, TransformComponent.Position, TransformComponent.scale.X, Color.White);
        }
    }
}
