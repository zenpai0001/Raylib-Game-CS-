using Box2dNet.Interop;
using Raylib_cs;

namespace Raylib_Game_C_;

// Designated dumping ground for extension methods and math functions
public static class Utils
{
    public static Color ToRaylib(this b2HexColor color)
    {
        var c = color.ToDotNet();
        return new Color(c.R, c.G, c.B, c.A);
    }
}