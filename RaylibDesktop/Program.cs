using GameLibrary;
using Raylib_cs;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Game.Dir = Directory.GetCurrentDirectory() + "/resource/";
        
        Game.Load();
        
        while (!Raylib.WindowShouldClose())
        {
            Game.Update();
        }

        Raylib.CloseWindow();
    }
}