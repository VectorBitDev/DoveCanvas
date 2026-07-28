using System.Numerics;
using Raylib_cs;

namespace DoveCanvas.Ui;

/**
 * @enum TextAlignment
 * @brief Defines the horizontal alignment of text within a UI element.
 */
public enum TextAlignment
{
    Left,
    Center,
    Right
}

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

    // Sprite rendering
    public Texture2D? SpriteTexture;

    // Scrollbar state
    public float ScrollValue = 0f;
    public float ScrollMax = 100f;
    public float ViewportSize = 100f;
    public bool IsVerticalScrollbar = true;

    // Text alignment
    public TextAlignment TextAlignment = TextAlignment.Left;

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
     * @brief Draws the UI element's background and border using Styles.
     * This is a helper method that derived classes can call.
     */
    protected void DrawBackgroundAndBorder()
    {
        if (Styles.CornerRadius > 0.0f)
        {
            if (Styles.Background.A > 0)
            {
                Raylib.DrawRectangleRounded(
                    Bounds,
                    Styles.CornerRadius,
                    8,
                    Styles.Background);
            }

            if (Styles.BorderThickness > 0)
            {
                Raylib.DrawRectangleRoundedLinesEx(
                    Bounds,
                    Styles.CornerRadius,
                    8,
                    Styles.BorderThickness,
                    Styles.BorderColor);
            }
        }
        else
        {
            if (Styles.Background.A > 0)
            {
                Raylib.DrawRectangleRec(Bounds, Styles.Background);
            }

            if (Styles.BorderThickness > 0)
            {
                Raylib.DrawRectangleLinesEx(
                    Bounds,
                    Styles.BorderThickness,
                    Styles.BorderColor);
            }
        }
    }

    /**
     * @brief Draws a sprite/texture within the element's bounds.
     * Uses SpriteTexture if set, tinted by Styles.Background.
     */
    protected void DrawSprite()
    {
        if (SpriteTexture.HasValue && SpriteTexture.Value.Id > 0)
        {
            Color tint = Styles.Background.A > 0 ? Styles.Background : Color.White;
            Raylib.DrawTexturePro(
                SpriteTexture.Value,
                new Rectangle(0, 0, SpriteTexture.Value.Width, SpriteTexture.Value.Height),
                Bounds,
                new Vector2(0, 0),
                0f,
                tint);
        }
    }

    /**
     * @brief Draws text using the element's Name as content and Styles for formatting.
     */
    protected void DrawText()
    {
        string text = Name.Value;
        if (string.IsNullOrEmpty(text))
            return;

        int fontSize = Styles.FontSize;
        int fontSpacing = Styles.FontSpacing;
        Color textColor = Styles.TextColor;

        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, fontSize, fontSpacing);

        float x = Bounds.X;
        float y = Bounds.Y + (Bounds.Height - textSize.Y) * 0.5f;

        switch (TextAlignment)
        {
            case TextAlignment.Left:
                x = Bounds.X + 4;
                break;
            case TextAlignment.Center:
                x = Bounds.X + (Bounds.Width - textSize.X) * 0.5f;
                break;
            case TextAlignment.Right:
                x = Bounds.X + Bounds.Width - textSize.X - 4;
                break;
        }

        Raylib.DrawTextEx(Raylib.GetFontDefault(), text, new Vector2(x, y), fontSize, fontSpacing, textColor);
    }

    /**
     * @brief Draws a scrollbar within the element's bounds.
     * Uses ScrollValue, ScrollMax, ViewportSize, and IsVerticalScrollbar for state.
     * Uses Styles for colors (Background=track, Foreground=thumb, BorderColor/BorderThickness=border).
     */
    protected void DrawScrollBar()
    {
        if (ViewportSize >= ScrollMax || ScrollMax <= 0)
            return;

        float trackSize = IsVerticalScrollbar ? Bounds.Height : Bounds.Width;
        float thumbSize = Math.Max(20f, trackSize * (ViewportSize / ScrollMax));
        float maxThumbPos = trackSize - thumbSize;
        float thumbPos = maxThumbPos * (ScrollValue / (ScrollMax - ViewportSize));
        thumbPos = Math.Clamp(thumbPos, 0, maxThumbPos);

        Rectangle trackRect;
        Rectangle thumbRect;

        if (IsVerticalScrollbar)
        {
            trackRect = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            thumbRect = new Rectangle(Bounds.X, Bounds.Y + thumbPos, Bounds.Width, thumbSize);
        }
        else
        {
            trackRect = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            thumbRect = new Rectangle(Bounds.X + thumbPos, Bounds.Y, thumbSize, Bounds.Height);
        }

        // Draw track
        if (Styles.Background.A > 0)
        {
            Raylib.DrawRectangleRec(trackRect, Styles.Background);
        }

        // Draw thumb
        if (Styles.Foreground.A > 0)
        {
            if (Styles.CornerRadius > 0)
            {
                Raylib.DrawRectangleRounded(thumbRect, Styles.CornerRadius, 8, Styles.Foreground);
            }
            else
            {
                Raylib.DrawRectangleRec(thumbRect, Styles.Foreground);
            }
        }

        // Draw border if needed
        if (Styles.BorderThickness > 0)
        {
            Raylib.DrawRectangleLinesEx(trackRect, Styles.BorderThickness, Styles.BorderColor);
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
