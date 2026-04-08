// using Box2dNet.Interop;
// using Raylib_cs;
//
// namespace Raylib_Game_C_;
//
// // Designated dumping ground for extension methods and math functions
// public static class Utils
// {
//     public static Color ToRaylib(this b2HexColor color)
//     {
//         var c = color.ToDotNet();
//         return new Color(c.R, c.G, c.B, c.A);
//     }
// }

using System.Numerics;
using Box2D;
using Box2D.NET;
using Raylib_cs;

namespace Raylib_Game_C_;

// Designated dumping ground for extension methods and math functions
public static class Utils
{
    public static Color ToRaylib(this B2HexColor color)
    {
        // var c = color;
        return Color.Red;
        // return new Color(c.R, c.G, c.B, c.A);
    }

    public static B2Vec2 ToB2(this Vector2 vec)
    {
        return new B2Vec2(vec.X, vec.Y);
    }

    public static Vector2 ToVec2(this B2Vec2 vec)
    {
        return new Vector2(vec.X, vec.Y);
    }
}