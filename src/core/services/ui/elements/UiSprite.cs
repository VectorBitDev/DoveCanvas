using Raylib_cs;

namespace DoveCanvas.Ui;

/**
 * @class UISprite
 * @brief A UI element that draws a sprite/texture within its bounds.
 */
public class UISprite : UIElement
{
    /**
     * @brief Draws the sprite and its children.
    */
    public override void Draw()
    {
        if (!Visible)
        {
            return;
        }

        DrawSprite();
        base.Draw();
    }
}
