using DoveCanvas.Abstract;
using System.Reflection;
using rlImGui_cs;
using Raylib_cs;

namespace DoveCanvas;

/**
    * @class SchedularService
    * @brief A service that manages and executes engine systems.
*/
[Service]
public class SchedularService : Singleton<SchedularService>
{
    private readonly List<ISimulationSystem> simulationSystems = [];
    private readonly List<IRenderSystem> renderSystems = [];
    private readonly List<IDebugSystem> debugSystems = [];

    private ProfilerService profiler = Services.Profiler;
    private SceneService scene = Services.Scene;

    /**
        * @brief Initializes the scheduler service.
        * @returns void
    */
    internal void Initialize()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            RegisterSystems(assembly);
        }

        simulationSystems.Sort((a, b) => a.priority.CompareTo(b.priority));
        renderSystems.Sort((a, b) => a.priority.CompareTo(b.priority));

#if DEBUG
        debugSystems.Sort((a, b) => a.priority.CompareTo(b.priority));
#endif

        profiler = Services.Profiler;
        scene = Services.Scene;
    }

    /**
     * @brief Registers all systems found in an assembly.
     * @param assembly The assembly to scan.
     */
    private void RegisterSystems(Assembly assembly)
    {
        Type[] types;

        try
        {
            types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            types = ex.Types.Where(type => type != null).Cast<Type>().ToArray();
        }

        foreach (Type type in types)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                continue;
            }

            if (typeof(ISimulationSystem).IsAssignableFrom(type))
            {
                simulationSystems.Add((ISimulationSystem)Activator.CreateInstance(type)!);
                continue;
            }

            if (typeof(IRenderSystem).IsAssignableFrom(type))
            {
                renderSystems.Add((IRenderSystem)Activator.CreateInstance(type)!);
                continue;
            }

#if DEBUG
            if (typeof(IDebugSystem).IsAssignableFrom(type))
            {
                debugSystems.Add((IDebugSystem)Activator.CreateInstance(type)!);
            }
#endif
        }
    }

    /**
        * @brief Updates all simulation systems.
        * @param dt Delta time in seconds.
    */
    internal void Tick()
    {
        float dt = Raylib.GetFrameTime();

        if (dt > 0.1f)
        {
            dt = 0.1f; // Clamp delta time to avoid large jumps
        }

        profiler.Begin("Frame", "Tick");

        foreach (ISimulationSystem system in simulationSystems)
        {
            string name = system.GetType().Name;

            profiler.Begin("Tick", name);

            system.Tick(dt);

            profiler.End("Tick", name);
        }

        scene.Tick(dt);

        profiler.End("Frame", "Tick");
    }

    /**
        * @brief Draws all render systems.
        * @returns void
    */
    internal void Draw()
    {
        profiler.Begin("Frame", "Draw");

        foreach (IRenderSystem system in renderSystems)
        {
            string name = system.GetType().Name;

            profiler.Begin("Draw", name);

            system.Draw();

            profiler.End("Draw", name);
        }

        scene.Draw();
        profiler.End("Frame", "Draw");
    }

    /**
        * @brief Draws all debug systems.
    */
    internal void DebugDraw()
    {
#if DEBUG
        profiler.Begin("Frame", "DebugDraw");

        foreach (IDebugSystem system in debugSystems)
        {
            string name = system.GetType().Name;

            profiler.Begin("DebugDraw", name);

            system.Draw();

            profiler.End("DebugDraw", name);
        }

        profiler.End("Frame", "DebugDraw");
#endif
    }
}
