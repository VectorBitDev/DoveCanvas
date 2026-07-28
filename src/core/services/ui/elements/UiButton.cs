using Raylib_cs;

namespace DoveCanvas.Ui;

/**
    * @class UIButton
    * @brief A button UI element that can be clicked and interacted with.
*/
public class UIButton : UIElement
{
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
