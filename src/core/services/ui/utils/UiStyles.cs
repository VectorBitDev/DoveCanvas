using Raylib_cs;

namespace DoveCanvas.Ui;

/**
 * @class UiStyles
 * @brief Defines the visual style of a UI element.
*/
public class UiStyles
{
    public Color Background = Color.Black;
    public Color Foreground = Color.Black;

    public Color BorderColor = Color.Blank;
    public float BorderThickness = 0.0f;

    public float CornerRadius = 0.0f;

    public int FontSize = 20;
    public int FontSpacing = 1;

    public Color TextColor = Color.White;
}
