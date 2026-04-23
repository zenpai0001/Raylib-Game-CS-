using Raylib_cs;

namespace GameLibrary;

public class MainMenu : Scene
{
    private Music _menuMusic = Resources.Musics["menu_theme"];
    
    public MainMenu()
    {
        Raylib.PlayMusicStream(_menuMusic);
    }
    
    public override void Update()
    {
        Raylib.UpdateMusicStream(_menuMusic);
        
        Raylib.ClearBackground(Color.Black);
        
        Raylib.DrawTexture(Resources.Sprites["menubg"], 0, 0, Color.White);
        
        Raylib.DrawText("GAME TITLE", 10, 10, 50, Color.White);
        if (ImGui.Button("Play", 100, 100))
        {
            Game.ActiveScene = new GameScene();
        }
        
        if (ImGui.Button("Play PhysicsTest", 100, 150))
        {
            Game.ActiveScene = new PhysicsTest(Resources.Tilemaps["ldtkexample"]);
        }

        if (ImGui.Button("Quit", 100, 200))
        {
            Game.ShouldQuit = true;
        }
        
    }
}