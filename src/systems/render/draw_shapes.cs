using DoveCanvas.Abstract;
using System.Numerics;
using Raylib_cs;

namespace DoveCanvas;

/**
    * @enum Shape2dType
    * @brief An enumeration of 2D shape types that can be rendered.
    * @details This enum defines the different types of 2D shapes that can be drawn in the scene.
    * @value Circle - Represents a circle shape.
    * @value Rectangle - Represents a rectangle shape.
    * @value Triangle - Represents a triangle shape.
    * @value Ellipse - Represents an ellipse shape.
    * @value Ring - Represents a ring shape.
    * @value Polygon - Represents a polygon shape.
    * @value Pixel - Represents a single pixel shape.
    * @value Line - Represents a line shape.
*/
public enum Shape2dType
{
    Circle,
    CircleLines,

    Rectangle,
    RectangleLines,
    RectangleRounded,
    RectangleRoundedLines,

    Triangle,
    TriangleLines,

    Ellipse,
    EllipseLines,

    Ring,

    Polygon,
    PolygonLines,

    Pixel,
    Line,
}

/**
    * @enum Shape3dType
    * @brief An enumeration of 3D shape types that can be rendered.
    * @details This enum defines the different types of 3D shapes that can be drawn in the scene.
    * @value Cube - Represents a cube shape.
    * @value Sphere - Represents a sphere shape.
    * @value Cylinder - Represents a cylinder shape.
    * @value Capsule - Represents a capsule shape.
    * @value Plane - Represents a plane shape.
    * @value Grid - Represents a grid shape.
    * @value Triangle - Represents a triangle shape.
    * @value Line3D - Represents a 3D line shape.
*/
public enum Shape3dType
{
    Cube,
    Sphere,
    Cylinder,
    Capsule,
    Plane,
    Grid,
    Triangle,
    Line3D,
}

/**
    * @class Shape2dRenderComponent
    * @brief A component that represents a 2D shape to be rendered in the scene.
    * @details This component holds information about the 2D shape type, color, and dimensions for rendering.
    * @property {Shape2dType} shape - The type of 2D shape to render (Circle, Triangle, Rectangle, etc.).
    * @property {Color} color - The color of the 2D shape to render.
    * @property {float} radius - The radius of the 2D shape (for circles).
    * @property {float} innerRadius - The inner radius of the 2D shape (for rings).
    * @property {int} sides - The number of sides for the 2D shape (for polygons).
    * @property {float} roundness - The roundness factor for rounded rectangles.
*/
public class Shape2dRenderComponent : Component
{
    public Shape2dType Shape;
    public Color Color = Color.White;

    public float Radius = 32.0f;
    public float InnerRadius = 16.0f;
    public int Sides = 6;
    public float Roundness = 0.25f;
}

/**
    * @class Shape3dRenderComponent
    * @brief A component that represents a 3D shape to be rendered in the scene.
    * @details This component holds information about the 3D shape type, color, and dimensions for rendering.
    * @property {Shape3dType} shape - The type of 3D shape to render (Cube, Sphere, Cylinder, etc.).
    * @property {Color} color - The color of the 3D shape to render.
    * @property {float} radius - The radius of the 3D shape (for spheres and cylinders).
    * @property {float} radiusTop - The top radius of the cylinder (for cylinders).
    * @property {float} radiusBottom - The bottom radius of the cylinder (for cylinders).
    * @property {float} height - The height of the 3D shape (for cylinders).
    * @property {int} sides - The number of sides for the 3D shape (for cylinders).
    * @property {int} slices - The number of slices for the 3D shape (for spheres).
*/
public class Shape3dRenderComponent : Component
{
    public Shape3dType Shape;
    public Color Color = Color.White;

    public float Radius = 1.0f;
    public float RadiusTop = 1.0f;
    public float RadiusBottom = 1.0f;
    public float Height = 1.0f;
    public int Sides = 16;
    public int Slices = 10;
}

/**
    * @class DrawShapesSystem
    * @brief A system that draws shapes in the scene.
*/
public class DrawShapesSystem : IRenderSystem
{
    private readonly WorldQuery query2d = Services.World
        .Query()
        .With<Transform2dComponent>()
        .With<Shape2dRenderComponent>();

    private readonly WorldQuery query3d = Services.World
        .Query()
        .With<Transform3dComponent>()
        .With<Shape3dRenderComponent>();

    /**
     * @brief Draws every shape.
     */
    public override void Draw()
    {
        Draw2D();
        Draw3D();
    }

    /**
     * @brief Draws every 2D shape.
     */
    private void Draw2D()
    {
        foreach (Entity entity in query2d)
        {
            Shape2dRenderComponent shape = Services.World.GetComponent<Shape2dRenderComponent>(entity);
            Transform2dComponent transform = Services.World.GetComponent<Transform2dComponent>(entity);

            switch (shape.Shape)
            {
                case Shape2dType.Circle:
                    Raylib.DrawCircleV(transform.Position, shape.Radius, shape.Color);
                    break;

                case Shape2dType.CircleLines:
                    Raylib.DrawCircleLinesV(transform.Position, shape.Radius, shape.Color);
                    break;

                case Shape2dType.Rectangle:
                    Raylib.DrawRectangleV(transform.Position, transform.Scale, shape.Color);
                    break;

                case Shape2dType.RectangleLines:
                    Raylib.DrawRectangleLinesEx(
                        new Rectangle(
                            transform.Position.X,
                            transform.Position.Y,
                            transform.Scale.X,
                            transform.Scale.Y),
                        1.0f,
                        shape.Color);
                    break;

                case Shape2dType.RectangleRounded:
                    Raylib.DrawRectangleRounded(
                        new Rectangle(
                            transform.Position.X,
                            transform.Position.Y,
                            transform.Scale.X,
                            transform.Scale.Y),
                        shape.Roundness,
                        shape.Sides,
                        shape.Color);
                    break;

                case Shape2dType.RectangleRoundedLines:
                    Raylib.DrawRectangleRoundedLinesEx(
                        new Rectangle(
                            transform.Position.X,
                            transform.Position.Y,
                            transform.Scale.X,
                            transform.Scale.Y),
                        shape.Roundness,
                        shape.Sides,
                        1.0f,
                        shape.Color);
                    break;

                case Shape2dType.Triangle:
                    {
                        Vector2 p1 = transform.Position;
                        Vector2 p2 = transform.Position + new Vector2(transform.Scale.X, 0);
                        Vector2 p3 = transform.Position + new Vector2(transform.Scale.X * 0.5f, transform.Scale.Y);

                        Raylib.DrawTriangle(p1, p2, p3, shape.Color);
                        break;
                    }

                case Shape2dType.TriangleLines:
                    {
                        Vector2 p1 = transform.Position;
                        Vector2 p2 = transform.Position + new Vector2(transform.Scale.X, 0);
                        Vector2 p3 = transform.Position + new Vector2(transform.Scale.X * 0.5f, transform.Scale.Y);

                        Raylib.DrawTriangleLines(p1, p2, p3, shape.Color);
                        break;
                    }

                case Shape2dType.Ellipse:
                    Raylib.DrawEllipse(
                        (int)transform.Position.X,
                        (int)transform.Position.Y,
                        transform.Scale.X,
                        transform.Scale.Y,
                        shape.Color);
                    break;

                case Shape2dType.EllipseLines:
                    Raylib.DrawEllipseLines(
                        (int)transform.Position.X,
                        (int)transform.Position.Y,
                        transform.Scale.X,
                        transform.Scale.Y,
                        shape.Color);
                    break;

                case Shape2dType.Ring:
                    Raylib.DrawRing(
                        transform.Position,
                        shape.InnerRadius,
                        shape.Radius,
                        0,
                        360,
                        Math.Max(shape.Sides, 16),
                        shape.Color);
                    break;

                case Shape2dType.Polygon:
                    Raylib.DrawPoly(
                        transform.Position,
                        shape.Sides,
                        shape.Radius,
                        transform.Rotation,
                        shape.Color);
                    break;

                case Shape2dType.PolygonLines:
                    Raylib.DrawPolyLines(
                        transform.Position,
                        shape.Sides,
                        shape.Radius,
                        transform.Rotation,
                        shape.Color);
                    break;

                case Shape2dType.Pixel:
                    Raylib.DrawPixelV(transform.Position, shape.Color);
                    break;

                case Shape2dType.Line:
                    Raylib.DrawLineV(
                        transform.Position,
                        transform.Position + transform.Scale,
                        shape.Color);
                    break;
            }
        }
    }

    /**
     * @brief Draws every 3D shape.
     */
    private void Draw3D()
    {
        var IsDebugViewEnabled = Services.Window.IsDebugViewEnabled();

        foreach (Entity entity in query3d)
        {
            Shape3dRenderComponent shape = Services.World.GetComponent<Shape3dRenderComponent>(entity);
            Transform3dComponent transform = Services.World.GetComponent<Transform3dComponent>(entity);

            switch (shape.Shape)
            {
                case Shape3dType.Cube:
                    if (!IsDebugViewEnabled)
                        Raylib.DrawCube(
                            transform.Position,
                            transform.Scale.X,
                            transform.Scale.Y,
                            transform.Scale.Z,
                            shape.Color);
                    else
                        Raylib.DrawCubeWires(
                            transform.Position,
                            transform.Scale.X,
                            transform.Scale.Y,
                            transform.Scale.Z,
                            shape.Color);
                    break;

                case Shape3dType.Sphere:
                    if (!IsDebugViewEnabled)
                        Raylib.DrawSphere(
                            transform.Position,
                            shape.Radius,
                            shape.Color);
                    else
                        Raylib.DrawSphereWires(
                            transform.Position,
                            shape.Radius,
                            Math.Max(shape.Slices, 3),
                            Math.Max(shape.Sides, 6),
                            shape.Color);
                    break;

                case Shape3dType.Cylinder:
                    if (!IsDebugViewEnabled)
                        Raylib.DrawCylinder(
                            transform.Position,
                            shape.RadiusTop,
                            shape.RadiusBottom,
                            shape.Height,
                            Math.Max(shape.Sides, 3),
                            shape.Color);
                    else
                        Raylib.DrawCylinderWires(
                            transform.Position,
                            shape.RadiusTop,
                            shape.RadiusBottom,
                            shape.Height,
                            Math.Max(shape.Sides, 3),
                            shape.Color);
                    break;

                case Shape3dType.Capsule:
                    if (!IsDebugViewEnabled)
                        Raylib.DrawCapsule(
                            transform.Position + Vector3.UnitY * (shape.Height * 0.5f),
                            transform.Position - Vector3.UnitY * (shape.Height * 0.5f),
                            shape.Radius,
                            Math.Max(shape.Sides, 3),
                            16,
                            shape.Color);
                    else
                        Raylib.DrawCapsuleWires(
                            transform.Position + Vector3.UnitY * (shape.Height * 0.5f),
                            transform.Position - Vector3.UnitY * (shape.Height * 0.5f),
                            shape.Radius,
                            Math.Max(shape.Sides, 3),
                            16,
                            shape.Color);
                    break;

                case Shape3dType.Plane:
                    if (!IsDebugViewEnabled)
                        Raylib.DrawPlane(
                            transform.Position,
                            new Vector2(transform.Scale.X, transform.Scale.Z),
                            shape.Color);
                    else
                        Raylib.DrawCubeWires(
                            transform.Position,
                            transform.Scale.X,
                            0.0f,
                            transform.Scale.Z,
                            shape.Color);
                    break;

                case Shape3dType.Grid:
                    if (!IsDebugViewEnabled)
                        Raylib.DrawGrid(
                            10,
                            1.0f);
                    break;

                case Shape3dType.Triangle:
                    {
                        Vector3 p1 = transform.Position;
                        Vector3 p2 = transform.Position + new Vector3(transform.Scale.X, 0, 0);
                        Vector3 p3 = transform.Position + new Vector3(
                            transform.Scale.X * 0.5f,
                            transform.Scale.Y,
                            transform.Scale.Z);

                        if (!IsDebugViewEnabled)
                            Raylib.DrawTriangle3D(p1, p2, p3, shape.Color);
                        break;
                    }

                case Shape3dType.Line3D:
                    Raylib.DrawLine3D(
                        transform.Position,
                        transform.Position + transform.Scale,
                        shape.Color);
                    break;
            }
        }
    }
}
