using System.Numerics;
using Raylib_cs;

namespace GameLibrary;

public class GameScene : Scene
{
    private float _playerAnimStart;
    private List<MimicWord> _speechParticles = new();
    private Music _dialogue;

    private List<MimicWordTemplate> _wordTemplates = new List<MimicWordTemplate>()
    {
        new MimicWordTemplate("I'm", 0.07f, 0.52f, "demo_monster"),
        new MimicWordTemplate("vent", 0.83f, 0.40f, "demo_monster"),
        new MimicWordTemplate("OK", 1.72f, 0.56f, "demo_monster"),
        new MimicWordTemplate("am", 2.77f, 0.60f, "demo_monster"),
        new MimicWordTemplate("fix", 3.81f, 0.40f, "demo_monster")
    };
    
    public GameScene()
    {
        Game.MusicPlaying = Resources.Musics["ambience_corridor"];
        _dialogue = Resources.Musics["demo_question"];
        _dialogue.Looping = false;
        Raylib.PlayMusicStream(Game.MusicPlaying.Value);
        Raylib.PlayMusicStream(_dialogue);
        Time.Reset();
    }
    
    public override void Update() 
    {
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawTexture(Resources.Sprites["undertale_door"], 0, 0, Color.White);
        Raylib.DrawTexture(Resources.Sprites["monster_placeholder"], 220, 60, Color.White);

        if (Raylib.IsKeyPressed(KeyboardKey.One)) { _speechParticles.Add(new MimicWord(WordSpawnPos(), _wordTemplates[0])); }
        if (Raylib.IsKeyPressed(KeyboardKey.Two)) { _speechParticles.Add(new MimicWord(WordSpawnPos(), _wordTemplates[1])); }
        if (Raylib.IsKeyPressed(KeyboardKey.Three)) { _speechParticles.Add(new MimicWord(WordSpawnPos(), _wordTemplates[2])); }
        if (Raylib.IsKeyPressed(KeyboardKey.Four)) { _speechParticles.Add(new MimicWord(WordSpawnPos(), _wordTemplates[3])); }
        if (Raylib.IsKeyPressed(KeyboardKey.Five)) { _speechParticles.Add(new MimicWord(WordSpawnPos(), _wordTemplates[4])); }
        
        Raylib.UpdateMusicStream(_dialogue);

        // Draw all speech particles and remove any that are too old.
        _speechParticles.RemoveAll(p => p.Draw());
        
        Raylib.DrawTexture(Resources.Sprites["intercom"], 0, 0, Color.White);
        Raylib.DrawTexture(Resources.Sprites["speech_bubble"], 0, 140, Color.White);
        string caption = "Richard, are you okay? I \nheard a loud thud.";
        caption = caption.Substring(0, Math.Min((int)(Time.Scaled * 15), caption.Length));
        Raylib.DrawText(caption, 5, 180, 20, Color.White);
    }

    public Vector2 WordSpawnPos()
    {
        return new Vector2(Random.Shared.Next(200, 260), Random.Shared.Next(60, 70));
    }
}

public class MimicWordTemplate
{
    public string Caption;
    public float SoundStart;
    public float SoundDuration;
    public string SoundSourcePath;

    public MimicWordTemplate(string caption, float soundStart, float soundDuration, string soundSourcePath)
    {
        Caption = caption;
        SoundStart = soundStart;
        SoundDuration = soundDuration;
        SoundSourcePath = soundSourcePath;
    }
}