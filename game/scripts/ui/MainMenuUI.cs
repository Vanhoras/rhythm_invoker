using Godot;

public partial class MainMenuUI : Node
{
    [Signal]
    public delegate void PlayGameEventEventHandler();

    [Export]
    private Control mainMenuContainer;

    [Export]
    private HighscoreMenu highscoreMenu;

    public void Instantiate(bool showHighscore, HighScoreManager highScoreManager)
    {
        if (showHighscore )
        {
            mainMenuContainer.Hide();
            highscoreMenu.Show();
        }

        highscoreMenu.DisplayScore(highScoreManager.HighScore, highScoreManager.LastScore);
    }

    public void PlayGame()
	{
        EmitSignal(SignalName.PlayGameEvent);
    }
}
