using Godot;

public partial class MainMenu : Node
{
	[Export]
    private SceneLoader loader;

    [Export]
    private MainMenuUI ui;

    public override void _Ready()
    {
        ui.PlayGameEvent += PlayGame;
    }

    public void Instantiate(SceneLoader sceneLoader, bool showHighscore, HighScoreManager highScoreManager)
    {
        loader = sceneLoader;
        ui.Instantiate(showHighscore, highScoreManager);
    }

    public void PlayGame()
	{
        loader.LoadLevel();
    }
}
