using System.Numerics;
using Raylib_cs;

namespace GameLibrary;

public class MimicPhrase
{
    public List<(string Caption, float SoundStart)> Words;
    public float TotalDuration;
    public Music MonsterMouth;
    public List<int> FavoriteWords;
    
    public MimicPhrase(Music monsterMouth, List<(string Caption, float SoundStart)> words, List<int> favoriteWords)
    {
        MonsterMouth = monsterMouth;
        MonsterMouth.Looping = false;
        Words = words;
        TotalDuration = Raylib.GetMusicTimeLength(MonsterMouth);
        FavoriteWords = favoriteWords;
    }
    
    public MimicWord GetWord(int index, Vector2 pos)
    {
        index = Math.Clamp(index, 0, Words.Count);
        string text = Words[index].Caption;
        float soundStart = MathF.Max(0.1f, Words[index].SoundStart);
        float soundDuration = TotalDuration - soundStart;
        if (index < Words.Count - 1) soundDuration = Words[index + 1].SoundStart - soundStart;
        return new MimicWord(pos, text, soundStart, soundDuration);
    }
    
    public string GetCaptionForWord(int index)
    {
        return Words[index].Caption;
    }
    
    public int GetWordIndexAtTime(float time)
    {
        return Math.Max(0, Words.FindLastIndex(t => t.SoundStart < time));
    }
}

public class MimicWord
{
    public Vector2 Position;
    public string Text;
    public float TimeCreated;
    public float SoundStart;
    public float SoundDuration;
    private float _visualDuration;
    private static Font _monsterFont;

    static MimicWord()
    {
        _monsterFont = Resources.Fonts["sd_auto_pilot"];
    }

    public MimicWord(Vector2 position, string text, float soundStart, float soundDuration)
    {
        Position = position;
        Text = text;
        TimeCreated = Time.Scaled;
        SoundStart = soundStart;
        SoundDuration = soundDuration;
        _visualDuration = 4;
    }

    // Updates position, plays audio, draws, returns true if this particle should be destroyed
    public bool Draw()
    {
        Position.Y -= 0.2f;
        float age = Time.Scaled - TimeCreated;
        
        Raylib.DrawTextEx(_monsterFont, Text, new Vector2((int)Position.X, (int)Position.Y), 20, 1, Raylib.ColorAlpha(Color.White, (_visualDuration - age) / _visualDuration));
        return age > _visualDuration;
    }
}

public class Pingas
{
    public Music SoundSource;
    public float StartTime;
    public float EndTime;
    public Action? EndAction;
    
    public Pingas(Music soundSource, float? startTime = null, float? duration = null, Action? endAction = null)
    {
        SoundSource = soundSource;
        StartTime = startTime ?? 0.1f;
        EndTime = startTime + duration ?? Raylib.GetMusicTimeLength(SoundSource);
        EndAction = endAction;
    }

    public Pingas Start()
    {
        SoundSource.Looping = false;
        Raylib.PlayMusicStream(SoundSource);
        Raylib.SeekMusicStream(SoundSource, StartTime);
        return this;
    }
    
    public bool Update()
    {
        Raylib.UpdateMusicStream(SoundSource);

        if (Raylib.GetMusicTimePlayed(SoundSource) >= EndTime || !Raylib.IsMusicStreamPlaying(SoundSource))
        {
            EndAction?.Invoke();
            return true;
        }
        return false;
    }
}