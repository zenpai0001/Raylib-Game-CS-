using Raylib_cs;

namespace GameLibrary;

public static class Time
{
    public static double Scaled; // Seconds of game time since last reset. Reset happens at start of battle
    public static double Unscaled; // Seconds since last reset. Is not affected by pause or ffw, but is affected by lag.
    public static int Tick; // Number of updates since last reset.
    public static int Frame; // Number of frames since last reset. Equivalent to Unscaled.
    public static double Real; // Seconds since program started
    public const int FrameRate = 60;
    public const double DeltaTime = 1.0/FrameRate;
    public static bool Paused;
    
    public static void UpdateTime(bool fastForward = false)
    {
        if (!fastForward)
        {
            Frame++;
            Unscaled += DeltaTime;
        }
        if (!Paused)
        {
            Tick++;
            Scaled = Tick * DeltaTime;
        }
        Real = Raylib.GetTime();
    }

    public static void Reset()
    {
        Scaled = 0;
        Unscaled = 0;
        Tick = 0;
        Frame = 0;
    }
}