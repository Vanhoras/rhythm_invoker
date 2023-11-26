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

    [Export]
    private float flareDurationInBeats;
    private float flareDuration;


    private int innerCircleAnimationFrames;
	private int innerCircleCurrentFrame;
	
	private int outerCircleAnimationFrames;
    private int outerCircleCurrentFrame;

    private bool flaring = false;
    private float currentFlareDuration;

    public override void _Ready()
	{
		conductor.OnBeat += OnBeat;

		innerCircleAnimationFrames = innerCircle.SpriteFrames.GetFrameCount("default");
        outerCircleAnimationFrames = outerCircle.SpriteFrames.GetFrameCount("default");

		innerCircle.Centered = true;
		outerCircle.Centered = true;

        innerCircle.Animation = "default";
        outerCircle.Animation = "default";

        flareDuration = (float) (flareDurationInBeats * conductor.Song.SPB);
    }

	public override void _Process(double delta)
	{
		float rotation = rotationSpeed * (float)delta;

        innerCircle.Rotate(rotation);
		outerCircle.Rotate(-rotation);

        if (flaring)
        {
            currentFlareDuration += (float)delta;
            if (currentFlareDuration >= flareDuration)
            {
                UnFlare();
            }
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("button"))
        {
            OnButtonPress();
        }
    }

    private void OnButtonPress()
	{
        innerCircle.Animation = "flare";
        outerCircle.Animation = "flare";

        innerCircle.Frame = 0;
        outerCircle.Frame = 0;

        flaring = true;
        currentFlareDuration = 0;
    }

    private void UnFlare()
    {
        flaring = false;

        innerCircle.Animation = "default";
        outerCircle.Animation = "default";

        innerCircle.Frame = innerCircleCurrentFrame;
        outerCircle.Frame = outerCircleCurrentFrame;
    }

	private void OnBeat()
	{
        innerCircleCurrentFrame = (innerCircleCurrentFrame + 1) % innerCircleAnimationFrames;
        outerCircleCurrentFrame = (outerCircleCurrentFrame + 1) % outerCircleAnimationFrames;

        if (flaring) return;

        innerCircle.Frame = innerCircleCurrentFrame;
        outerCircle.Frame = outerCircleCurrentFrame;
    }
}
