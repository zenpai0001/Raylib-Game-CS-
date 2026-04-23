using Raylib_cs;
using System.Numerics;

namespace GameLibrary;

public static class Game
{
    public static bool IsWeb;
    public static bool ShouldQuit;
    public static string Dir = "";
    public static Menu Menu = new Menu();
    public static Player Player = new Player();
    public static Queue<Action> LateActions = new Queue<Action>(); // LateActions are dequeued and invoked after everything else has updated.
    public static Scene ActiveScene;

    // Target Resolution: 320x240, 2X: 640x480, 3X: 960x720, 4x 1280x960
    private static Camera2D _defaultCamera = new Camera2D(new Vector2(0, 0), Vector2.Zero, 0, 4);
    private static Camera2D _activeCamera = _defaultCamera;

    public static void Load(int scaleFactor)
    {
        _defaultCamera.Zoom = scaleFactor;
        Raylib.InitWindow(320 * scaleFactor, 240 * scaleFactor, "Hello World");
        Raylib.SetTargetFPS(Time.FrameRate);
        Raylib.InitAudioDevice();
        Raylib.SetExitKey(KeyboardKey.Null);
        
        Resources.Load();
        
        ActiveScene = new MainMenu();
    }
    
    public static void Update()
    {
        Time.UpdateTime();
        
        Raylib.BeginDrawing();
        SetCamera();
        
        ActiveScene.Update();
        
        while (LateActions.Count > 0) LateActions.Dequeue().Invoke();
        
        Raylib.EndMode2D();
        Raylib.DrawText("FPS: " + Raylib.GetFPS(), 0, 0, 20, Color.White);
        _activeCamera = new Camera2D();
        Raylib.EndDrawing();
    }
    
    public static void SetCamera(Camera2D? camera = null)
    {
        Camera2D cam = camera ?? _defaultCamera;
        
        if (_activeCamera.Offset == cam.Offset &&      // ReSharper disable once CompareOfFloatsByEqualityOperator
            _activeCamera.Zoom == cam.Zoom &&          // ReSharper disable once CompareOfFloatsByEqualityOperator
            _activeCamera.Rotation == cam.Rotation &&
            _activeCamera.Target == cam.Target)
        {
            return;
        }
        Raylib.EndMode2D();
        _activeCamera = cam;
        Raylib.BeginMode2D(_activeCamera);
    }

    public static Camera2D GetActiveCamera() => _activeCamera;

    public static Vector2 GetCursorPos() => Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _activeCamera);
}
