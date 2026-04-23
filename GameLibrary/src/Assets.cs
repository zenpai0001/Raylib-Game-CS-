using Raylib_cs;

namespace GameLibrary;

// Game data assets (things like enemy stats, level names, etc.) can just be hard coded constants in this file. 
public static class Assets
{
    public static List<KeyboardKey> UsableKeys = new List<KeyboardKey>()
    {
        KeyboardKey.A,
        KeyboardKey.B,
        KeyboardKey.C,
        KeyboardKey.D,
        KeyboardKey.E,
        KeyboardKey.F,
        KeyboardKey.G,
        KeyboardKey.H,
        KeyboardKey.I,
        KeyboardKey.J,
        KeyboardKey.K,
        KeyboardKey.L,
        KeyboardKey.M,
        KeyboardKey.N,
        KeyboardKey.O,
        KeyboardKey.P,
        KeyboardKey.Q,
        KeyboardKey.R,
        KeyboardKey.S,
        KeyboardKey.T,
        KeyboardKey.U,
        KeyboardKey.V,
        KeyboardKey.W,
        KeyboardKey.X,
        KeyboardKey.Y,
        KeyboardKey.Z
    };
    
    // Demo phrase data
    // private MimicPhrase _mimicPhrase = new MimicPhrase("demo_monster",
    //     new List<(string Caption, float SoundStart)>()
    //     {
    //         ( "I'm",  0.07f ),
    //         ( "vent", 0.83f ),
    //         ( "OK",   1.72f ),
    //         ( "am",   2.77f ),
    //         ( "fix",  3.81f )
    //     }, 4.21f);
    
    public static MimicPhrase MimicPhrase = new MimicPhrase("source1", new List<(string Caption, float SoundStart)>()
    {
        ( "yeah", 0.07f ),
        ( "something", 0.714f ),
        ( "broke", 1.108f ),
        ( "the", 1.314f ),
        ( "vent", 1.478f ),
        ( "no", 2.356f ),
        ( "I", 3.046f ),
        ( "can", 3.185f ),
        ( "fix", 3.309f ),
        ( "it", 3.355f ),
        ( "myself", 3.651f ),
        ( "alright", 4.908f ),
        ( "I'm", 5.708f ),
        ( "goingto", 5.850f ),
        ( "be", 6.102f ),
        ( "back", 6.243f ),
        ( "inna", 6.480f ),
        ( "minute", 6.634f )
    }, 6.98f);
}