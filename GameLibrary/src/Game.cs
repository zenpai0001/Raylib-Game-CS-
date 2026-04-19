using Raylib_cs;
using System.Numerics;

namespace GameLibrary;

public static class Game
{
    public static string dir = "";
    public static Menu Menu = new Menu();
    public static Player Player = new Player();
    public static Queue<Action> LateActions = new Queue<Action>(); // LateActions are dequeued and invoked after everything else has updated.
    public static Camera2D Camera2D = new Camera2D(new Vector2(0, 0), Vector2.Zero, 0, 2);
    public static Scene ActiveScene;

    public static void Load()
    {
        Raylib.InitWindow(800, 600, "Hello World");
        Raylib.SetTargetFPS(60);
        Raylib.InitAudioDevice();
        Raylib.SetExitKey(KeyboardKey.Null);
        
        Resources.Load();
        
        ActiveScene = new MainMenu();
    }
    
    public static void Update()
    {
        Raylib.BeginDrawing();
        
        ActiveScene.Update();
        
        while (LateActions.Count > 0) LateActions.Dequeue().Invoke();
        
        Raylib.DrawText("FPS: " + Raylib.GetFPS(), 10, 40, 20, Color.White);
        Raylib.EndDrawing();
    }
}
