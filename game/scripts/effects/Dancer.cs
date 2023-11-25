using Godot;

public partial class Dancer : Node
{
	[Export]
	private Conductor conductor;

	[Export]
	private AnimatedSprite2D animation;

	private int frameNumber;
	private int currentFrame;

	public override void _Ready()
	{
		conductor.OnBeat += ShowNextFrame;

		frameNumber = animation.SpriteFrames.GetFrameCount("default");
	}

	private void ShowNextFrame()
	{
		currentFrame = currentFrame < frameNumber - 1 ? currentFrame + 1 : 0;

		animation.Frame = currentFrame;
	}
}
