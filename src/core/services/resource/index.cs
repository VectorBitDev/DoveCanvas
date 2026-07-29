using Raylib_cs;
using DoveCanvas.Abstract;

namespace DoveCanvas;

/**
    * @class ResourceService
    * @brief A service that provides functionality for loading engine resources.
*/
public class ResourceService : Singleton<ResourceService>
{
    private string _rootDirectory = string.Empty;
    private string _assetsDirectory = string.Empty;

    /**
        * @brief Initializes the resource service.
    */
    public void Initialize()
    {
        FindProjectRoot();
    }

    /**
        * @brief Finds the project root directory by searching for a .csproj file.
    */
    private void FindProjectRoot()
    {
        DirectoryInfo? directory = new(AppContext.BaseDirectory);

        while (directory != null)
        {
            if (directory.GetFiles("*.csproj").Length > 0)
            {
                _rootDirectory = directory.FullName;
                return;
            }

            directory = directory.Parent;
        }

        _rootDirectory = AppContext.BaseDirectory;
    }

    /**
        * @brief Gets the full path of an asset.
        * @param path Relative asset path.
        * @return Full asset path.
    */
    private string GetPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Logger.Error("Path is null or empty.");
            return string.Empty;
        }
        return Path.Combine(_rootDirectory, path);
    }

    /**
        * @brief Checks if an asset exists.
        * @param path Relative asset path.
        * @return True if the asset exists.
    */
    public bool Exists(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Logger.Error("Path is null or empty.");
            return false;
        }
        return File.Exists(GetPath(path));
    }

    /**
        * @brief Loads an image resource.
        * @param path Relative image path.
        * @return Loaded image.
    */
    public Image LoadImage(string path)
    {
        if (!Exists(path))
        {
            Logger.Error($"Image file '{path}' does not exist.");
            return default;
        }
        return Raylib.LoadImage(GetPath(path));
    }

    /**
        * @brief Loads a texture resource.
        * @param path Relative texture path.
        * @return Loaded texture.
    */
    public Texture2D LoadTexture(string path)
    {
        if (!Exists(path))
        {
            Logger.Error($"Texture file '{path}' does not exist.");
            return default;
        }
        return Raylib.LoadTexture(GetPath(path));
    }

    /**
        * @brief Loads a font resource.
        * @param path Relative font path.
        * @return Loaded font.
    */
    public Font LoadFont(string path, int size = 32)
    {
        if (!Exists(path))
        {
            Logger.Error($"Font file '{path}' does not exist.");
            return default;
        }
        return Raylib.LoadFontEx(GetPath(path), size, null, 0);
    }

    /**
        * @brief Loads an audio resource.
        * @param path Relative audio path.
        * @return Loaded sound.
    */
    public Sound LoadSound(string path)
    {
        if (!Exists(path))
        {
            Logger.Error($"Sound file '{path}' does not exist.");
            return default;
        }
        return Raylib.LoadSound(GetPath(path));
    }

    /**
        * @brief Loads music resource.
        * @param path Relative music path.
        * @return Loaded music stream.
    */
    public Music LoadMusic(string path)
    {
        if (!Exists(path))
        {
            Logger.Error($"Music file '{path}' does not exist.");
            return default;
        }
        return Raylib.LoadMusicStream(GetPath(path));
    }

    /**
        * @brief Loads a 3D model resource.
        * @param path Relative model path.
        * @return Loaded model.
    */
    public Model? LoadModel(string path)
    {
        if (!Exists(path))
        {
            Logger.Error($"Model file '{path}' does not exist.");
            return null;
        }
        return Raylib.LoadModel(GetPath(path));
    }

    /**
        * @brief Loads a shader resource.
        * @param vertexPath Relative vertex shader path.
        * @param fragmentPath Relative fragment shader path.
        * @return Loaded shader.
    */
    public Shader LoadShader(string vertexPath, string fragmentPath)
    {
        if (!Exists(vertexPath))
        {
            Logger.Error($"Vertex shader file '{vertexPath}' does not exist.");
            return default;
        }
        return Raylib.LoadShader(
            GetPath(vertexPath),
            GetPath(fragmentPath)
        );
    }
}
