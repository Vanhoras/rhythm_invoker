using Godot;

public partial class MainMenu : Node
{
	[Export]
    private SceneLoader loader;

    [Export]
    private Control mainMenuContainer;

    [Export]
    private Control highscoreMenu;

    public void Instantiate(SceneLoader sceneLoader, bool showHighscore)
    {
        loader = sceneLoader;
        if (showHighscore )
        {
            mainMenuContainer.Hide();
            highscoreMenu.Show();
        }
    }

    public void PlayGame()
	{
        loader.LoadLevel();
    }
}
