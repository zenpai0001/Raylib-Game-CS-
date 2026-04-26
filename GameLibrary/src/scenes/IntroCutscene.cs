using Raylib_cs;

namespace GameLibrary;

public class IntroCutscene : Scene
{
    private Music _cutsceneAudio = Resources.Musics["intro_scene"];

    private string _script =
"""
Captain Windlass: 
    ...come in... 
    Earth Control, do you read?

Earth Control: 
    This is Earth Control, 
    we've got you on all bands.

Captain Windlass: 
    This is captain Janet 
    Windlass of the EFF Mill Bay. 
    our vessel's been struck by 
    unregistered debris and our
    engines are disabled. Our 
    orbit will decay in thirty 
    minutes without reboost. 
    We're attempting repairs, but 
    the damage seems severe.

Earth Control: 
    Copy that Mill Bay, the 
    nearest tugs we can scramble 
    are fourty minutes out. We 
    advise you ready your escape 
    pods. Projected impact site 
    is uninhabited.

Captain Windlass: 
    Roger, Mill Bay out.

""".ReplaceLineEndings("\n");

    public IntroCutscene()
    {
        Time.Reset();
        _cutsceneAudio.Looping = false;
        Raylib.PlayMusicStream(_cutsceneAudio);
    }
    
    public override void Update()
    {
        Raylib.UpdateMusicStream(_cutsceneAudio);
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawTexture(Resources.Sprites["menubg"], 0, 0, Color.White);
        Raylib.DrawRectangle(35, 0, 250, 240, new Color(0,0,0,200));
        ImGui.DrawText(_script, 37, (int)(200 - Time.Scaled * 10), 10);
        if (!Raylib.IsMusicStreamPlaying(_cutsceneAudio) || Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            Game.ActiveScene = new RichardCutscene();
        }
    }
}