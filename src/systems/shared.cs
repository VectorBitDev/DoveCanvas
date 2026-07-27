using System.Numerics;

namespace DoveCanvas;

/**
 * @class Transform2dComponent
 * @brief A component that represents the position, scale, and rotation of an entity in 2D space.
 */
public class Transform2dComponent : Component
{
    public Vector2 Position = new Vector2(0, 0);
    public Vector2 scale = new Vector2(1, 1);
    public float rotation = 0.0f;
}

/**
 * @class Transform3dComponent
 * @brief A component that represents the position, scale, and rotation of an entity in 3D space.
 */
public class Transform3dComponent : Component
{
    public Vector3 Position = new Vector3(0, 0, 0);
    public Vector3 scale = new Vector3(1, 1, 1);
    public Quaternion rotation = Quaternion.Identity;
}
