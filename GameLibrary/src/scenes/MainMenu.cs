using Raylib_cs;

namespace GameLibrary;

public class MainMenu : Scene
{
    public override void Update()
    {
        Raylib.ClearBackground(Color.Black);

        Raylib.DrawText("GAME TITLE", 100, 100, 50, Color.White);
        if (ImGui.Button("Play PhysicsTest", 100, 150))
        {
            Game.ActiveScene = new PhysicsTest(Resources.Tilemaps["ldtkexample"]);
        }

        if (ImGui.Button("Quit", 100, 200))
        {
            Raylib.CloseWindow();
        }
    }
}