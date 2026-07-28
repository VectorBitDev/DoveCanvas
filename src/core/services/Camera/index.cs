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
    Camera3D
}

/**
    * @class CameraService
    * @brief A service that provides functionality.
*/
public class CameraService : Singleton<CameraService>
{
    private CameraType cameraType;

    private Camera2D camera2D;
    private Camera3D camera3D;

    /**
        * @brief Initializes the camera service with a 2D camera.
        * @param {Camera2D} camera - The 2D camera to initialize the service with.
    */
    public void InitializeCamera(Camera2D camera)
    {
        cameraType = CameraType.Camera2D;
        camera2D = camera;
    }

    /**
        * @brief Initializes the camera service with a 3D camera.
        * @param {Camera3D} camera - The 3D camera to initialize the service with.
    */
    public void InitializeCamera(Camera3D camera)
    {
        cameraType = CameraType.Camera3D;
        camera3D = camera;
    }

    /**
        * @brief Begins the camera mode based on the initialized camera type.
        * @details This method switches the rendering context to the appropriate camera mode (2D or 3D) based on the initialized camera type.
    */
    internal void BeginCamera()
    {
        switch (cameraType)
        {
            case CameraType.Camera2D:
                Raylib.BeginMode2D(camera2D);
                break;
            case CameraType.Camera3D:
                Raylib.BeginMode3D(camera3D);
                break;
            default:
                return;
        }
    }

    /**
        * @brief Ends the camera mode based on the initialized camera type.
        * @details This method switches the rendering context back to the default mode after rendering with the appropriate camera mode (2D or 3D).
    */
    internal void EndCamera()
    {
        switch (cameraType)
        {
            case CameraType.Camera2D:
                Raylib.EndMode2D();
                break;
            case CameraType.Camera3D:
                Raylib.EndMode3D();
                break;
            default:
                return;
        }
    }

    /**
        * @brief Checks if the initialized camera is a 2D camera.
        * @returns {bool} True if the initialized camera is a 2D camera, false otherwise.
    */
    public bool isCamera2D()
    {
        return cameraType == CameraType.Camera2D;
    }

    /**
        * @brief Checks if the initialized camera is a 3D camera.
        * @returns {bool} True if the initialized camera is a 3D camera, false otherwise.
    */
    public bool isCamera3D()
    {
        return cameraType == CameraType.Camera3D;
    }

    /**
        * @brief Converts a 3D world position to a 2D screen position based on the initialized camera type.
        * @param {Vector3} worldPosition - The 3D world position to convert.
        * @returns {Vector2?} The corresponding 2D screen position if the initialized camera is a 3D camera, null otherwise.
    */
    public Vector2? WorldToScreen(Vector3 worldPosition)
    {
        if (cameraType == CameraType.Camera3D)
        {
            return Raylib.GetWorldToScreen(worldPosition, camera3D);
        }
        else
        {
            return null;
        }
    }
}
