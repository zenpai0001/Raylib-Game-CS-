namespace GameLibrary;

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
