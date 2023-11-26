using Godot;

public partial class Credits : Control
{
    [Export]
    private Control mainMenuControl;

    public void GoBackToMenu()
    {
        mainMenuControl.Show();
        Hide();
    }
}
