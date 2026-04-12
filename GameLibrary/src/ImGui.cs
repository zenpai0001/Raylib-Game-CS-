using Raylib_cs;

// Immediate mode GUI functions, similar to raygui.
public static class ImGui
{
    private static Sound _buttonPress;
    private static Sound _buttonRelease;

    static ImGui()
    {
        _buttonPress = Raylib.LoadSound(Game.dir + "button_press.wav");
        _buttonRelease = Raylib.LoadSound(Game.dir + "button_release.wav");
    }
    
    public static bool Button(int x, int y, string label)
    {
        Rectangle rect = new Rectangle(x, y, 100, 30);
        bool hovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect);
        bool pressed = hovered && Raylib.IsMouseButtonDown(MouseButton.Left);
        Color color = hovered ? (pressed ? Color.DarkGray : Color.LightGray) : Color.Gray;
        
        if (hovered && Raylib.IsMouseButtonPressed(MouseButton.Left)) Raylib.PlaySound(_buttonPress);
        
        Raylib.DrawRectangleRec(rect, color);
        Raylib.DrawText(label, x + 10, y + 10, 12, Color.White);

        if (hovered && Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            Raylib.PlaySound(_buttonRelease);
            return true;
        }

        return false;
    }
}