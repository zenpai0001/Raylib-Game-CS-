using Raylib_cs;

namespace GameLibrary;

public static class Resources
{
    public static Dictionary<string, Texture2D> Sprites = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();
    public static Dictionary<string, Music> Musics = new Dictionary<string, Music>();
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

        Fonts.Add("sd_auto_pilot", Raylib.LoadFontEx(Game.Dir + "font/sd_auto_pilot.ttf", 21, null, 0));
        Fonts.Add("arcadepix", Raylib.LoadFontEx(Game.Dir + "font/arcadepix.ttf", 10, null, 0));
        
        // Hilarious hack to unblur fonts, doesn't quite work.
        // Image fontImg = Raylib.LoadImageFromTexture(Fonts["arcadepix"].Texture);
        // Raylib.ImageFormat(ref fontImg, PixelFormat.UncompressedR8G8B8A8);
        // for (int x = 0; x < fontImg.Width; x++)
        // for (int y = 0; y < fontImg.Height; y++)
        // {
        //     int a = Raylib.GetImageColor(fontImg, x, y).A;
        //     if (a < 250) Raylib.ImageDrawPixel(ref fontImg, x, y, Color.Blank);
        // }
        // Raylib.ExportImage(fontImg, Game.Dir + "fontimg.png");
        //
        // unsafe
        // {
        //     Color *pixels = Raylib.LoadImageColors(fontImg);
        //     Raylib.UpdateTexture(Fonts["arcadepix"].Texture, pixels);
        //     Raylib.UnloadImageColors(pixels);
        // }
        // Raylib.UnloadImage(fontImg);
        
        Console.WriteLine("All game files loaded OK!");
    }
}