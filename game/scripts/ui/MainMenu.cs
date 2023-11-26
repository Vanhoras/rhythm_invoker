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

    public void Instantiate(SceneLoader sceneLoader, bool showHighscore)
    {
        loader = sceneLoader;
        ui.Instantiate(showHighscore);
    }

    public void PlayGame()
	{
        loader.LoadLevel();
    }
}
