using Raylib_cs;
using System.Numerics;

public static class Game
{
    public static string dir = "";
    public static Menu Menu = new Menu();
    public static Player Player = new Player();
    public static Queue<Action> LateActions = new Queue<Action>(); // LateActions are dequeued and invoked after everything else has updated.
    public static Camera2D Camera2D = new Camera2D(new Vector2(100, 50), Vector2.Zero, 0, 2);
    public static Tilemap Tilemap;
    public static PhysicsTest PhysicsTest;

    [STAThread]
    public static void Load()
    {
        Raylib.InitWindow(800, 600, "Hello World");
        Raylib.SetTargetFPS(60);
        Raylib.InitAudioDevice();
        
        Tilemap = new Tilemap("ldtkexample.json");
        PhysicsTest = new PhysicsTest(Tilemap);
        
        Menu.GamePaused += (sender, e) => {
            // Handle game paused event here
            Raylib.DrawText("Game Paused!", 10, 50, 20, Color.Red);
        };
    }
    
    [STAThread]
    public static void Update()
    {
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(Camera2D);
        Raylib.ClearBackground(Color.Black);
        
        //Inserting Gui logic here.
        Raylib.SetMouseCursor(MouseCursor.Crosshair);

        //Ending Gui logic here.
        
        Tilemap.Draw();
        Player.Update();
        PhysicsTest.Step();
        while (LateActions.Count > 0) LateActions.Dequeue().Invoke();
        Raylib.DrawCircleV(Player.playerPosition, 20, Color.Green);

        Raylib.EndMode2D();
        
        if (ImGui.Button(0, 0, "Clear"))
        {
            PhysicsTest.ClearBalls();
        }
        
        Raylib.DrawText("FPS: " + Raylib.GetFPS().ToString(), 10, 40, 12, Color.White);
        Raylib.EndDrawing();
    }
}
