using Raylib_cs;
using System.Numerics;
using Raylib_Game_C_;

namespace Raylib_Game_CS_;

public class Game
{
    public Player player = new Player();
    public static Queue<Action> LateActions = new Queue<Action>(); // LateActions are dequeued and invoked after everything else has updated.
    public static Camera2D Camera2D = new Camera2D(new Vector2(100, 50), Vector2.Zero, 0, 2);

    // STAThread is required if you deploy using NativeAOT on Windows - See https://github.com/raylib-cs/raylib-cs/issues/301
    [System.STAThread]
    public void Update()
    {
        //This is where the game updates every frame. Game loop calls this function.
        Raylib.DrawText("FPS: " +Raylib.GetFPS().ToString(), 10, 40, 20, Color.White);
        // Raylib.DrawRectangle((int)player.hitbox2.X,(int)player.hitbox2.Y,100,20,Color.Blue);
        //
        // player.CollisionCheck(player);
    }
    
    public void GameStart()
    {
        Game game = new Game();
        
        Raylib.InitWindow(800, 600, "Hello World");
        Raylib.SetTargetFPS(60);

        
        Tilemap tilemap = new Tilemap("ldtkexample.json");
        PhysicsTest physicsTest = new PhysicsTest(tilemap);
        
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.BeginMode2D(Camera2D);
            Raylib.ClearBackground(Color.Black);
            
            tilemap.Draw();
            // player.Update();
            game.Update();
            physicsTest.Step();
            while (LateActions.Count > 0) LateActions.Dequeue().Invoke();
            Raylib.DrawCircleV(player.playerPosition, 20, Color.Green);
            
            Raylib.EndMode2D();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
    
    public static void Main()
    {
        Game game = new Game();
        
        game.GameStart();
    }
}
