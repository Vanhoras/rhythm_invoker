using Godot;

public partial class Fireball : Node
{

	[Export]
	public Conductor conductor;

	[Export]
	public Node2D circle;

	Vector2 circleCenter;



	public override void _Ready()
	{
		circleCenter = circle.GlobalPosition;

    }

	public override void _Process(double delta)
	{
		double songDelta = conductor.GetSongPositionDelta();


    }
}
