namespace DoveCanvas.Ui;

/**
 * @enum UILengthMode
 * @brief Defines how a layout value is interpreted.
*/
public enum UILengthMode
{
    // Value modes
    Percent,
    Pixels,
    Fill,

    // Alignment modes
    Left,
    Right,

    Top,
    Bottom,

    Center,
}

/**
 * @class UILength
 * @brief Represents a layout value.
*/
public class UILength
{
    public UILengthMode Mode;
    public float Value;

    private UILength(UILengthMode mode, float value = 0)
    {
        Mode = mode;
        Value = value;
    }

    public static UILength Pixels(float value) => new(UILengthMode.Pixels, value);
    public static UILength Percent(float value) => new(UILengthMode.Percent, value);

    public static UILength Left(float offset = 0) => new(UILengthMode.Left, offset);
    public static UILength Right(float offset = 0) => new(UILengthMode.Right, offset);

    public static UILength Top(float offset = 0) => new(UILengthMode.Top, offset);
    public static UILength Bottom(float offset = 0) => new(UILengthMode.Bottom, offset);

    public static UILength Center(float offset = 0) => new(UILengthMode.Center, offset);

    public static UILength Fill() => new(UILengthMode.Fill);
}
