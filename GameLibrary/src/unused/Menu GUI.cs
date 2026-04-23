using Raylib_cs;

namespace GameLibrary;

public class Menu
{
    public static float MusicVolume;
    public static float SoundEffectVolume;

    // Immediate mode GUI functions, similar to raygui.
    public int x { get; set; }
    public int y { get; set; }


    public bool Button(int x, int y, string label)
    {
        var rect = new Rectangle(x, y, 50, 30);
        bool hovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect);
        var pressed = hovered && Raylib.IsMouseButtonDown(MouseButton.Left);
        var color = hovered ? pressed ? Color.DarkGray : Color.LightGray : Color.Gray;


        Raylib.DrawRectangleRec(rect, color);
        Raylib.DrawText(label, x + 10, y + 10, 12, Color.White);

        Raylib.SetTargetFPS(60);
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(Game.GetActiveCamera());

        if (pressed)
            Raylib.DrawText("Paused?", 100, 150, 50, Color.Red);
        else if (hovered) Raylib.DrawText("Pause button", 10, 10, 10, Color.White);
        return true;
        Raylib.EndMode2D();
        Raylib.EndDrawing();
    }

    public bool Slider(float MusicVolume, float SoundEffectVolume, int x, int y)
    {
        var musicRect = new Rectangle(x, y, 100, 20);
        var soundRect = new Rectangle(x, y + 30, 100, 20);
        Raylib.DrawRectangleRec(musicRect, Color.Gray);
        Raylib.DrawRectangleRec(soundRect, Color.Gray);
        
        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), musicRect) &&
            Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            MusicVolume = (Raylib.GetMouseX() - x) / 100f;
            MusicVolume = Math.Clamp(MusicVolume, 0, 1);
            Raylib.DrawText("Music Volume: " + (int)(MusicVolume * 100) + "%", x, y - 20, 12, Color.White);
        }
        else if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), soundRect) &&
                 Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            SoundEffectVolume = (Raylib.GetMouseX() - x) / 100f;
            SoundEffectVolume = Math.Clamp(SoundEffectVolume, 0, 1);
            Raylib.DrawText("Sound Effect Volume: " + (int)(SoundEffectVolume * 100) + "%", x, y + 10, 12, Color.White);
        }

        return true;
    }
}