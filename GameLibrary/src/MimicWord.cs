using System.Numerics;
using Raylib_cs;

namespace GameLibrary;

public class MimicPhrase
{
    public string SoundSourcePath;
    public List<(string Caption, float SoundStart)> Words;
    public float TotalDuration;

    public MimicPhrase(string soundSourcePath, List<(string Caption, float SoundStart)> words, float totalDuration)
    {
        SoundSourcePath = soundSourcePath;
        Words = words;
        TotalDuration = totalDuration;
    }

    public MimicWord GetWord(int index, Vector2 pos)
    {
        index = Math.Clamp(index, 0, Words.Count);
        string text = Words[index].Caption;
        float soundStart = Words[index].SoundStart;
        float soundDuration = TotalDuration - soundStart;
        if (index < Words.Count - 1) soundDuration = Words[index + 1].SoundStart - soundStart;
        return new MimicWord(pos, text, SoundSourcePath, soundStart, soundDuration);
    }

    public string GetCaptionForWord(int index)
    {
        return Words[index].Caption;
    }

    public int GetWordIndexAtTime(float time)
    {
        return Words.FindLastIndex(t => t.SoundStart < time);
    }
}

public class MimicWord
{
    public Vector2 Position;
    public string Text;
    public float TimeCreated;
    public Music SoundSource;
    public float SoundStart;
    public float SoundDuration;
    private bool _soundOver;
    private float VisualDuration;

    public MimicWord(Vector2 position, string text, string soundSourcePath, float soundStart, float soundDuration)
    {
        Position = position;
        Text = text;
        TimeCreated = (float)Time.Scaled;
        SoundSource = Raylib.LoadMusicStream(Game.Dir + $"music/{soundSourcePath}.mp3");
        SoundStart = soundStart;
        SoundDuration = soundDuration;
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