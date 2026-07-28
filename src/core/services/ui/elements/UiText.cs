using Raylib_cs;

namespace DoveCanvas.Ui;

/**
 * @class UIText
 * @brief A UI element that draws text within its bounds.
 * Uses Name.Value for the text content.
 * Uses Styles.TextColor, Styles.FontSize, Styles.FontSpacing for text appearance.
 * Uses TextAlignment for horizontal alignment.
 */
public class UIText : UIElement
{
    /**
     * @brief Draws the text and its children.
    */
    public override void Draw()
    {
        if (!Visible)
        {
            return;
        }

        DrawText();
        base.Draw();
    }
}
