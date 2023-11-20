using Godot;

public partial class PauseMenu : Control
{
    private bool _isPaused = false;

    public bool IsPaused {  get { return _isPaused; } }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            Pause(!_isPaused);
        }
    }

    public void Pause(bool pause)
    {
        GD.Print("Pause " + pause);
        _isPaused = pause;
        GetTree().Paused = pause;
        Visible = pause;
    }

}
