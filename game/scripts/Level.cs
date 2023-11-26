using Godot;

public partial class Level : Node
{
	[Export]
	private Conductor conductor;

    [Export]
    private ScoreManager scoreManager;

    private SceneLoader sceneLoader;	
	private HighScoreManager highScoreManager;

	public void Instantiate(SceneLoader sceneLoader, HighScoreManager highScoreManager)
	{
		this.sceneLoader = sceneLoader;
		this.highScoreManager = highScoreManager;
	}

	public override void _Ready()
	{
		conductor.Finished += FinishLevel;
	}

	public void FinishLevel()
	{
		highScoreManager.SetScore(scoreManager.Score);

        sceneLoader.LoadMainMenu();
	}

}
