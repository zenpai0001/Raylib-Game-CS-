using Raylib_cs;
using System;
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

      
    }
    public void GameStart()
    {
        Game game = new Game();
        Raylib.InitWindow(800, 480, "Hello World");

        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            game.Update();
            Raylib.BeginDrawing();
            player.Update();
            Raylib.ClearBackground(Color.Black);
            Raylib.DrawCircleV(player.playerPosition, 20, Color.Green);
            Raylib.DrawText("Use WAD to move the circle", 10, 10, 20, Color.White);

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