namespace DoveCanvas.Abstract;

/**
 * @enum SystemPriority
 * @brief Represents the priority levels for systems in the DoveCanvas framework.
 */
public enum SystemPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Extreme = 3
}


/**
 * @class ISystem
 * @brief Represents a system in the DoveCanvas framework.
 */
public class ISystem
{
    /**
     * @brief The priority of the system.
     */
    internal SystemPriority priority = SystemPriority.Medium;
}

/**
 * @class ISimulationSystem
 * @brief Represents a simulation system in the DoveCanvas framework.
 */
public abstract class ISimulationSystem : ISystem
{
    /**
     * @brief Updates the simulation system.
     * @param dt The time delta since the last update.
     */
    public virtual void Tick(float dt) { }
}

/**
 * @class IRenderSystem
 * @brief Represents a render system in the DoveCanvas framework.
 */
public abstract class IRenderSystem : ISystem
{
    /**
     * @brief Draws the render system.
     */
    public virtual void Draw() { }
}

/**
 * @class IDebugSystem
 * @brief Represents a debug system in the DoveCanvas framework.
 */
internal abstract class IDebugSystem : ISystem
{
    /**
     * @brief Draws the debug system.
     */
    public virtual void Draw() { }
}
