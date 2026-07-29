using ImGuiNET;
using Raylib_cs;
using DoveCanvas.Abstract;

namespace DoveCanvas;

#if DEBUG

/**
    * @class ProfilerDebugSystem
    * @brief A debug system that displays profiling information using ImGui.
*/
internal class ProfilerDebugSystem : IDebugSystem
{
    private KeyboardKey toggleKey = KeyboardKey.F1;
    private bool isVisible;

    private readonly List<ProfileInfo> cachedProfiles = [];
    private double lastRefreshTime;
    private const double RefreshRate = 0.10; // 100 ms

    private const int FrameHistorySize = 120;
    private readonly float[] frameHistory = new float[FrameHistorySize];
    private int frameHistoryIndex;

    /**
        * @brief Formats a duration in milliseconds or microseconds.
        * @param milliseconds The duration in milliseconds.
        * @return The formatted string.
    */
    private static string FormatTime(double milliseconds)
    {
        if (milliseconds < 1.0)
        {
            return $"{milliseconds * 1000.0:F1} µs";
        }

        return $"{milliseconds:F3} ms";
    }

    /**
        * @brief Draws the profiler debug window.
        * @return void
    */
    public override void Draw()
    {
        if (Raylib.IsKeyPressed(toggleKey))
            isVisible = !isVisible;

        if (!isVisible)
            return;

        double time = Raylib.GetTime();

        if (time - lastRefreshTime >= RefreshRate)
        {
            cachedProfiles.Clear();
            cachedProfiles.AddRange(Services.Profiler.GetProfiles());

            lastRefreshTime = time;
        }

        frameHistory[frameHistoryIndex] = Raylib.GetFrameTime() * 1000.0f;
        frameHistoryIndex++;

        if (frameHistoryIndex >= FrameHistorySize)
        {
            frameHistoryIndex = 0;
        }

        ImGui.SetNextWindowSize(new System.Numerics.Vector2(700, 450), ImGuiCond.FirstUseEver);

        if (!ImGui.Begin("Profiler", ref isVisible))
        {
            ImGui.End();
            return;
        }

        float frameMs = Raylib.GetFrameTime() * 1000.0f;

        ImGui.Text($"FPS: {Raylib.GetFPS()}");
        ImGui.SameLine(120);
        ImGui.Text($"{frameMs:F2} ms");

        ImGui.Checkbox("Debug View", ref Services.Window.IsRefDebugViewEnabled());


        ImGui.PlotLines(
            "##FrameTime",
            ref frameHistory[0],
            FrameHistorySize,
            frameHistoryIndex,
            null,
            0.0f,
            20.0f,
            new System.Numerics.Vector2(-1, 80));

        foreach (ProfileInfo profile in cachedProfiles)
        {
            ImGui.SeparatorText(profile.Name);

            foreach (SubProfileInfo sub in profile.SubProfiles)
            {
                ImGui.PushID(sub.Name);

                ImGui.AlignTextToFramePadding();
                ImGui.Text(sub.Name);

                ImGui.SameLine(220);

                ImGui.TextDisabled($"{sub.Percentage:F1}%");

                ImGui.SameLine(290);

                ImGui.ProgressBar(
                    (float)(sub.Percentage / 100.0f),
                    new System.Numerics.Vector2(160, 18));

                ImGui.SameLine();

                ImGui.Text(FormatTime(sub.Milliseconds));

                ImGui.PopID();
            }

            ImGui.Spacing();
        }

        ImGui.End();
    }
}


#endif
