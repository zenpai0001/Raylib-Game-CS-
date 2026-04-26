using Raylib_cs;

namespace GameLibrary;

public class RichardCutscene : Scene
{
    private Music _cutsceneAudio = Resources.Musics["richard_cutscene"];
    
    public RichardCutscene()
    {
        Time.Reset();
        _cutsceneAudio.Looping = false;
        Raylib.PlayMusicStream(_cutsceneAudio);
    }
    
    public override void Update()
    {
        Raylib.UpdateMusicStream(_cutsceneAudio);
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawTexture(Resources.Sprites["engineeringbg"], 41, 20, Color.White);
        if (!Raylib.IsMusicStreamPlaying(_cutsceneAudio) || Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            Game.ActiveScene = new GameScene
            (
                Assets.Level1Phrase,
                1, 
                2, 
                Resources.Musics["brewster_1"], 
                "Richard, are you there?", 
                Resources.Sprites["portrait1"], 
                Resources.Sprites["door"]
            );
        }
    }
}