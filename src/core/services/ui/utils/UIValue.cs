namespace DoveCanvas.Ui;

/**
 * @class UIValue
 * @brief Represents a value that can be bound to a UI element in the DoveCanvas framework.
 * @tparam T The type of the value.
 */
public class UIValue<T>
{
    private T value = default!;
    private Func<T>? getter;

    /**
     * @brief Gets or sets the value of the UIValue.
     * @returns The current value of the UIValue.
     */
    public T Value
    {
        get
        {
            if (getter != null)
            {
                return getter();
            }

            return value;
        }

        set
        {
            this.value = value;
            getter = null;
        }
    }

    /**
     * @brief Binds a getter function to the UIValue.
     * @param getter The function that returns the value of the UIValue.
     */
    public void Bind(Func<T> getter)
    {
        this.getter = getter;
    }

    /**
     * @brief Implicitly converts a UIValue to its underlying value type.
     * @param value The UIValue to convert.
     * @returns The underlying value of the UIValue.
     */
    public static implicit operator T(UIValue<T> value)
    {
        return value.Value;
    }

    /**
     * @brief Implicitly converts a value of type T to a UIValue.
     * @param value The value to convert.
     * @returns A new UIValue containing the specified value.
     */
    public static implicit operator UIValue<T>(T value)
    {
        return new UIValue<T> { Value = value };
    }
}
