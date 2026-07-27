namespace DoveCanvas.Ui;

/**
 * @class UiEvents
 * @brief Stores callbacks for UI interaction events.
*/
public class UiEvents
{
    public Action? Click;
    public Action? DoubleClick;

    public Action? HoverEnter;
    public Action? HoverExit;

    public Action? MouseDown;
    public Action? MouseUp;

    public Action? Focus;
    public Action? Blur;
}
