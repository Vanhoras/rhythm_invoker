using Godot;

public partial class Circle : Node2D
{

	[Export]
	private Conductor conductor;

	[Export]
	private AnimatedSprite2D innerCircle;

	[Export]
	private AnimatedSprite2D outerCircle;

    [Export]
    private float rotationSpeed;


    private int innerCircleAnimationFrames;
	private int innerCircleCurrentFrame;
	
	private int outerCircleAnimationFrames;
    private int outerCircleCurrentFrame;

    public override void _Ready()
	{
		conductor.OnBeat += OnBeat;

		innerCircleAnimationFrames = innerCircle.SpriteFrames.GetFrameCount("default");
        outerCircleAnimationFrames = outerCircle.SpriteFrames.GetFrameCount("default");

		innerCircle.Centered = true;
		outerCircle.Centered = true;
    }

	public override void _Process(double delta)
	{
		float rotation = rotationSpeed * (float)delta;

        innerCircle.Rotate(rotation);
		outerCircle.Rotate(-rotation);
	}

	private void OnBeat()
	{
        innerCircleCurrentFrame = innerCircleCurrentFrame < innerCircleAnimationFrames - 1 ? innerCircleCurrentFrame + 1 : 0;
        outerCircleCurrentFrame = outerCircleCurrentFrame < outerCircleAnimationFrames - 1 ? outerCircleCurrentFrame + 1 : 0;

		innerCircle.Animation = "default";
		outerCircle.Animation = "default";

        innerCircle.Frame = innerCircleCurrentFrame;
        outerCircle.Frame = outerCircleCurrentFrame;
    }
}
