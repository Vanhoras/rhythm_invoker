using Godot;

public partial class MainMenuUI : Node
{
    [Signal]
    public delegate void PlayGameEventEventHandler();

    [Export]
    private Control mainMenuContainer;

    [Export]
    private Control highscoreMenu;

    public void Instantiate(bool showHighscore)
    {
        if (showHighscore )
        {
            mainMenuContainer.Hide();
            highscoreMenu.Show();
        }
    }

    public void PlayGame()
	{
        EmitSignal(SignalName.PlayGameEvent);
    }
}
