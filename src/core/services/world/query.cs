using System.Collections;

namespace DoveCanvas;

/**
    * @class WorldQuery
    * @brief Represents a cached query against the world.
*/
public sealed class WorldQuery : IEnumerable<Entity>
{
    private readonly WorldService world;

    internal readonly HashSet<Type> withTypes = [];
    internal readonly HashSet<Type> withoutTypes = [];

    private readonly List<Entity> entities = [];

    private bool dirty = true;

    /**
        * @brief Creates a new world query.
        * @param world The owning world.
    */
    internal WorldQuery(WorldService world)
    {
        this.world = world;
    }

    /**
        * @brief Requires entities to contain a component.
        * @return The current query.
    */
    public WorldQuery With<T>() where T : Component
    {
        if (withTypes.Add(typeof(T)))
        {
            dirty = true;
        }

        return this;
    }

    /**
        * @brief Excludes entities that contain a component.
        * @return The current query.
    */
    public WorldQuery Without<T>() where T : Component
    {
        if (withoutTypes.Add(typeof(T)))
        {
            dirty = true;
        }

        return this;
    }

    /**
        * @brief Returns every entity matching the query.
        * @return The cached entity list.
    */
    public IReadOnlyList<Entity> Fetch()
    {
        if (!dirty)
        {
            return entities;
        }

        Rebuild();

        return entities;
    }

    /**
        * @brief Marks the query as dirty.
    */
    internal void Invalidate()
    {
        dirty = true;
    }

    /**
        * @brief Rebuilds the cached entity list.
    */
    private void Rebuild()
    {
        dirty = false;

        entities.Clear();

        foreach (Entity entity in world.Entities)
        {
            if (!Matches(entity))
            {
                continue;
            }

            entities.Add(entity);
        }
    }

    /**
        * @brief Returns whether an entity satisfies this query.
        * @param entity The entity.
        * @return True if the entity matches.
    */
    private bool Matches(Entity entity)
    {
        foreach (Type type in withTypes)
        {
            if (!world.HasComponent(entity, type))
            {
                return false;
            }
        }

        foreach (Type type in withoutTypes)
        {
            if (world.HasComponent(entity, type))
            {
                return false;
            }
        }

        return true;
    }

    /**
        * @brief Returns an enumerator over the matching entities.
    */
    public IEnumerator<Entity> GetEnumerator()
    {
        return Fetch().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
