using Raylib_cs;

namespace GameLibrary;

// Immediate mode GUI functions, similar to raygui.
public static class ImGui
{
    private static Sound _buttonPress;
    private static Sound _buttonRelease;

    static ImGui()
    {
        _buttonPress = Resources.Sounds["button_press"];
        _buttonRelease = Resources.Sounds["button_release"];
    }
    
    public static bool Button(string label, int x, int y)
    {
        Rectangle rect = new Rectangle(x, y, 100, 30);
        bool hovered = Raylib.CheckCollisionPointRec(Game.GetCursorPos(), rect);
        bool pressed = hovered && Raylib.IsMouseButtonDown(MouseButton.Left);
        Color color = hovered ? (pressed ? Color.DarkGray : Color.LightGray) : Color.Gray;
        
        if (hovered && Raylib.IsMouseButtonPressed(MouseButton.Left)) Raylib.PlaySound(_buttonPress);
        
        Raylib.DrawRectangleRec(rect, color);
        Raylib.DrawText(label, x + 10, y + 10, 20, Color.White);

        if (hovered && Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            Raylib.PlaySound(_buttonRelease);
            return true;
        }

        return false;
    }
}