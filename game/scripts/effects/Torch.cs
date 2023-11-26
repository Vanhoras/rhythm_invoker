using Godot;

public partial class Torch : Node
{
    [Export]
    private Conductor conductor;

    [Export]
    private AnimatedSprite2D fire;

    [Export]
    private float flareDurationInBeats;
    private float flareDuration;

    private int smallFireAnimationFrames;
    private int smallFireCurrentFrame;

    private int largeFireAnimationFrames;
    private int largeFireCurrentFrame;

    private bool flaring = false;
    private float currentFlareDuration;

    public override void _Ready()
	{
        conductor.OnBeat += OnBeat;

        smallFireAnimationFrames = fire.SpriteFrames.GetFrameCount("default");
        largeFireAnimationFrames = fire.SpriteFrames.GetFrameCount("flare");

        fire.Centered = true;

        fire.Animation = "default";

        flareDuration = (float)(flareDurationInBeats * conductor.Song.SPB);

        GD.Print(Name);
    }

	public override void _Process(double delta)
	{
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
        fire.Animation = "flare";

        fire.Frame = largeFireCurrentFrame;

        flaring = true;
        currentFlareDuration = 0;
    }

    private void UnFlare()
    {
        flaring = false;

        fire.Animation = "default";

        fire.Frame = smallFireCurrentFrame;
    }

    private void OnBeat()
    {
        GD.Print(Name + "OnBeat");
        smallFireCurrentFrame = (smallFireCurrentFrame + 1) % smallFireAnimationFrames;
        largeFireCurrentFrame = (largeFireCurrentFrame + 1) % largeFireAnimationFrames;

        if (flaring)
        {
            fire.Frame = largeFireCurrentFrame;
        }
        else
        {
            fire.Frame = smallFireCurrentFrame;
        }


    }
}
