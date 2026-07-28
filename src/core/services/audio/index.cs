using Raylib_cs;
using DoveCanvas.Abstract;

namespace DoveCanvas;

/**
    * @class AudioService
    * @brief A service that provides functionality.
*/
public class AudioService : Singleton<AudioService>
{
    private bool _initialized;

    /**
        * @brief Initializes the service.
        * @return The instance of the service.
    */
    public void Initialize()
    {
        if (_initialized)
        {
            Logger.Info("AudioService is already initialized.");
            return;
        }

        Raylib_cs.Raylib.InitAudioDevice();
        _initialized = true;
        Logger.Info("AudioService initialized.");
    }

    /**
        * @brief Shuts down the service.
    */
    public void Shutdown()
    {
        if (!_initialized)
        {
            Logger.Info("AudioService is not initialized.");
            return;
        }

        Raylib_cs.Raylib.CloseAudioDevice();
        _initialized = false;
        Logger.Info("AudioService shutdown.");
    }

    /**
        * @brief Plays a sound.
        * @param soundPath The path to the sound file.
    */
    public Action? PlaySound(string soundPath)
    {
        if (!_initialized)
        {
            Logger.Warning("AudioService is not initialized.");
            return null;
        }

        var sound = Services.Resource.LoadSound(soundPath);
        Raylib.PlaySound(sound);

        return () =>
        {
            Raylib.StopSound(sound);
            Raylib.UnloadSound(sound);
        };
    }

    /**
        * @brief Plays music.
        * @param musicPath The path to the music file.
    */
    public Action? PlayMusic(string musicPath)
    {
        if (!_initialized)
        {
            Logger.Warning("AudioService is not initialized.");
            return null;
        }

        var music = Services.Resource.LoadMusic(musicPath);
        Raylib.PlayMusicStream(music);

        return () =>
        {
            Raylib.StopMusicStream(music);
            Raylib.UnloadMusicStream(music);
        };
    }

}
