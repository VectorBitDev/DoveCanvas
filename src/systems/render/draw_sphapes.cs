using DoveCanvas.Abstract;
using System.Numerics;
using Raylib_cs;

namespace DoveCanvas;

/**
    * @enum ShapeType
    * @brief An enumeration of shape types that can be rendered.
    * @details This enum defines the different types of shapes that can be drawn in the scene.
    * @value Circle - Represents a circle shape.
    * @value Triangle - Represents a triangle shape.
    * @value Rectangle - Represents a rectangle shape.
*/
public enum Shape2dType
{
    Circle,
    Triangle,
    Rectangle,
}

public enum Shape3dType
{
    Sphere,
    Triangle,
    Cube,
}

/**
    * @class ShapeRenderComponent
    * @brief A component that represents a shape to be rendered in the scene.
    * @details This component holds information about the shape type and color for rendering.
    * @property {ShapeType} shape - The type of shape to render (Circle, Triangle, Rectangle).
    * @property {Color} color - The color of the shape to render.
*/
public class Shape2dRenderComponent : Component
{
    public Shape2dType shape;
    public Color color;
}

public class Shaped3dRenderComponent : Component
{
    public Shape3dType shape;
    public Color color;
}

/**
    * @class DrawShapesSystem
    * @brief A system that draws shapes in the scene.
*/
public class DrawShapesSystem : IRenderSystem
{
    private WorldQuery query2d = Services.World.Query()
        .With<Transform2dComponent>()
        .With<Shape2dRenderComponent>();

    private WorldQuery query3d = Services.World.Query()
        .With<Transform3dComponent>()
        .With<Shaped3dRenderComponent>();

    /**
        * @brief Draws all shapes in the scene.
        * @returns void
    */
    public override void Draw()
    {
        // Draw 2D shapes
        foreach (var entity in query2d.Fetch())
        {
            var ShapeComponent = Services.World.GetComponent<Shape2dRenderComponent>(entity);
            var TransformComponent = Services.World.GetComponent<Transform2dComponent>(entity);

            switch (ShapeComponent.shape)
            {
                case Shape2dType.Circle:
                    Raylib.DrawCircleV(TransformComponent.Position, TransformComponent.scale.X, ShapeComponent.color);
                    break;
                case Shape2dType.Triangle:
                    Vector2 p1 = TransformComponent.Position;
                    Vector2 p2 = TransformComponent.Position + new Vector2(TransformComponent.scale.X, 0);
                    Vector2 p3 = TransformComponent.Position + new Vector2(TransformComponent.scale.X / 2, TransformComponent.scale.Y);
                    Raylib.DrawTriangle(p1, p2, p3, ShapeComponent.color);
                    break;
                case Shape2dType.Rectangle:
                    Raylib.DrawRectangleV(TransformComponent.Position, TransformComponent.scale, ShapeComponent.color);
                    break;
            }
        }

        // Draw 3D shapes
        foreach (var entity in query3d.Fetch())
        {
            var ShapeComponent = Services.World.GetComponent<Shaped3dRenderComponent>(entity);
            var TransformComponent = Services.World.GetComponent<Transform3dComponent>(entity);

            switch (ShapeComponent.shape)
            {
                case Shape3dType.Sphere:
                    Raylib.DrawSphere(TransformComponent.Position, TransformComponent.scale.X, ShapeComponent.color);
                    break;
                case Shape3dType.Triangle:
                    Vector3 p1 = TransformComponent.Position;
                    Vector3 p2 = TransformComponent.Position + new Vector3(TransformComponent.scale.X, 0, 0);
                    Vector3 p3 = TransformComponent.Position + new Vector3(TransformComponent.scale.X / 2, TransformComponent.scale.Y, 0);
                    Raylib.DrawTriangle3D(p1, p2, p3, ShapeComponent.color);
                    break;
                case Shape3dType.Cube:
                    Raylib.DrawCube(TransformComponent.Position, TransformComponent.scale.X, TransformComponent.scale.Y, TransformComponent.scale.Z, ShapeComponent.color);
                    break;
            }
        }
    }
}
