using Raylib_cs;

namespace DoveCanvas.Ui;

/**
 * @class UIPanel
 * @brief A container that groups and renders child UI elements.
*/
public class UIPanel : UIElement
{
    /**
     * @brief Draws the panel and its children.
    */
    public override void Draw()
    {
        if (!Visible)
        {
            return;
        }

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

        base.Draw();
    }
}
