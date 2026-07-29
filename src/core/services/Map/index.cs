using System.Text.Json;
using System.Text.Json.Nodes;
using DoveCanvas.Abstract;
using Raylib_cs;

namespace DoveCanvas;

public enum MapType
{
    Map2d,
    Map3d,
}

/**
 * @class MapService
 * @brief Saves and loads maps.
 */
public class MapService : Singleton<MapService>
{
    private static readonly JsonSerializerOptions Options = new()
    {
        IncludeFields = true,
        WriteIndented = true
    };

    /// <summary>
    /// Component types that prevent an entity from being saved.
    /// </summary>
    private static readonly HashSet<Type> IgnoredComponents =
    [
        typeof(DontSaveComponent),
    ];

    public void UnloadMap()
    {
    }

    /**
     * @brief Saves the current world to a map file.
     */
    public void SaveMap(string mapName, string saveToPath)
    {
        var world = Services.World;

        JsonObject mapData = Json.Object();
        JsonArray entitiesData = Json.Array();

        mapData["MapName"] = mapName;
        mapData["Entities"] = entitiesData;

        IReadOnlyList<Entity> entities = world.GetAllEntities();
        IReadOnlyList<Type> componentTypes = world.GetComponentTypes();

        foreach (Entity entity in entities)
        {
            if (ShouldIgnoreEntity(world, entity))
            {
                continue;
            }

            JsonObject entityData = Json.Object();
            JsonArray componentsData = Json.Array();

            entityData["Components"] = componentsData;

            foreach (Type componentType in componentTypes)
            {
                if (!world.HasComponent(entity, componentType))
                {
                    continue;
                }

                Component component = world.GetComponent(entity, componentType)!;

                JsonObject componentData = Json.Object();
                componentData["Type"] = componentType.FullName;
                componentData["Data"] = JsonSerializer.SerializeToNode(
                    component,
                    component.GetType(),
                    Options);

                componentsData.Add(componentData);
            }

            entitiesData.Add(entityData);
        }

        string? directory = Path.GetDirectoryName(saveToPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(saveToPath, Json.Stringify(mapData));
    }

    /**
     * @brief Loads a map from disk.
     */
    public void LoadMap(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        JsonNode? mapData = JsonNode.Parse(jsonString);

        if (mapData == null)
        {
            throw new Exception("Failed to parse map data.");
        }

        JsonArray? entitiesData = mapData["Entities"]?.AsArray();

        if (entitiesData == null)
        {
            throw new Exception("No entities found in map data.");
        }

        var world = Services.World;

        foreach (JsonNode? entityNode in entitiesData)
        {
            if (entityNode == null)
            {
                continue;
            }

            Entity entity = world.CreateEntity();

            JsonArray? componentsData = entityNode["Components"]?.AsArray();

            if (componentsData == null)
            {
                continue;
            }

            foreach (JsonNode? componentNode in componentsData)
            {
                if (componentNode == null)
                {
                    continue;
                }

                string? typeName = componentNode["Type"]?.GetValue<string>();
                JsonNode? componentData = componentNode["Data"];

                if (typeName == null || componentData == null)
                {
                    continue;
                }

                Type? componentType = Type.GetType(typeName);

                if (componentType == null || !typeof(Component).IsAssignableFrom(componentType))
                {
                    continue;
                }

                Component? componentInstance = (Component?)componentData.Deserialize(componentType, Options);

                if (componentInstance == null)
                {
                    continue;
                }

                if (componentInstance is ModelRenderComponent modelComponent)
                {
                    Model? model = Services.Resource.LoadModel(modelComponent.ModelPath);

                    if (model == null)
                    {
                        Logger.Error($"Failed to load model '{modelComponent.ModelPath}'.");
                        continue;
                    }

                    modelComponent.Model = model.Value;
                }

                world.AddComponent(entity, componentInstance);
            }
        }
    }

    /**
     * @brief Returns whether an entity should be skipped during serialization.
     */
    private static bool ShouldIgnoreEntity(WorldService world, Entity entity)
    {
        foreach (Type componentType in IgnoredComponents)
        {
            if (world.HasComponent(entity, componentType))
            {
                return true;
            }
        }

        return false;
    }

    internal void DrawMap()
    {
    }
}
