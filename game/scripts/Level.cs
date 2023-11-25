using Godot;

public partial class Level : Node
{
	[Export]
	private Conductor conductor;

	private SceneLoader sceneLoader;	

	public void Instantiate(SceneLoader sceneLoader)
	{
		this.sceneLoader = sceneLoader;
	}

	public override void _Ready()
	{
		conductor.Finished += FinishLevel;
	}

	public void FinishLevel()
	{
		sceneLoader.LoadMainMenu();
	}

}
