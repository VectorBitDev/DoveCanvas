using System.Numerics;
using Raylib_cs;

namespace DoveCanvas.Ui;

/**
 * @class UIElement
 * @brief Represents a UI element in the DoveCanvas framework.
*/
public abstract class UIElement
{
    public UIValue<string> Name = "";

    public readonly List<UIElement> Children = [];
    public UIElement? Parent;

    public UIConstraints Constraints = new();
    public UiStyles Styles = new();
    public UiEvents Events = new();

    public Rectangle Bounds;

    public UIValue<bool> Visible = true;
    public UIValue<bool> Enabled = true;

    private bool wasHovered = false;

    /**
     * @brief Adds a child UI element to this element.
     * @param child The child UI element to add.
    */
    public void Add(UIElement child)
    {
        child.Parent = this;
        Children.Add(child);
    }

    /**
     * @brief Removes a child UI element from this element.
     * @param child The child UI element to remove.
    */
    public void Remove(UIElement child)
    {
        child.Parent = null;
        Children.Remove(child);
    }

    /**
     * @brief Resolves the layout constraints of the UI element.
    */
    internal void Update()
    {
        if (!Visible || !Enabled)
        {
            return;
        }

        ResolveConstraints();
        UpdateEvents();

        foreach (UIElement child in Children)
        {
            child.Update();
        }
    }

    /**
     * @brief Resolves the element's layout constraints.
    */
    private void ResolveConstraints()
    {
        Rectangle parent = Parent?.Bounds ?? new Rectangle(
            0,
            0,
            Raylib.GetScreenWidth(),
            Raylib.GetScreenHeight());

        float width = ResolveWidth(Constraints.Width, parent);
        float height = ResolveHeight(Constraints.Height, parent);

        float x = ResolveX(Constraints.X, parent, width);
        float y = ResolveY(Constraints.Y, parent, height);

        Bounds = new Rectangle(x, y, width, height);
    }

    /**
     * @brief Resolves the width constraint.
    */
    private static float ResolveWidth(UILength constraint, Rectangle parent)
    {
        return constraint.Mode switch
        {
            UILengthMode.Pixels => constraint.Value,
            UILengthMode.Percent => parent.Width * (constraint.Value / 100.0f),
            UILengthMode.Fill => parent.Width,
            _ => constraint.Value
        };
    }

    /**
     * @brief Resolves the height constraint.
    */
    private static float ResolveHeight(UILength constraint, Rectangle parent)
    {
        return constraint.Mode switch
        {
            UILengthMode.Pixels => constraint.Value,
            UILengthMode.Percent => parent.Height * (constraint.Value / 100.0f),
            UILengthMode.Fill => parent.Height,
            _ => constraint.Value
        };
    }

    /**
     * @brief Resolves the horizontal position.
    */
    private static float ResolveX(UILength constraint, Rectangle parent, float width)
    {
        return constraint.Mode switch
        {
            UILengthMode.Left => parent.X + constraint.Value,
            UILengthMode.Right => parent.X + parent.Width - width - constraint.Value,
            UILengthMode.Center => parent.X + (parent.Width - width) * 0.5f + constraint.Value,
            UILengthMode.Pixels => parent.X + constraint.Value,
            UILengthMode.Percent => parent.X + parent.Width * (constraint.Value / 100.0f),
            _ => parent.X
        };
    }

    /**
     * @brief Resolves the vertical position.
    */
    private static float ResolveY(UILength constraint, Rectangle parent, float height)
    {
        return constraint.Mode switch
        {
            UILengthMode.Top => parent.Y + constraint.Value,
            UILengthMode.Bottom => parent.Y + parent.Height - height - constraint.Value,
            UILengthMode.Center => parent.Y + (parent.Height - height) * 0.5f + constraint.Value,
            UILengthMode.Pixels => parent.Y + constraint.Value,
            UILengthMode.Percent => parent.Y + parent.Height * (constraint.Value / 100.0f),
            _ => parent.Y
        };
    }

    /**
     * @brief Updates the element's input state and fires events.
    */
    private void UpdateEvents()
    {
        Vector2 mouse = Raylib.GetMousePosition();

        bool hovered = Raylib.CheckCollisionPointRec(mouse, Bounds);

        if (hovered == false && wasHovered)
        {
            wasHovered = false;
            Events.HoverExit?.Invoke();
        }

        if (hovered)
        {
            wasHovered = true;
            Events.HoverEnter?.Invoke();

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Events.MouseDown?.Invoke();
            }

            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                Events.MouseUp?.Invoke();
            }

            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                Events.Click?.Invoke();
            }
        }
    }

    /**
     * @brief Draws the UI element.
    */
    public virtual void Draw()
    {
        foreach (UIElement child in Children)
        {
            try
            {
                child.Draw();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error drawing UI element '{child.Name}': {e.Message}");
            }
        }
    }
}
