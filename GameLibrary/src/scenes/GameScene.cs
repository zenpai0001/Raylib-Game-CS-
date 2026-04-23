using System.Numerics;
using Raylib_cs;

namespace GameLibrary;

public class GameScene : Scene
{
    private float _playerAnimStart;
    private List<MimicWord> _speechParticles = new();
    private Music _ambience;
    private Music _gameMusic;
    private Music _mindMusic;
    private Music _dialogue;
    private Font _monsterFont = Resources.Fonts["oxygene"];
    private bool _mindMenuOpen;
    private float _mindMenuTransition;
    private MimicPhrase _mimicPhrase = Assets.MimicPhrase;
    private Music _sourceSound = Raylib.LoadMusicStream(Game.Dir + $"music/{Assets.MimicPhrase.SoundSourcePath}.mp3");

    private Dictionary<KeyboardKey, int> _bindings = new Dictionary<KeyboardKey, int>();
    
    
    public GameScene()
    {
        _dialogue = Resources.Musics["demo_question"];
        _gameMusic = Resources.Musics["metrnoid"];
        _mindMusic = Resources.Musics["monstermind"];
        _ambience = Resources.Musics["ambience_corridor"];
        _dialogue.Looping = false;
        Raylib.PlayMusicStream(_gameMusic);
        Raylib.PlayMusicStream(_mindMusic);
        Raylib.PlayMusicStream(_ambience);
        Raylib.PlayMusicStream(_dialogue);
        Time.Reset();
    }
    
    public override void Update()
    {
        Raylib.ClearBackground(Color.Black);

        if (Raylib.IsKeyPressed(KeyboardKey.Backspace) || Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (_mindMenuOpen)
            {
                
            }
            else
            {
                OpenMindMenu();
            }
            _mindMenuOpen = !_mindMenuOpen;
        }

        if (_mindMenuOpen)
        {
            MindMenu(true);
            _mindMenuTransition = _mindMenuTransition.MoveTowards(1f, 1f / (60f * 0.3f));
        }
        else
        {
            DoorScene(true);
            _mindMenuTransition = _mindMenuTransition.MoveTowards(0f, 1f / (60f * 0.3f));
        }
        
        
        Raylib.SetMusicVolume(_gameMusic, 1 - _mindMenuTransition);
        Raylib.SetMusicVolume(_mindMusic, _mindMenuTransition);
        
        Raylib.UpdateMusicStream(_gameMusic);
        Raylib.UpdateMusicStream(_mindMusic);
        Raylib.UpdateMusicStream(_ambience);
    }

    private void OpenMindMenu()
    {
        Raylib.PlayMusicStream(_sourceSound);
    }

    private void MindMenu(bool inputAllowed)
    {
        Raylib.UpdateMusicStream(_sourceSound);
        
        Raylib.DrawTexture(Resources.Sprites["waveform"], 10, 50, Color.White);
        float t = Raylib.GetMusicTimePlayed(_sourceSound) / Raylib.GetMusicTimeLength(_sourceSound);
        Raylib.DrawRectangle((int)(10 + 300 * t), 50, 1, 48, Color.White);

        foreach (KeyboardKey key in Assets.UsableKeys)
        {
            if (Raylib.IsKeyPressed(key))
            {
                _bindings.Remove(key);
                _bindings.Add(key, _mimicPhrase.GetWordIndexAtTime(Raylib.GetMusicTimePlayed(_sourceSound)));
            }
        }

        foreach (KeyValuePair<KeyboardKey,int> binding in _bindings)
        {
            Vector2 pos = new Vector2(10 + 300 * _mimicPhrase.Words[binding.Value].SoundStart / _mimicPhrase.TotalDuration, 50);
            Raylib.DrawTextEx(_monsterFont, Enum.GetName(binding.Key), pos + Vector2.UnitY * 50, 12, 0, Color.White);
        }
    }

    private void DoorScene(bool inputAllowed)
    {
        Raylib.DrawTexture(Resources.Sprites["door"], 0, -20, Color.White);
        Raylib.DrawTexture(Resources.Sprites["monster"], 15, 70, Color.White);

        foreach (KeyboardKey key in Assets.UsableKeys)
        {
            if (Raylib.IsKeyPressed(key) && _bindings.ContainsKey(key))
            {
                _speechParticles.Add(_mimicPhrase.GetWord(_bindings[key], WordSpawnPos()));
            }
        }
        
        Raylib.UpdateMusicStream(_dialogue);

        // Draw all speech particles and remove any that are too old.
        _speechParticles.RemoveAll(p => p.Draw());
        
        Raylib.DrawTexture(Resources.Sprites["intercom"], 200, 0, Color.White);
        Raylib.DrawTexture(Resources.Sprites["speech_bubble"], 0, 140, Color.White);
        string caption = "Richard, are you okay? I \nheard a loud thud.";
        caption = caption.Substring(0, Math.Min((int)(Time.Scaled * 15), caption.Length));
        Raylib.DrawText(caption, 5, 180, 20, Color.White);
    }

    private Vector2 WordSpawnPos()
    {
        return new Vector2(Random.Shared.Next(100, 160), Random.Shared.Next(60, 70));
    }
}