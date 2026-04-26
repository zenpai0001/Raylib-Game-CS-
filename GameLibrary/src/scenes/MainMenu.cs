using System.Numerics;
using Raylib_cs;

namespace GameLibrary;

public class MainMenu : Scene
{
    private Music _menuMusic = Resources.Musics["menu_theme"];
    private Pingas _pingas;

    private List<Font> _testFonts = new List<Font>();
    
    public MainMenu()
    {
        Raylib.SetMusicVolume(_menuMusic, 0.5f);
        Raylib.PlayMusicStream(_menuMusic);
    }
    
    public override void Update()
    {
        if (_pingas?.Update() ?? false) _pingas = null;

        Raylib.UpdateMusicStream(_menuMusic);
        
        Raylib.ClearBackground(Color.Black);
        
        Raylib.DrawTexture(Resources.Sprites["titlebg"], 0, 0, Color.White);
        Raylib.DrawTexture(Resources.Sprites["titletext"], 88, 62 + (int)(Math.Sin(Time.Scaled) * 4), Color.White);
        
        if ((Time.Scaled/2) % 1 < 0.5f) {ImGui.DrawText("Press '1' to start", 95, 220, 10);}
        
        if (Raylib.IsKeyPressed(KeyboardKey.One))
        {
            Game.ActiveScene = new IntroCutscene();
        }

        if (Raylib.IsKeyPressed(KeyboardKey.P))
        {
            _pingas = Assets.Pingas[Random.Shared.Next(Assets.Pingas.Count)];
            _pingas.Start();
        }
    }
}