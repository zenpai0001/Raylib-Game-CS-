using Raylib_cs;
using System;
using System.Numerics;
using ldtk;
using Raylib_Game_C_;

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
        
        LdtkJson ldtkJson = LdtkJson.FromJson(File.ReadAllText(Directory.GetCurrentDirectory() + "/resource/ldtkexample.json"));
        
        List<Color> Colors = [
            Color.Red, Color.Blue, Color.DarkGreen, Color.Yellow, Color.Pink, Color.Orange, Color.RayWhite, Color.LightGray, Color.Gray, Color.DarkGray,
            Color.Black, Color.Brown, Color.DarkBlue, Color.Green, Color.SkyBlue, Color.Maroon, Color.Gold, Color.DarkPurple, Color.DarkBrown];

        Texture2D tileBgTex = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "/resource/Inca_back2_by_Kronbits.png");
        Texture2D tileTex = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "/resource/Inca_front_by_Kronbits-extended.png");
        
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.BeginMode2D(new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1));
            
            foreach (TileInstance? tile in ldtkJson.Levels[0].LayerInstances[1].AutoLayerTiles)
            {
                int tID = (int)tile.T;
                int columns = tileBgTex.Width / 16;
                Raylib.DrawTextureRec(tileBgTex, new Rectangle((tID % columns) * 16, (tID / columns) * 16, 16, 16), new Vector2((int)tile.Px[0], (int)tile.Px[1]), Color.White);
                // Raylib.DrawRectangle((int)tile.Px[0], (int)tile.Px[1], 16, 16, Colors[(int)tile.T % Colors.Count]);
            }
            
            foreach (TileInstance? tile in ldtkJson.Levels[0].LayerInstances[0].AutoLayerTiles)
            {
                int tID = (int)tile.T;
                int columns = tileTex.Width / 16;
                Raylib.DrawTextureRec(tileTex, new Rectangle((tID % columns) * 16, (tID / columns) * 16, 16, 16), new Vector2((int)tile.Px[0], (int)tile.Px[1]), Color.White);
                // Raylib.DrawRectangle((int)tile.Px[0], (int)tile.Px[1], 16, 16, Colors[(int)tile.T % Colors.Count]);
            }
            
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
