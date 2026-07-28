using System.Diagnostics;
using DoveCanvas.Abstract;

namespace DoveCanvas;

/**
    * @struct ProfileInfo
    * @brief Represents a profiler snapshot.
*/
public readonly struct ProfileInfo
{
    public readonly string Name;
    public readonly double TotalMilliseconds;
    public readonly IReadOnlyList<SubProfileInfo> SubProfiles;

    public ProfileInfo(
        string name,
        double totalMilliseconds,
        IReadOnlyList<SubProfileInfo> subProfiles)
    {
        Name = name;
        TotalMilliseconds = totalMilliseconds;
        SubProfiles = subProfiles;
    }
}

/**
    * @struct SubProfileInfo
    * @brief Represents a profiler sub profile.
*/
public readonly struct SubProfileInfo
{
    public readonly string Name;
    public readonly double Milliseconds;
    public readonly double Percentage;

    public SubProfileInfo(
        string name,
        double milliseconds,
        double percentage)
    {
        Name = name;
        Milliseconds = milliseconds;
        Percentage = percentage;
    }
}

/**
    * @class ProfilerService
    * @brief A service that manages profiling and performance metrics for the engine.
*/
public class ProfilerService : Singleton<ProfilerService>
{
    private sealed class SubProfile
    {
        public required Stopwatch Stopwatch;

        public double Milliseconds;
    }

    private sealed class Profile
    {
        public required Stopwatch Stopwatch;

        public readonly Dictionary<string, SubProfile> SubProfiles = [];
    }


    private readonly Dictionary<string, Profile> profiles = [];

    /**
        * @brief Begins a profile.
        * @param name The profile name.
    */
    public void Begin(string name)
    {
        if (!profiles.TryGetValue(name, out Profile? profile))
        {
            profile = new Profile()
            {
                Stopwatch = new Stopwatch()
            };

            profiles.Add(name, profile);
        }

        profile.Stopwatch.Restart();
    }

    /**
        * @brief Begins a sub profile.
        * @param profileName The profile name.
        * @param subProfileName The sub profile name.
    */
    public void Begin(string profileName, string subProfileName)
    {
        if (!profiles.TryGetValue(profileName, out Profile? profile))
        {
            Begin(profileName);
            profile = profiles[profileName];
        }

        if (!profile.SubProfiles.TryGetValue(subProfileName, out SubProfile? subProfile))
        {
            subProfile = new SubProfile()
            {
                Stopwatch = new Stopwatch()
            };

            profile.SubProfiles.Add(subProfileName, subProfile);
        }

        subProfile.Stopwatch.Restart();
    }

    /**
        * @brief Ends a profile.
        * @param name The profile name.
    */
    public void End(string name)
    {
        if (!profiles.TryGetValue(name, out Profile? profile))
        {
            return;
        }

        profile.Stopwatch.Stop();
    }

    /**
        * @brief Ends a sub profile.
        * @param profileName The profile name.
        * @param subProfileName The sub profile name.
    */
    public void End(string profileName, string subProfileName)
    {
        if (!profiles.TryGetValue(profileName, out Profile? profile))
        {
            return;
        }

        if (!profile.SubProfiles.TryGetValue(subProfileName, out SubProfile? subProfile))
        {
            return;
        }

        subProfile.Stopwatch.Stop();
        subProfile.Milliseconds = subProfile.Stopwatch.Elapsed.TotalMilliseconds;
    }

    /**
        * @brief Clears all profiler data.
    */
    public void Clear()
    {
        profiles.Clear();
    }

    /**
        * @brief Returns a snapshot of all profiles.
    */
    public IReadOnlyList<ProfileInfo> GetProfiles()
    {
        List<ProfileInfo> result = [];

        foreach ((string profileName, Profile profile) in profiles)
        {
            double total = profile.Stopwatch.Elapsed.TotalMilliseconds;

            List<SubProfileInfo> subProfiles = [];

            foreach ((string subName, SubProfile subProfile) in profile.SubProfiles)
            {
                double percent = total <= 0.0
                    ? 0.0
                    : subProfile.Milliseconds / total * 100.0;

                subProfiles.Add(new SubProfileInfo(
                    subName,
                    subProfile.Milliseconds,
                    percent));
            }

            result.Add(new ProfileInfo(
                profileName,
                total,
                subProfiles));
        }

        return result;
    }
}
