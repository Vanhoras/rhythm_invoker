using Godot;

public partial class HighscoreMenu : Control
{
	[Export]
	private Control mainMenuControl;

	[Export]
	private MainMenuUI mainMenuUI;
	
	public void GoBackToMenu()
	{
        mainMenuControl.Show();
		Hide();
    }

	public void PlayGame()
	{
        mainMenuUI.PlayGame();
    }
}
