using DoveCanvas.Abstract;
using System.Numerics;
using Raylib_cs;

namespace DoveCanvas;


public class FreeCamComponent : Component
{
    public float Speed = 100.0f;
    public float SprintMultiplier = 4.0f;
    public float Sensitivity = 0.1f;

    public float Pitch = 0.0f;
    public float Yaw = -90.0f;
}
public class FreeCamSystem : ISimulationSystem
{
    private readonly WorldQuery query = Services.World
        .Query()
        .With<FreeCamComponent>()
        .With<Camera3dComponent>()
        .With<Transform3dComponent>();

    private bool _cursorEnabled = true;

    public bool IsCursorEnabled => _cursorEnabled;

    /**
     * @brief Enables the cursor and unlocks it from the center of the window.
     */
    public void EnableCursor()
    {
        Raylib.EnableCursor();
        _cursorEnabled = true;
    }

    /**
     * @brief Disables the cursor and locks it to the center of the window.
     */
    public void DisableCursor()
    {
        Raylib.DisableCursor();
        _cursorEnabled = false;
    }

    /**
     * @brief Toggles the cursor state between enabled and disabled.
     */
    public void ToggleCursor()
    {
        if (_cursorEnabled)
        {
            DisableCursor();
        }
        else
        {
            EnableCursor();
        }
    }

    /**
     * @brief Updates all free cameras.
     * @param dt Delta time.
     */
    public override void Tick(float dt)
    {
        foreach (Entity entity in query)
        {
            FreeCamComponent freeCam = Services.World.GetComponent<FreeCamComponent>(entity);
            Transform3dComponent transform = Services.World.GetComponent<Transform3dComponent>(entity);

            UpdateRotation(freeCam, transform);
            UpdateMovement(freeCam, transform, dt);
        }
    }

    /**
     * @brief Updates the camera rotation from mouse movement.
     */
    private void UpdateRotation(FreeCamComponent freeCam, Transform3dComponent transform)
    {
        if (!Raylib.IsMouseButtonDown(MouseButton.Right))
        {
            if (IsCursorEnabled == false)
            {
                EnableCursor();
            }
            return;
        }
        else
        {
            if (IsCursorEnabled == true)
            {
                DisableCursor();
            }
        }

        Vector2 delta = Raylib.GetMouseDelta();

        freeCam.Yaw -= delta.X * freeCam.Sensitivity;
        freeCam.Pitch += delta.Y * freeCam.Sensitivity;

        freeCam.Pitch = Math.Clamp(freeCam.Pitch, -89.0f, 89.0f);

        float yaw = MathF.PI / 180.0f * freeCam.Yaw;
        float pitch = MathF.PI / 180.0f * -freeCam.Pitch;

        transform.Rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0.0f);
    }

    /**
     * @brief Updates the camera position from keyboard input.
     */
    private void UpdateMovement(FreeCamComponent freeCam, Transform3dComponent transform, float dt)
    {
        Vector3 forward = Vector3.Normalize(
            Vector3.Transform(-Vector3.UnitZ, transform.Rotation));

        Vector3 right = Vector3.Normalize(
            Vector3.Transform(Vector3.UnitX, transform.Rotation));

        Vector3 movement = Vector3.Zero;

        if (Raylib.IsKeyDown(KeyboardKey.W))
            movement += forward;

        if (Raylib.IsKeyDown(KeyboardKey.S))
            movement -= forward;

        if (Raylib.IsKeyDown(KeyboardKey.D))
            movement += right;

        if (Raylib.IsKeyDown(KeyboardKey.A))
            movement -= right;

        if (Raylib.IsKeyDown(KeyboardKey.Space))
            movement += Vector3.UnitY;

        if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
            movement -= Vector3.UnitY;

        if (movement == Vector3.Zero)
            return;

        movement = Vector3.Normalize(movement);

        float speed = freeCam.Speed;

        if (Raylib.IsKeyDown(KeyboardKey.LeftControl))
        {
            speed *= freeCam.SprintMultiplier;
        }

        transform.Position += movement * speed * dt;
    }
}
