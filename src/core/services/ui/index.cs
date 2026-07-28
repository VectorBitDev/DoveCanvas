using DoveCanvas.Abstract;
using DoveCanvas.Ui;
using DoveCanvas;
using Raylib_cs;

namespace DoveCanvas.Ui;

/**
    * @class UiService
    * @brief A service that provides functionality.
*/
public class UiService : Singleton<UiService>
{
    private UIElement Root { get; } = new UIPanel()
    {
        Styles = new UiStyles()
        {
            Background = Color.Blank,
        },

        Constraints = new UIConstraints()
        {
            Width = UILength.Fill(),
            Height = UILength.Fill(),
        },
    };

    /**
        * @brief Initializes the service.
        * @return The instance of the service.
    */
    internal void Initialize()
    {
    }

    /**
        * @brief Updates the Children UiElements.
    */
    internal void Update()
    {
        Services.Profiler.Begin("UI Update", "Root Update");
        Root.Update();
        Services.Profiler.End("UI Update", "Root Update");
    }

    /**
        * @brief Draws the Children UiElements.
    */
    internal void Draw()
    {
        Services.Profiler.Begin("UI Draw", "Root Draw");
        Root.Draw();
        Services.Profiler.End("UI Draw", "Root Draw");
    }

    /**
        * @brief Adds a UIElement to the Root.
        * @param element The UIElement to add.
    */
    public void Add(UIElement element)
    {
        Root.Add(element);
    }

    /**
        * @brief Removes a UIElement from the Root.
        * @param element The UIElement to remove.
    */
    public void Remove(UIElement element)
    {
        Root.Remove(element);
    }

    /**
        * @brief Clears all the Children UiElements.
    */
    public void Clear()
    {
        Root.Children.Clear();
    }
}
