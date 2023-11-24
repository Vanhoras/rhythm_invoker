using Godot;

public partial class SettingsMenu : Control
{
    [Export]
    private Control mainMenuControl;

    public void GoBackToMenu()
    {
        mainMenuControl.Show();
        Hide();
    }
}
