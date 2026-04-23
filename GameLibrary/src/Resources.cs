using Raylib_cs;

namespace GameLibrary;

public static class Resources
{
    public static Dictionary<string, Texture2D> Sprites = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();
    public static Dictionary<string, Music> Musics = new Dictionary<string, Music>();
    public static Dictionary<string, Tilemap> Tilemaps = new Dictionary<string, Tilemap>();
    public static Dictionary<string, Font> Fonts = new Dictionary<string, Font>();

    public static void Load()
    {
        Console.WriteLine("Detecting game files:");
        foreach (string path in Directory.GetFiles(Game.IsWeb ? "/" : Game.Dir, "*", SearchOption.AllDirectories))
        {
            Console.WriteLine("    " + path);
        }
        foreach (string spritePath in Directory.GetFiles(Game.Dir + "sprite/", "*.png", SearchOption.AllDirectories))
        {
            Sprites.Add(Path.GetFileNameWithoutExtension(spritePath), Raylib.LoadTexture(spritePath));
        }
        foreach (string soundPath in Directory.GetFiles(Game.Dir + "sound/", "*", SearchOption.AllDirectories))
        {
            Sounds.Add(Path.GetFileNameWithoutExtension(soundPath), Raylib.LoadSound(soundPath));
        }
        foreach (string musicPath in Directory.GetFiles(Game.Dir + "music/", "*", SearchOption.AllDirectories))
        {
            Musics.Add(Path.GetFileNameWithoutExtension(musicPath), Raylib.LoadMusicStream(musicPath));
        }
        foreach (string tilemapPath in Directory.GetFiles(Game.Dir + "tilemap/", "*.json", SearchOption.AllDirectories))
        {
            Tilemaps.Add(Path.GetFileNameWithoutExtension(tilemapPath), new Tilemap(tilemapPath));
        }
        foreach (string fontPath in Directory.GetFiles(Game.Dir + "font/", "*.*", SearchOption.AllDirectories))
        {
            Font f = Raylib.LoadFont(fontPath);
            Raylib.SetTextureFilter(f.Texture, TextureFilter.Point);
            Fonts.Add(Path.GetFileNameWithoutExtension(fontPath), f);
        }
        Console.WriteLine("All game files loaded OK!");
    }
}