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
        _rootDirectory = AppContext.BaseDirectory;
        _assetsDirectory = Path.Combine(_rootDirectory, "Assets");
    }

    /**
        * @brief Gets the full path of an asset.
        * @param path Relative asset path.
        * @return Full asset path.
    */
    private string GetPath(string path)
    {
        return Path.Combine(_assetsDirectory, path);
    }

    /**
        * @brief Checks if an asset exists.
        * @param path Relative asset path.
        * @return True if the asset exists.
    */
    public bool Exists(string path)
    {
        return File.Exists(GetPath(path));
    }

    /**
        * @brief Loads an image resource.
        * @param path Relative image path.
        * @return Loaded image.
    */
    public Image LoadImage(string path)
    {
        return Raylib.LoadImage(GetPath(path));
    }

    /**
        * @brief Loads a texture resource.
        * @param path Relative texture path.
        * @return Loaded texture.
    */
    public Texture2D LoadTexture(string path)
    {
        return Raylib.LoadTexture(GetPath(path));
    }

    /**
        * @brief Loads a font resource.
        * @param path Relative font path.
        * @return Loaded font.
    */
    public Font LoadFont(string path, int size = 32)
    {
        return Raylib.LoadFontEx(GetPath(path), size, null, 0);
    }

    /**
        * @brief Loads an audio resource.
        * @param path Relative audio path.
        * @return Loaded sound.
    */
    public Sound LoadSound(string path)
    {
        return Raylib.LoadSound(GetPath(path));
    }

    /**
        * @brief Loads music resource.
        * @param path Relative music path.
        * @return Loaded music stream.
    */
    public Music LoadMusic(string path)
    {
        return Raylib.LoadMusicStream(GetPath(path));
    }

    /**
        * @brief Loads a 3D model resource.
        * @param path Relative model path.
        * @return Loaded model.
    */
    public Model LoadModel(string path)
    {
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
        return Raylib.LoadShader(
            GetPath(vertexPath),
            GetPath(fragmentPath)
        );
    }
}
