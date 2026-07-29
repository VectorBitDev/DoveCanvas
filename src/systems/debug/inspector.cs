using DoveCanvas.Abstract;
using System.Reflection;
using System.Numerics;
using Raylib_cs;
using ImGuiNET;

namespace DoveCanvas;

#if DEBUG

/**
    * @class InspectorDebugSystem
    * @brief A debug system that displays an inspector window using ImGui.
*/
internal class InspectorDebugSystem : IDebugSystem
{
    private KeyboardKey toggleKey = KeyboardKey.F2;
    private bool isVisible;

    private Entity? selectedEntity;

    private string search = "";
    private string withSearch = "";
    private string withoutSearch = "";

    private readonly List<Entity> filteredEntities = [];

    /**
        * @brief Draws the inspector debug window.
        * @return void
    */
    public override void Draw()
    {
        if (Raylib.IsKeyPressed(toggleKey))
        {
            isVisible = !isVisible;
        }

        if (!isVisible)
        {
            return;
        }

        ImGui.SetNextWindowSize(
            new System.Numerics.Vector2(900, 600),
            ImGuiCond.FirstUseEver);

        if (!ImGui.Begin("World Inspector", ref isVisible))
        {
            ImGui.End();
            return;
        }

        ImGui.Columns(2, "InspectorColumns");

        DrawEntityPanel();

        ImGui.NextColumn();

        DrawEntityInspector();

        ImGui.Columns(1);

        ImGui.End();
    }

    /**
        * @brief Draws a component editor using reflection.
        * @param entity The entity.
        * @param component The component.
    */
    private void DrawComponent(Entity entity, Component component)
    {
        Type type = component.GetType();

        ImGui.PushID(type.Name);

        if (ImGui.CollapsingHeader(
            type.Name,
            ImGuiTreeNodeFlags.DefaultOpen))
        {
            foreach (FieldInfo field in type.GetFields(
                BindingFlags.Public |
                BindingFlags.Instance))
            {
                DrawField(component, field);
            }

            ImGui.Spacing();

            if (ImGui.Button("Remove"))
            {
                Services.World.RemoveComponent(entity, type);
            }
        }

        ImGui.PopID();
    }

    /**
        * @brief Draws a component field.
        * @param instance The component instance.
        * @param field The field to edit.
    */
    private void DrawField(object instance, FieldInfo field)
    {
        object? value = field.GetValue(instance);

        if (value == null)
        {
            return;
        }

        Type type = field.FieldType;

        if (type == typeof(float))
        {
            float number = (float)value;

            if (ImGui.DragFloat(field.Name, ref number))
            {
                field.SetValue(instance, number);
            }

            return;
        }

        if (type == typeof(int))
        {
            int number = (int)value;

            if (ImGui.DragInt(field.Name, ref number))
            {
                field.SetValue(instance, number);
            }

            return;
        }

        if (type == typeof(bool))
        {
            bool enabled = (bool)value;

            if (ImGui.Checkbox(field.Name, ref enabled))
            {
                field.SetValue(instance, enabled);
            }

            return;
        }

        if (type == typeof(string))
        {
            string text = (string)value;

            if (ImGui.InputText(field.Name, ref text, 256))
            {
                field.SetValue(instance, text);
            }

            return;
        }

        if (type == typeof(Vector2))
        {
            Vector2 vector = (Vector2)value;

            if (ImGui.DragFloat2(
                field.Name,
                ref vector))
            {
                field.SetValue(instance, vector);
            }

            return;
        }

        if (type == typeof(Vector3))
        {
            Vector3 vector = (Vector3)value;

            if (ImGui.DragFloat3(
                field.Name,
                ref vector))
            {
                field.SetValue(instance, vector);
            }

            return;
        }

        if (type == typeof(Color))
        {
            Color color = (Color)value;

            Vector4 vector = new(
                color.R / 255.0f,
                color.G / 255.0f,
                color.B / 255.0f,
                color.A / 255.0f);

            if (ImGui.ColorEdit4(field.Name, ref vector))
            {
                color = new Color(
                    (byte)(vector.X * 255),
                    (byte)(vector.Y * 255),
                    (byte)(vector.Z * 255),
                    (byte)(vector.W * 255));

                field.SetValue(instance, color);
            }

            return;
        }
    }

    /**
        * @brief Draws the entity list panel.
    */
    private void DrawEntityPanel()
    {
        if (ImGui.Button("+ Entity"))
        {
            selectedEntity = Services.World.CreateEntity();
        }

        ImGui.InputText(
            "Search",
            ref search,
            64);

        ImGui.InputText(
            "With",
            ref withSearch,
            64);

        ImGui.InputText(
            "Without",
            ref withoutSearch,
            64);

        ImGui.Separator();

        RefreshEntities();

        foreach (Entity entity in filteredEntities)
        {
            bool selected =
                selectedEntity.HasValue &&
                selectedEntity.Value.Id == entity.Id;

            if (ImGui.Selectable(
                $"Entity {entity.Id}",
                selected))
            {
                selectedEntity = entity;
            }
        }
    }

    /**
        * @brief Draws the selected entity inspector.
    */
    private void DrawEntityInspector()
    {
        if (!selectedEntity.HasValue)
        {
            ImGui.TextDisabled("No entity selected.");
            return;
        }

        Entity entity = selectedEntity.Value;

        ImGui.Text($"Entity {entity.Id}");

        ImGui.SameLine();

        if (ImGui.Button("Delete"))
        {
            Services.World.DestroyEntity(entity);
            selectedEntity = null;
            return;
        }

        ImGui.Separator();

        ImGui.Text("Components");

        foreach (Component component in Services.World.GetComponents(entity))
        {
            DrawComponent(entity, component);
        }

        if (ImGui.Button("+ Add Component"))
        {
            ImGui.OpenPopup("AddComponent");
        }

        DrawAddComponentPopup(entity);
    }

    /**
        * @brief Draws the add component popup.
        * @param entity The entity to add a component to.
    */
    private void DrawAddComponentPopup(Entity entity)
    {
        if (!ImGui.BeginPopup("AddComponent"))
        {
            return;
        }

        foreach (Type type in Services.World.GetComponentTypes())
        {
            if (Services.World.HasComponent(entity, type))
            {
                continue;
            }

            if (ImGui.Selectable(type.Name))
            {
                Services.World.AddComponent(entity, type);
                ImGui.CloseCurrentPopup();
                break;
            }
        }

        ImGui.EndPopup();
    }

    /**
        * @brief Refreshes the list of filtered entities based on the current search criteria.
    */
    private void RefreshEntities()
    {
        filteredEntities.Clear();

        foreach (Entity entity in Services.World.Entities)
        {
            if (!Matches(entity))
            {
                continue;
            }

            filteredEntities.Add(entity);
        }
    }

    /**
        * @brief Checks if an entity matches the current search criteria.
        * @param entity The entity to check.
        * @return True if the entity matches, false otherwise.
    */
    private bool Matches(Entity entity)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            if (!entity.Id.ToString().Contains(search))
            {
                return false;
            }
        }

        foreach (Type type in Services.World.GetComponentTypes())
        {
            if (!string.IsNullOrWhiteSpace(withSearch))
            {
                if (type.Name.Contains(withSearch, StringComparison.OrdinalIgnoreCase))
                {
                    if (!Services.World.HasComponent(entity, type))
                    {
                        return false;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(withoutSearch))
            {
                if (type.Name.Contains(withoutSearch, StringComparison.OrdinalIgnoreCase))
                {
                    if (Services.World.HasComponent(entity, type))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}

#endif
