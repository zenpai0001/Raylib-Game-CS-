using System.Numerics;
using Raylib_cs;

namespace GameLibrary;

public class GameScene : Scene
{
    private float _playerAnimStart;
    private Music _ambience;
    private Music _gameMusic;
    private Music _mindMusic;
    private Music _dialogue; // NPC Speech
    private string _dialogueCaption = "Richard, are you okay? I \nheard a loud thud.";
    private float _dialogueStartTime = 0f;
    // private Music _monsterMouth = Raylib.LoadMusicStream(Game.Dir + $"music/source1.mp3"); // Player Speech
    private Font _monsterFont = Resources.Fonts["sd_auto_pilot"];
    private bool _mindMenuOpen;
    private float _mindMenuTransition;
    private MimicPhrase _mimicPhrase;
    private string _currentSentence = "";
    private float _lastWordTime;
    private List<MimicWord> _mimicWords = new();
    private MimicWord? _activeWord;
    private int _levelId;
    private int _levelStage = 1;
    private int _levelStageCount = 3;
    private Texture2D _portrait;
    private Texture2D _background;

    private float _fadeToBlackTime = -1;

    private int _wordToAssign = 0;

    private int _mashCounter = 0;

    private Pingas? _pingas;

    private Dictionary<KeyboardKey, int> _bindings = new Dictionary<KeyboardKey, int>();
    
    public GameScene(MimicPhrase phrase, int levelId, int levelStageCount, Music dialogue, string dialogueCaption, Texture2D portrait, Texture2D background)
    {
        Time.Reset();
        _mimicPhrase = phrase;
        _levelId = levelId;
        _levelStageCount = levelStageCount;
        _dialogue  = dialogue;
        _dialogueCaption = dialogueCaption;
        _portrait = portrait;
        _background = background;
        UpdateNPCDialogue(_dialogue, _dialogueCaption);
        _gameMusic = Resources.Musics["metrnoid"];
        _mindMusic = Resources.Musics["monstermind"];
        _ambience  = Resources.Musics["ambience_corridor"];
        _dialogue.Looping = false;
        Raylib.PlayMusicStream(_gameMusic);
        Raylib.PlayMusicStream(_mindMusic);
        Raylib.SetMusicVolume(_ambience, 0.5f);
        Raylib.PlayMusicStream(_ambience);
        Raylib.PlayMusicStream(_dialogue);
    }
    
    public override void Update()
    {
        Raylib.ClearBackground(Color.Black);

        if (Raylib.IsKeyPressed(KeyboardKey.Backspace) || Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            if (_mindMenuOpen)
                CloseMindMenu();
            else
                OpenMindMenu();
            _mindMenuOpen = !_mindMenuOpen;
        }
        
        if (Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift) || Raylib.IsKeyDown(KeyboardKey.Zero))
        {
            Raylib.SetMusicPitch(_mimicPhrase.MonsterMouth, 0.66f);
        }
        else if (Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl) || Raylib.IsKeyDown(KeyboardKey.Nine))
        {
            Raylib.SetMusicPitch(_mimicPhrase.MonsterMouth, 1.5f);
        }
        else
        {
            Raylib.SetMusicPitch(_mimicPhrase.MonsterMouth, 1f);
        }

        if (_pingas?.Update() ?? false) _pingas = null;

        if (_mindMenuOpen)
        {
            MindMenu();
            _mindMenuTransition = _mindMenuTransition.MoveTowards(1f, 1f / (60f * 0.3f));
        }
        else
        {
            DoorScene();
            _mindMenuTransition = _mindMenuTransition.MoveTowards(0f, 1f / (60f * 0.3f));
        }
        
        Raylib.SetMusicVolume(_gameMusic, 0);
        Raylib.SetMusicVolume(_mindMusic, _mindMenuTransition*0.1f);
        
        Raylib.UpdateMusicStream(_gameMusic);
        Raylib.UpdateMusicStream(_mindMusic);
        Raylib.UpdateMusicStream(_ambience);

        if (_fadeToBlackTime > 0)
        {
            Raylib.DrawRectangle(0, 0, 320, 240, Raylib.ColorAlpha(Color.Black, Math.Clamp(1 - (_fadeToBlackTime - Time.Scaled) * 2, 0, 1)));
        }
    }

    private void OpenMindMenu()
    {
        _mimicPhrase.MonsterMouth.Looping = true;
        Raylib.SeekMusicStream(_mimicPhrase.MonsterMouth, 0.01f);
        Raylib.PlayMusicStream(_mimicPhrase.MonsterMouth);
        Time.Paused = true;
    }

    private void CloseMindMenu()
    {
        _mimicPhrase.MonsterMouth.Looping = false;
        Raylib.SetMusicPitch(_mimicPhrase.MonsterMouth, 1f);
        Raylib.PauseMusicStream(_mimicPhrase.MonsterMouth);
        Time.Paused = false;
    }

    private void MindMenu()
    {
        Raylib.DrawTexture(Resources.Sprites["keyboard"], 10, 120, Color.White);
        
        Raylib.UpdateMusicStream(_mimicPhrase.MonsterMouth);

        if (Raylib.IsKeyPressed(KeyboardKey.Left))
        {
            Raylib.SeekMusicStream(_mimicPhrase.MonsterMouth, Math.Max(Raylib.GetMusicTimePlayed(_mimicPhrase.MonsterMouth)-5f, 0f));
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Right))
        {
            Raylib.SeekMusicStream(_mimicPhrase.MonsterMouth, Math.Min(Raylib.GetMusicTimePlayed(_mimicPhrase.MonsterMouth)+5f, Raylib.GetMusicTimeLength(_mimicPhrase.MonsterMouth)-1));
        }
        
        Raylib.DrawTexture(Resources.Sprites["waveform"], 10, 50, Color.White);
        float t = Raylib.GetMusicTimePlayed(_mimicPhrase.MonsterMouth) / Raylib.GetMusicTimeLength(_mimicPhrase.MonsterMouth);
        Raylib.DrawRectangle((int)(10 + 300 * t), 50, 1, 48, Color.White);

        foreach (KeyboardKey key in Assets.UsableKeys)
        {
            if (Raylib.IsKeyPressed(key))
            {
                _bindings.Remove(key);
                _bindings.Add(key, _mimicPhrase.GetWordIndexAtTime(Raylib.GetMusicTimePlayed(_mimicPhrase.MonsterMouth)));
            }
        }

        foreach (KeyValuePair<KeyboardKey,int> binding in _bindings)
        {
            Vector2 pos = new Vector2((int)(10 + 300 * _mimicPhrase.Words[binding.Value].SoundStart / _mimicPhrase.TotalDuration), 100);
            Raylib.DrawTextEx(_monsterFont, Enum.GetName(binding.Key), pos, 21, 0, Color.White);
        }
    }
    
    private void NewMindMenu()
    {
        Raylib.DrawTexture(Resources.Sprites["keyboard"], 10, 120, Color.White);
        
        Raylib.UpdateMusicStream(_mimicPhrase.MonsterMouth);

        if (Raylib.IsKeyPressed(KeyboardKey.Backspace) || Raylib.IsKeyPressed(KeyboardKey.Left))
        {
            _wordToAssign--;
            if (_wordToAssign < 0) _wordToAssign = _mimicPhrase.Words.Count - 1;
            Raylib.SeekMusicStream(_mimicPhrase.MonsterMouth, _mimicPhrase.Words[_wordToAssign].SoundStart);
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.Right))
        {
            _wordToAssign++;
            if (_wordToAssign >= _mimicPhrase.Words.Count) _wordToAssign = 0;
            Raylib.SeekMusicStream(_mimicPhrase.MonsterMouth, _mimicPhrase.Words[_wordToAssign].SoundStart);
        }

        if (_wordToAssign < _mimicPhrase.Words.Count - 1 && 
            Raylib.GetMusicTimePlayed(_mimicPhrase.MonsterMouth) > _mimicPhrase.Words[_wordToAssign + 1].SoundStart)
        {
            Raylib.PlayMusicStream(_mimicPhrase.MonsterMouth);
            Raylib.SeekMusicStream(_mimicPhrase.MonsterMouth, _mimicPhrase.Words[_wordToAssign].SoundStart);
        }
        
        Raylib.DrawTextEx(_monsterFont, _mimicPhrase.Words[_wordToAssign].Caption, new Vector2(100, 30), 21, 0, Color.White);
        
        if (_activeWord != null && Raylib.GetMusicTimePlayed(_mimicPhrase.MonsterMouth) >= _activeWord.SoundStart + _activeWord.SoundDuration)
        {
            Raylib.StopMusicStream(_mimicPhrase.MonsterMouth);
        }

        foreach (KeyboardKey key in Assets.UsableKeys)
        {
            if (Raylib.IsKeyPressed(key))
            {
                _bindings.Remove(key);
                _bindings.Add(key, _wordToAssign);

                _wordToAssign++;
                _wordToAssign %= _mimicPhrase.Words.Count;
                
                Raylib.PlayMusicStream(_mimicPhrase.MonsterMouth);
                Raylib.SeekMusicStream(_mimicPhrase.MonsterMouth, _mimicPhrase.Words[_wordToAssign].SoundStart);

                break;
            }
        }
    }

    private void DoorScene()
    {
        Raylib.DrawTexture(_background, 0, 0, Color.White);
        Raylib.DrawTexture(Resources.Sprites["monster"], 20, 70, Color.White);

        foreach (KeyboardKey key in Assets.UsableKeys)
        {
            if (Raylib.IsKeyPressed(key) && _bindings.ContainsKey(key))
            {
                Speak(_bindings[key]);
            }
        }
        
        if (Raylib.IsKeyPressed(KeyboardKey.One))   {Speak(_mimicPhrase.FavoriteWords[0]);}
        if (Raylib.IsKeyPressed(KeyboardKey.Two))   {Speak(_mimicPhrase.FavoriteWords[1]);}
        if (Raylib.IsKeyPressed(KeyboardKey.Three)) {Speak(_mimicPhrase.FavoriteWords[2]);}
        if (Raylib.IsKeyPressed(KeyboardKey.Four))  {Speak(_mimicPhrase.FavoriteWords[3]);}
        if (Raylib.IsKeyPressed(KeyboardKey.Five))  {Speak(_mimicPhrase.FavoriteWords[4]);}
        if (Raylib.IsKeyPressed(KeyboardKey.Six))   {Speak(_mimicPhrase.FavoriteWords[5]);}

        if (_currentSentence != "" && Time.Scaled - _lastWordTime > 1f)
        {
            Console.WriteLine($"Player has spoken the line: {_currentSentence}");
            CheckForLevelProgress();
            _currentSentence = "";
        }

        if (_activeWord != null && Raylib.GetMusicTimePlayed(_mimicPhrase.MonsterMouth) >= _activeWord.SoundStart + _activeWord.SoundDuration)
        {
            Raylib.StopMusicStream(_mimicPhrase.MonsterMouth);
        }
        
        Raylib.UpdateMusicStream(_dialogue);
        Raylib.UpdateMusicStream(_mimicPhrase.MonsterMouth);

        // Draw all speech particles and remove any that are too old.
        _mimicWords.RemoveAll(p => p.Draw());
        
        Raylib.DrawTexture(Resources.Sprites["intercom"], 210, 0, Color.White);
        Raylib.DrawTexture(_portrait, 220, 10, Color.White);
        Raylib.DrawTexture(Resources.Sprites["speech_bubble"], 0, 140, Color.White);
        string caption = _dialogueCaption.Substring(0, Math.Clamp((int)((Time.Scaled - _dialogueStartTime) * 15), 0, _dialogueCaption.Length));
        ImGui.DrawText(caption, 6, 176, 10);
        
        Raylib.DrawRectangle(0, 0, 320, 240, Raylib.ColorAlpha(Color.Black, Math.Clamp(1-Time.Unscaled * 2, 0, 1)));

        if (_levelId == 1)
        {
            Raylib.DrawRectangle(0, 0, 250, 12, new Color(0,0,0,200));
            ImGui.DrawText("Press 1, 2, 3, 4, 5, 6 to speak", 2, 2, 10);
        }

        if (Time.Frame % 100 == 0)
        {
            if (_mashCounter > 15 && _pingas == null)
            {
                _pingas = Assets.Pingas[_levelId];
                _pingas.Start();
            }
            _mashCounter = 0;
        }
    }

    private void Speak(int wordIndex)
    {
        _mashCounter++;
        _activeWord = _mimicPhrase.GetWord(wordIndex, WordSpawnPos());
        _currentSentence += _activeWord.Text + " ";
        _lastWordTime = Time.Scaled;
        _mimicWords.Add(_activeWord);
        Raylib.PlayMusicStream(_mimicPhrase.MonsterMouth);
        Raylib.SeekMusicStream(_mimicPhrase.MonsterMouth, _activeWord.SoundStart);
    }
    
    private Vector2 WordSpawnPos()
    {
        return new Vector2(Random.Shared.Next(100, 160), Random.Shared.Next(60, 70));
    }

    private void UpdateNPCDialogue(Music dialogue, string caption)
    {
        _dialogue = dialogue;
        _dialogue.Looping = false;
        Raylib.PlayMusicStream(_dialogue);
        _dialogueStartTime = Time.Scaled;
        _dialogueCaption = caption;
    }

    private void CheckForLevelProgress()
    {
        switch ($"{_levelId}-{_levelStage}")
        {
            case "1-1":
                if (_currentSentence.ToLowerInvariant().Contains("yeah") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    UpdateNPCDialogue(Resources.Musics["brewster_2"], "Are you OK? It sounded like \nsomething big moved around \nin there");
                    _levelStage = 2;
                }
                break;
            case "1-2":
                if (_currentSentence.ToLowerInvariant().Contains("alright") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    _fadeToBlackTime = Time.Scaled + 6;
                    _dialogueStartTime = Time.Scaled;
                    _dialogueCaption = "Okay, come in. Can you cover my \npost? I need to report this \nmalfunction to chief engineer \nPayton. And hey, don't let me \ncatch you napping on shift \nlike yesterday. You'll make the whole crew look bad.";
                    _pingas = new Pingas(Resources.Musics["brewster_cutscene"], endAction: () =>
                    {
                        Game.ActiveScene = new GameScene
                        (
                            Assets.Level2Phrase,
                            2, 
                            2, 
                            Resources.Musics["ensignkat_1"], 
                            "Sir, Im sorry, this is a bad \ntime. Cant it wait?", 
                            Resources.Sprites["portrait2"], 
                            Resources.Sprites["navigationbg"]
                        );
                    }).Start();
                }
                break;
            case "2-1":
                if (_currentSentence.ToLowerInvariant().Contains("payton") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    UpdateNPCDialogue(Resources.Musics["ensignkat_2"], "Why not use ship comms? We \nare at red alert.");
                    _levelStage = 2;
                }
                break;
            case "2-2":
                if (_currentSentence.ToLowerInvariant().Contains("payton") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    UpdateNPCDialogue(Resources.Musics["ensignkat_4"], "P-Payton? What about him?");
                    _levelStage = 3;
                }
                break;
            case "2-3":
                if (_currentSentence.ToLowerInvariant().Contains("payton") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    UpdateNPCDialogue(Resources.Musics["ensignkat_5"], "Oh, right. Is it good news? \nCould you let me report it?");
                    _levelStage = 4;
                }
                break;
            case "2-4":
                if (_currentSentence.ToLowerInvariant().Contains("like payton") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    _fadeToBlackTime = Time.Scaled + 14;
                    _dialogueStartTime = Time.Scaled;
                    _dialogueCaption = "Who told you? I'd love it if he \nwould even acknowledge me. \nI've been scrubbing conduits \ntrying to get his attention \nsince my second week on \nthis ship! He acts all uptight, \nbut I know that secretly he's the one who keeps smuggling those smutty novels onboard. I bet he's got an eye on the captain, that rascal!";
                    _pingas = new Pingas(Resources.Musics["ensignkat_cutscene"], endAction: () =>
                    {
                        Game.ActiveScene = new GameScene
                        (
                            Assets.Level3Phrase,
                            3, 
                            2, 
                            Resources.Musics["payton_1"], 
                            "Ensign? We're at red alert, I \ncant let you into Engineering \nagain unless you’ve got a damn \ngood reason.", 
                            Resources.Sprites["portrait3"], 
                            Resources.Sprites["medicalbg"]
                        );
                    }).Start();
                }
                break;
            case "3-1":
                if (_currentSentence.ToLowerInvariant().Contains("smutty") || _currentSentence.ToLowerInvariant().Contains("smugg") || _currentSentence.ToLowerInvariant().Contains("novel") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    UpdateNPCDialogue(Resources.Musics["payton_2"], "That could be anybody!");
                    _levelStage = 2;
                }
                break;
            case "3-2":
                if (_currentSentence.ToLowerInvariant().Contains("captain") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    UpdateNPCDialogue(Resources.Musics["payton_3"], "Her? Shed never compromise her \ncommitment to the ship for a man. \nI know a lost cause when I see one!");
                    _levelStage = 3;
                }
                break;
            case "3-3":
                if (_currentSentence.ToLowerInvariant().Contains("i love you") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    _fadeToBlackTime = Time.Scaled + 7;
                    _dialogueStartTime = Time.Scaled;
                    _dialogueCaption = "Oh. OH. That explains... a \nlot. Why don't we share my \nescape pod, I think yours \n\"failed it's last inspection\".";
                    _pingas = new Pingas(Resources.Musics["payton_cutscene"], endAction: () =>
                    {
                        Game.ActiveScene = new GameScene
                        (
                            Assets.Level4Phrase,
                            4, 
                            2, 
                            Resources.Musics["captain_1"], 
                            "Payton, is that you? I'm glad \nto hear your voice, I can't \nreach any of the other crew.", 
                            Resources.Sprites["portrait4"], 
                            Resources.Sprites["engineeringbg"]
                        );
                    }).Start();
                }
                break;
            case "4-1":
                if (true || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    UpdateNPCDialogue(Resources.Musics["captain_2"], "I'm reading unidentified life \nsigns. There may be an intruder \naboard this ship. Stay sharp, \nand don't open your door for anyone.");
                    _levelStage = 2;
                }
                break;
            case "4-2":
                if (true || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    UpdateNPCDialogue(Resources.Musics["captain_3"], "We're approaching reentry. \nGet to your escape pod at \nonce!");
                    _levelStage = 3;
                }
                break;
            case "4-3":
                if ((_currentSentence.ToLowerInvariant().Contains("escape") && _currentSentence.ToLowerInvariant().Contains("failed")) || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    _fadeToBlackTime = Time.Scaled + 5;
                    _dialogueStartTime = Time.Scaled;
                    _dialogueCaption = "You can use mine, I wasn't \nplanning on taking it myself. \nShit! I can't let it escape, not \nthis close to earth! Computer, \ncommence self destruct sequence. \nCommand code: Omega-One-Seven-Victor-Victor-Three";
                    _pingas = new Pingas(Resources.Musics["captain_cutscene"], endAction: () =>
                    {
                        Game.ActiveScene = new GameScene
                        (
                            Assets.Level5Phrase,
                            5, 
                            2, 
                            Resources.Musics["self_destruct_sequence"], 
                            "SELF DESTRUCT IN ONE MINUTE...", 
                            Resources.Sprites["portrait5"], 
                            Resources.Sprites["bridgebg"]
                        );
                    }).Start();
                }

                break;
            case "5-1":
                if (_currentSentence.ToLowerInvariant().Contains("omega one seven victor victor three") || Raylib.IsKeyDown(KeyboardKey.Equal))
                {
                    Game.ActiveScene = new GameOverScene();
                }
                break;
        }
    }
}