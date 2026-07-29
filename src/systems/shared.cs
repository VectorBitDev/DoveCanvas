using System.Numerics;
using Raylib_cs;

namespace DoveCanvas;

/**
 * @class Transform2dComponent
 * @brief A component that represents the position, scale, and rotation of an entity in 2D space.
 */
public class Transform2dComponent : Component
{
    public Vector2 Position = new Vector2(0, 0);
    public Vector2 Scale = new Vector2(1, 1);
    public float Rotation = 0.0f;
}

/**
 * @class Transform3dComponent
 * @brief A component that represents the position, scale, and rotation of an entity in 3D space.
 */
public class Transform3dComponent : Component
{
    public Vector3 Position = new Vector3(0, 0, 0);
    public Vector3 Scale = new Vector3(1, 1, 1);
    public Quaternion Rotation = Quaternion.Identity;
}

/**
 * @class Camera2dComponent
 * @brief A component that represents a 2D camera in the scene.
 */
public class Camera2dComponent : Component
{
    public float Zoom = 1.0f;
    public float Rotation = 0.0f;
    public Vector2 Offset = Vector2.Zero;

    public bool Active = true;
}

/**
 * @class Camera3dComponent
 * @brief A component that represents a 3D camera in the scene.
 */
public class Camera3dComponent : Component
{
    public float FovY = 45.0f;

    public CameraProjection Projection = CameraProjection.Perspective;

    public Vector3 Up = Vector3.UnitY;

    public bool Active = true;
}

/**
 * @class DontSaveComponent
 * @brief A component that marks an entity to be excluded from map saving.
 */
public class DontSaveComponent : Component
{
    // This component is used to mark entities that should not be saved when saving the map.
}
