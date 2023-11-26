using Godot;

public partial class MainMenuContainer : Control
{
	[Export]
	private Control settingsNode;

    [Export]
    private Control creditsNode;

    [Export]
	private MainMenuUI mainMenuUI;

	public void QuitGame()
	{
		GetTree().Quit();
	}

    public void OpenSettings()
    {
        settingsNode.Show();
		Hide();
    }

	public void OpenCredits()
	{
		creditsNode.Show();
		Hide();
	}

	public void PlayGame()
	{
        mainMenuUI.PlayGame();
    }
}
