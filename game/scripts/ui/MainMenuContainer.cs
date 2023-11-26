using Godot;

public partial class MainMenuContainer : Control
{
	[Export]
	private Control settingsNode;

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

	public void PlayGame()
	{
        mainMenuUI.PlayGame();
    }
}
