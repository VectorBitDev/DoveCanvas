using DoveCanvas.Abstract;
using System.Numerics;
using Raylib_cs;

namespace DoveCanvas;

/**
    * @enum CameraType
    * @brief An enumeration that defines the types of cameras available in the engine.
*/
public enum CameraType
{
    Camera2D,
    Camera3D,
    Null
}

/**
    * @class CameraService
    * @brief A service that provides functionality.
*/
public class CameraService : Singleton<CameraService>
{
    private Entity? cameraEntity;
    private CameraType? cameraType;

    private Camera2D camera2D = new Camera2D();
    private Camera3D camera3D = new Camera3D();

    /**
        * @brief Registers a camera entity and its type.
        * @param cameraEntity The entity representing the camera.
        * @param cameraType The type of the camera (2D or 3D).
    */
    public void RegisterCameraEntity(Entity cameraEntity, CameraType cameraType)
    {
        this.cameraEntity = cameraEntity;
        this.cameraType = cameraType;
    }

    /**
        * @brief Unregisters the current camera entity and resets the camera type to null.
        * @details This method clears the reference to the current camera entity and sets the camera type to null, effectively disabling any active camera mode.
    */
    public void UnRegisterCameraEntity()
    {
        if (this.cameraEntity == null || this.cameraType == null)
        {
            return;
        }

        this.cameraEntity = null;
        this.cameraType = CameraType.Null;
    }

    private void UpdateCamera()
    {
        if (this.cameraEntity == null || this.cameraType == null)
        {
            return;
        }

        switch (this.cameraType)
        {
            case CameraType.Camera2D:
                var camera2DComponent = Services.World.GetComponent<Camera2dComponent>(this.cameraEntity.Value);
                var transform2DComponent = Services.World.GetComponent<Transform2dComponent>(this.cameraEntity.Value);

                this.camera2D.Zoom = camera2DComponent.Zoom;
                this.camera2D.Offset = camera2DComponent.Offset;
                this.camera2D.Rotation = camera2DComponent.Rotation;
                this.camera2D.Target = transform2DComponent.Position;
                break;
            case CameraType.Camera3D:
                var camera3DComponent = Services.World.GetComponent<Camera3dComponent>(cameraEntity.Value);
                var transform = Services.World.GetComponent<Transform3dComponent>(cameraEntity.Value);

                Vector3 forward = Vector3.Normalize(
                    Vector3.Transform(-Vector3.UnitZ, transform.Rotation));

                camera3D.Position = transform.Position;
                camera3D.Target = transform.Position + forward;
                camera3D.Up = Vector3.Transform(Vector3.UnitY, transform.Rotation);

                camera3D.FovY = camera3DComponent.FovY;
                camera3D.Projection = camera3DComponent.Projection;
                break;
            default:
                break;
        }
    }

    /**
        * @brief Begins the camera mode based on the initialized camera type.
        * @details This method switches the rendering context to the appropriate camera mode (2D or 3D) based on the initialized camera type.
    */
    internal void BeginCamera()
    {
        this.UpdateCamera();

        switch (this.cameraType)
        {
            case CameraType.Camera2D:
                Raylib.BeginMode2D(this.camera2D);
                break;
            case CameraType.Camera3D:
                Raylib.BeginMode3D(this.camera3D);
                break;
            default:
                break;
        }
    }

    /**
        * @brief Ends the camera mode based on the initialized camera type.
        * @details This method switches the rendering context back to the default mode after rendering with the appropriate camera mode (2D or 3D).
    */
    internal void EndCamera()
    {
        switch (this.cameraType)
        {
            case CameraType.Camera2D:
                Raylib.EndMode2D();
                break;
            case CameraType.Camera3D:
                Raylib.EndMode3D();
                break;
            default:
                break;
        }
    }

    /**
        * @brief Checks if the initialized camera is a 2D camera.
        * @returns {bool} True if the initialized camera is a 2D camera, false otherwise.
    */
    public bool isCamera2D()
    {
        switch (this.cameraType)
        {
            case CameraType.Camera2D:
                return true;
            default:
                return false;
        }
    }

    /**
        * @brief Checks if the initialized camera is a 3D camera.
        * @returns {bool} True if the initialized camera is a 3D camera, false otherwise.
    */
    public bool isCamera3D()
    {
        switch (this.cameraType)
        {
            case CameraType.Camera3D:
                return true;
            default:
                return false;
        }
    }

    /**
        * @brief Converts a 3D world position to a 2D screen position based on the initialized camera type.
        * @param {Vector3} worldPosition - The 3D world position to convert.
        * @returns {Vector2?} The corresponding 2D screen position if the initialized camera is a 3D camera, null otherwise.
    */
    public Vector2? WorldToScreen(Vector3 worldPosition)
    {
        switch (this.cameraType)
        {
            case CameraType.Camera3D:
                return Raylib.GetWorldToScreen(worldPosition, this.camera3D);
            default:
                return null;
        }
    }
}
