using System.Numerics;
using Raylib_cs;

namespace GameLibrary;

public class MimicWord
{
    public Vector2 Position;
    public float X => Position.X;
    public float Y => Position.Y;
    public string Text;
    public float TimeCreated;
    public Music SoundSource;
    public float SoundStart;
    public float SoundDuration;
    private bool _soundOver;
    private float VisualDuration;

    public MimicWord(Vector2 position, MimicWordTemplate template)
    {
        Position = position;
        Text = template.Caption;
        TimeCreated = (float)Time.Scaled;
        SoundSource = Raylib.LoadMusicStream(Game.Dir + $"music/{template.SoundSourcePath}.mp3");
        SoundStart = template.SoundStart;
        SoundDuration = template.SoundDuration;
        VisualDuration = Math.Max(4, SoundDuration);
        Raylib.PlayMusicStream(SoundSource);
        Raylib.SeekMusicStream(SoundSource, SoundStart);
    }

    // Updates position, plays audio, draws, returns true if this particle should be destroyed
    public bool Draw()
    {
        Position.Y -= 0.2f;
        float age = (float)Time.Scaled - TimeCreated;

        if (!_soundOver)
        {
            Raylib.UpdateMusicStream(SoundSource);

            if (Raylib.IsMusicStreamPlaying(SoundSource) && Raylib.GetMusicTimePlayed(SoundSource) > SoundDuration + SoundStart)
            {
                Raylib.StopMusicStream(SoundSource);
                Raylib.UnloadMusicStream(SoundSource);
                _soundOver = true;
            }
        }
        
        Raylib.DrawText(Text, (int)Position.X, (int)Position.Y, 20, Raylib.ColorAlpha(Color.White, (VisualDuration - age) / VisualDuration));
        return age > VisualDuration;
    }
}