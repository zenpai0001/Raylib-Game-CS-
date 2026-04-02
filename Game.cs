using Raylib_cs;
using System.Numerics;

namespace Raylib_Game_CS_;

public class Game
{
    public Player player = new Player();

    // STAThread is required if you deploy using NativeAOT on Windows - See https://github.com/raylib-cs/raylib-cs/issues/301
    [System.STAThread]
    public void Update()
    {
        //This is where the game updates every frame. Game loop calls this function.
        Raylib.DrawText("Player1", 10, 10, 20, Color.White);
        Raylib.DrawText("FPS: " +Raylib.GetFPS().ToString(), 10, 40, 20, Color.White);

        Raylib.DrawRectangle((int)player.hitbox2.X,(int)player.hitbox2.Y,100,20,Color.Blue);
        
        
        player.CollisionCheck(player);

    }
    public void GameStart()
    {
        Game game = new Game();
        Raylib.InitWindow(800, 480, "Hello World");

        Raylib.SetTargetFPS(60);

        Tilemap tilemap = new Tilemap("ldtkexample.json");
        
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.BeginMode2D(new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1));
            tilemap.Draw();
            player.Update();
            game.Update();
            Raylib.ClearBackground(Color.Black);
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
