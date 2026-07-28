using Raylib_cs;

namespace DoveCanvas.Ui;

/**
 * @class UIScrollBar
 * @brief A scrollbar UI element for scrolling content.
 * Uses ScrollValue, ScrollMax, ViewportSize, and IsVerticalScrollbar for state.
 * Uses Styles for visual appearance (Background=track, Foreground=thumb, BorderColor/BorderThickness=border).
 */
public class UIScrollBar : UIElement
{
    /**
     * @brief Draws the scrollbar and its children.
    */
    public override void Draw()
    {
        if (!Visible)
        {
            return;
        }

        DrawScrollBar();
        base.Draw();
    }
}
