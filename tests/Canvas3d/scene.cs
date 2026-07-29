using DoveCanvas.Abstract;
using DoveCanvas;
using Raylib_cs;
using System.Numerics;

public class Canvas3dScene : Scene3d
{

    private void CreateShapeEntity(Shape3dType shapeType, Vector3 position, Vector3 scale, Color color)
    {
        Entity entity = Services.World.CreateEntity();
        Services.World.AddComponent(entity, new Transform3dComponent()
        {
            Position = position,
            Scale = scale
        });
        Services.World.AddComponent(entity, new Shape3dRenderComponent()
        {
            Shape = shapeType,
            Color = color
        });
    }

    private void CreateMeshModelEntity(string modelPath, Vector3 position, Vector3 scale)
    {
        Model? loadedModel = Services.Resource.LoadModel(modelPath);

        if (loadedModel == null)
        {
            Logger.Error("Failed to load model.");
            return;
        }

        Logger.Info($"Meshes: {loadedModel.Value.MeshCount}");
        Logger.Info($"Materials: {loadedModel.Value.MaterialCount}");

        Entity entity = Services.World.CreateEntity();
        Services.World.AddComponent(entity, new Transform3dComponent()
        {
            Position = position,
            Scale = scale
        });
        Services.World.AddComponent(entity, new ModelRenderComponent
        {
            ModelPath = modelPath,
            Model = loadedModel.Value
        });
    }

    public override void Load()
    {
        base.Load();

        Logger.Info("Creating 3D Entities");

        CreateShapeEntity(Shape3dType.Plane, new Vector3(0, 0, 0), new Vector3(100, 1, 100), Color.Gray);
        CreateShapeEntity(Shape3dType.Cube, new Vector3(-8, 1, 0), new Vector3(2, 2, 2), Color.Blue);
        // CreateShapeEntity(Shape3dType.CubeWires, new Vector3(-4, 1, 0), new Vector3(2, 2, 2), Color.Red);
        // CreateShapeEntity(Shape3dType.Sphere, new Vector3(0, 1, 0), new Vector3(1, 2, 1), Color.Purple);
        // CreateShapeEntity(Shape3dType.SphereWires, new Vector3(4, 1, 0), new Vector3(1, 2, 1), Color.Maroon);
        // CreateShapeEntity(Shape3dType.Cylinder, new Vector3(8, 1, 0), new Vector3(1, 2.5f, 1), Color.Green);
        // CreateShapeEntity(Shape3dType.Capsule, new Vector3(12, 2, 0), new Vector3(0.75f, 3.0f, 0.75f), Color.Orange);
        // CreateShapeEntity(Shape3dType.CapsuleWires, new Vector3(16, 2, 0), new Vector3(0.75f, 3.0f, 0.75f), Color.Brown);
        // CreateShapeEntity(Shape3dType.Line3D, new Vector3(20, 1, 0), new Vector3(1, 2.5f, 1), Color.Pink);

        CreateMeshModelEntity("assets/monkey.glb", new Vector3(5, 1, 0), new Vector3(1, 1, 1));

        Entity camera = GetCameraEntity();

        Services.World.AddComponent(camera, new FreeCamComponent()
        {
            Speed = 10.0f,
            Sensitivity = 0.1f
        });

        // Services.Map.SaveMap("TestMap", "maps/test_map.json");
        // Services.Map.LoadMap("maps/test_map.json");
        Logger.Info("3D Entities Created");
    }

    public override void Unload()
    {
        base.Unload();
    }
}
