using Raylib_cs;
using System;
using System.Numerics;
namespace Raylib_Game_C;

public class Game
{
    public Player player = new Player();

    // STAThread is required if you deploy using NativeAOT on Windows - See https://github.com/raylib-cs/raylib-cs/issues/301
    [System.STAThread]
    public void GameStart()
    {
        
        Raylib.InitWindow(800, 480, "Hello World");

        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            //Update function records player input and updates position. I'll need to workout a few things.
            player.Update();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Raylib.DrawCircleV(player.position, 20, Color.Green);

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