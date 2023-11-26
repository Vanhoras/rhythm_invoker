using Godot;

public partial class HighscoreMenu : Control
{
	[Export]
	private Control mainMenuControl;

	[Export]
	private MainMenuUI mainMenuUI;

    [Export]
    private RichTextLabel lastScoreLabel;

    [Export]
    private RichTextLabel highScoreLabel;

    public void GoBackToMenu()
	{
        mainMenuControl.Show();
		Hide();
    }

	public void PlayGame()
	{
        mainMenuUI.PlayGame();
    }

	public void DisplayScore(long highScore, long lastScore)
	{
		lastScoreLabel.Text = "[center]" + lastScore.ToString();
		highScoreLabel.Text = "[center]" + highScore.ToString();
	}
}
