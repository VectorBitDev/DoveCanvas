namespace DoveCanvas.Ui;

/**
 * @class UIConstraints
 * @brief Defines the layout constraints of a UI element.
*/
public class UIConstraints
{
    public UILength X = UILength.Left();
    public UILength Y = UILength.Top();

    public UILength Width = UILength.Pixels(0);
    public UILength Height = UILength.Pixels(0);
}
