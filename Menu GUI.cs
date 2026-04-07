using Raylib_cs;
using Raylib_Game_CS_;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Numerics;


namespace Raylib_Game_C_;

public class Menu
{
    // UI required variables

    //Game paused event
    public event EventHandler? GamePaused;

    public event EventHandler? ButtonPressed;

    public void Container()
    {
    
    }

    protected virtual void OnButtonPressed(EventArgs e)
    {
        ButtonPressed?.Invoke(this, e);
    }
    protected virtual void OnPause(EventArgs e)
    {
            GamePaused?.Invoke(this, e);
    }
}
