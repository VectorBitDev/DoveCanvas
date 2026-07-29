using DoveCanvas.Abstract;

namespace DoveCanvas;

/**
    * @class WorldService
    * @brief Manages entities, components and queries.
*/
public class WorldService : Singleton<WorldService>
{
    private uint nextEntityId = 1;

    private readonly List<Entity> entities = [];
    private readonly Queue<uint> freeEntityIds = [];
    private readonly Dictionary<uint, Entity> entityLookup = [];

    private readonly List<Type> componentTypes = [];

    private readonly Dictionary<Type, Dictionary<uint, Component>> componentPools = [];

    private readonly List<WorldQuery> queries = [];

    private readonly Dictionary<uint, List<Action<Entity>>> entityDestroyedCallbacks = [];

    /**
        * @brief Gets all entities in the world.
    */
    internal IReadOnlyList<Entity> Entities => entities;

    /**
        * @brief Initializes the world.
    */
    public void Initialize()
    {
        foreach (Type type in typeof(WorldService).Assembly.GetTypes())
        {
            if (type.IsAbstract)
            {
                continue;
            }

            if (!typeof(Component).IsAssignableFrom(type))
            {
                continue;
            }

            componentTypes.Add(type);
        }
    }

    /**
        * @brief Creates a new entity.
        * @return The created entity.
    */
    public Entity CreateEntity()
    {
        uint id;

        if (freeEntityIds.Count > 0)
        {
            id = freeEntityIds.Dequeue();
        }
        else
        {
            id = nextEntityId++;
        }

        Entity entity = new(id);

        entities.Add(entity);
        entityLookup.Add(entity.Id, entity);

        InvalidateQueries();

        return entity;
    }

    /**
        * @brief Destroys an entity.
        * @param entity The entity to destroy.
    */
    public void DestroyEntity(Entity entity)
    {
        if (!entityLookup.ContainsKey(entity.Id))
        {
            return;
        }

        if (entityDestroyedCallbacks.TryGetValue(entity.Id, out List<Action<Entity>>? callbacks))
        {
            foreach (Action<Entity> callback in callbacks)
            {
                callback(entity);
            }

            entityDestroyedCallbacks.Remove(entity.Id);
        }

        entityLookup.Remove(entity.Id);
        entities.Remove(entity);

        foreach (Dictionary<uint, Component> pool in componentPools.Values)
        {
            pool.Remove(entity.Id);
        }

        freeEntityIds.Enqueue(entity.Id);

        InvalidateQueries();
    }

    /**
        * @brief Adds a component to an entity.
        * @param entity The entity.
        * @param component The component.
    */
    public void AddComponent(Entity entity, Component component)
    {
        Type type = component.GetType();

        if (!componentPools.TryGetValue(type, out Dictionary<uint, Component>? pool))
        {
            pool = [];
            componentPools.Add(type, pool);
        }

        pool[entity.Id] = component;

        InvalidateQueries();
    }

    /**
        * @brief Adds a component.
        * @param entity The entity.
        * @param type The component type.
    */
    public void AddComponent(Entity entity, Type type)
    {
        if (!typeof(Component).IsAssignableFrom(type))
        {
            return;
        }

        if (HasComponent(entity, type))
        {
            return;
        }

        if (Activator.CreateInstance(type) is not Component component)
        {
            return;
        }

        if (!componentPools.TryGetValue(type, out Dictionary<uint, Component>? pool))
        {
            pool = [];
            componentPools.Add(type, pool);
        }

        pool.Add(entity.Id, component);

        InvalidateQueries();
    }

    /**
        * @brief Removes a component from an entity.
        * @param entity The entity.
    */
    public void RemoveComponent<T>(Entity entity) where T : Component
    {
        Dictionary<uint, Component> pool = GetPool<T>();

        if (!pool.Remove(entity.Id))
        {
            return;
        }

        InvalidateQueries();
    }

    /**
        * @brief Returns a component.
        * @param entity The entity.
        * @return The component.
    */
    public T GetComponent<T>(Entity entity) where T : Component
    {
        Dictionary<uint, Component> pool = GetPool<T>();

        return (T)pool[entity.Id];
    }

    /**
        * @brief Returns a component.
        * @param entity The entity.
        * @param type The component type.
    */
    public Component? GetComponent(Entity entity, Type type)
    {
        if (!componentPools.TryGetValue(type, out Dictionary<uint, Component>? pool))
        {
            return null;
        }

        if (!pool.TryGetValue(entity.Id, out Component? component))
        {
            return null;
        }

        return component;
    }

    /**
        * @brief Returns every component attached to an entity.
        * @param entity The entity.
    */
    public IReadOnlyList<Component> GetComponents(Entity entity)
    {
        List<Component> components = [];

        foreach (Dictionary<uint, Component> pool in componentPools.Values)
        {
            if (!pool.TryGetValue(entity.Id, out Component? component))
            {
                continue;
            }

            components.Add(component);
        }

        return components;
    }

    /**
        * @brief Removes a component.
        * @param entity The entity.
        * @param type The component type.
    */
    public void RemoveComponent(Entity entity, Type type)
    {
        if (!componentPools.TryGetValue(type, out Dictionary<uint, Component>? pool))
        {
            return;
        }

        if (!pool.Remove(entity.Id))
        {
            return;
        }

        InvalidateQueries();
    }

    /**
        * @brief Tries to get a component.
        * @param entity The entity.
        * @param component The resulting component.
        * @return True if found.
    */
    public bool TryGetComponent<T>(Entity entity, out T? component) where T : Component
    {
        Dictionary<uint, Component> pool = GetPool<T>();

        if (pool.TryGetValue(entity.Id, out Component? value))
        {
            component = (T)value;
            return true;
        }

        component = null;
        return false;
    }

    /**
        * @brief Returns whether an entity has a component.
        * @param entity The entity.
    */
    public bool HasComponent<T>(Entity entity) where T : Component
    {
        return GetPool<T>().ContainsKey(entity.Id);
    }

    /**
        * @brief Returns whether an entity has a component.
        * @param entity The entity.
        * @param type The component type.
    */
    public bool HasComponent(Entity entity, Type type)
    {
        if (!componentPools.TryGetValue(type, out Dictionary<uint, Component>? pool))
        {
            return false;
        }

        return pool.ContainsKey(entity.Id);
    }

    /**
        * @brief Creates a query.
        * @return A new query.
    */
    public WorldQuery Query()
    {
        WorldQuery query = new(this);

        queries.Add(query);

        return query;
    }

    /**
        * @brief Returns all entities.
        * @return The entity list.
    */
    public IReadOnlyList<Entity> GetAllEntities()
    {
        return entities;
    }

    /**
        * @brief Returns every registered component type.
        * @return The component types.
    */
    public IReadOnlyList<Type> GetComponentTypes()
    {
        return componentTypes;
    }

    /**
        * @brief Returns the storage pool for a component.
    */
    private Dictionary<uint, Component> GetPool<T>() where T : Component
    {
        Type type = typeof(T);

        if (!componentPools.TryGetValue(type, out Dictionary<uint, Component>? pool))
        {
            pool = [];
            componentPools.Add(type, pool);
        }

        return pool;
    }

    /**
        * @brief Invalidates all cached queries.
    */
    private void InvalidateQueries()
    {
        foreach (WorldQuery query in queries)
        {
            query.Invalidate();
        }
    }

    /**
        * @brief Registers a callback that is invoked when the entity is destroyed.
        * @param entity The entity to observe.
        * @param callback The callback to invoke.
    */
    public void OnEntityDestroyed(Entity entity, Action<Entity> callback)
    {
        if (!entityDestroyedCallbacks.TryGetValue(entity.Id, out List<Action<Entity>>? callbacks))
        {
            callbacks = [];
            entityDestroyedCallbacks.Add(entity.Id, callbacks);
        }

        callbacks.Add(callback);
    }

    /**
        * @brief Clears the world, removing all entities, components, and queries.
    */
    public void Clear()
    {
        entities.Clear();
        entityLookup.Clear();
        freeEntityIds.Clear();
        componentPools.Clear();
        entityDestroyedCallbacks.Clear();

        nextEntityId = 1;

        InvalidateQueries();
    }

    internal void Shutdown()
    {
        Clear();
        queries.Clear();
    }
}
