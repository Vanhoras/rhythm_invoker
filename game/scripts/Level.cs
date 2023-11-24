using Godot;

public partial class Level : Node
{
	[Export]
	private CanvasLayer canvas;

	[Export]
	private PackedScene mainMenu;

	[Export]
	private Conductor conductor;

	public override void _Ready()
	{
		conductor.Finished += FinishLevel;
	}

	public void FinishLevel()
	{
		Node instance = mainMenu.Instantiate();
		canvas.AddChild(instance);

		QueueFree();
	}

}
