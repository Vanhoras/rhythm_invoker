using Godot;

public partial class HighscoreMenu : Control
{
	[Export]
	private Control mainMenuControl;

	[Export]
	private MainMenu mainMenu;
	
	public void GoBackToMenu()
	{
        mainMenuControl.Show();
		Hide();
    }

	public void PlayGame()
	{
        mainMenu.PlayGame();
    }
}
