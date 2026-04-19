using Raylib_cs;

namespace GameLibrary;

public static class Resources
{
    public static Dictionary<string, Texture2D> Sprites = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();
    public static Dictionary<string, Music> Musics = new Dictionary<string, Music>();
    public static Dictionary<string, Tilemap> Tilemaps = new Dictionary<string, Tilemap>();

    public static void Load()
    {
        foreach (string spritePath in Directory.GetFiles(Game.dir + "sprite/", "*.png", SearchOption.AllDirectories))
        {
            Sprites.Add(Path.GetFileNameWithoutExtension(spritePath), Raylib.LoadTexture(spritePath));
        }
        foreach (string soundPath in Directory.GetFiles(Game.dir + "sound/", "*", SearchOption.AllDirectories))
        {
            Sounds.Add(Path.GetFileNameWithoutExtension(soundPath), Raylib.LoadSound(soundPath));
        }
        foreach (string musicPath in Directory.GetFiles(Game.dir + "music/", "*", SearchOption.AllDirectories))
        {
            Musics.Add(Path.GetFileNameWithoutExtension(musicPath), Raylib.LoadMusicStream(musicPath));
        }
        foreach (string tilemapPath in Directory.GetFiles(Game.dir + "tilemap/", "*.json", SearchOption.AllDirectories))
        {
            Tilemaps.Add(Path.GetFileNameWithoutExtension(tilemapPath), new Tilemap(tilemapPath));
        }
    }
}