using Godot;

public partial class DistanceMeasurer : Node2D
{
	[Export]
	private Node2D targetNode;

	public override void _Ready()
	{
		float distance = GlobalPosition.DistanceTo(targetNode.GlobalPosition);
		GD.Print($"Distance: {distance}");
	}
}
