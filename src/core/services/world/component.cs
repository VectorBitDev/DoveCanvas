namespace DoveCanvas;

/**
    * @class Entity
    * @brief Represents a unique entity in the world.
*/
public readonly struct Entity : IEquatable<Entity>
{
    /**
        * @brief Gets the entity identifier.
    */
    public readonly uint Id;

    /**
        * @brief Creates a new entity.
        * @param id The entity identifier.
    */
    internal Entity(uint id)
    {
        Id = id;
    }

    /**
        * @brief Returns whether this entity is valid.
    */
    public bool IsValid => Id != 0;

    public bool Equals(Entity other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (int)Id;
    }

    public override string ToString()
    {
        return $"Entity({Id})";
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !left.Equals(right);
    }
}

/**
    * @class Component
    * @brief Base class for all components.
*/
public abstract class Component
{
}
