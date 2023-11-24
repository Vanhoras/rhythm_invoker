using Godot;

public partial class Fireball : Node2D
{

	[Export]
	public Conductor conductor;

	[Export]
	public Circle circle;

	[Export]
	public int moveTimeInBeats;

	[Export]
	private float lengthToPerfect;

	[Export]
	private float lengthToMissed;

	[Export]
	private double beatsTillMissed;

	[Export]
	private Sprite2D sprite;


	private Vector2 startPosition;

	private Vector2 circleCenter;
	private Vector2 perfectPosition;
	private Vector2 missedPosition;

	private double timeNeededToPerfect;
    private double timeNeededToMissed;
    private double elapsedTime = 0;

    public override void _Ready()
	{
		circleCenter = circle.GlobalPosition;
        startPosition = GlobalPosition;

		Vector2 directionVector = (startPosition - circleCenter).Normalized();
		perfectPosition = circleCenter + (directionVector * lengthToPerfect);
        missedPosition = circleCenter + (directionVector * lengthToMissed);


        timeNeededToPerfect = moveTimeInBeats * conductor.Song.SPB;
		timeNeededToMissed = timeNeededToPerfect + beatsTillMissed * conductor.Song.SPB;


        GD.Print($"Fireball: timeNeededToPerfect {timeNeededToPerfect} timeNeededToMissed {timeNeededToMissed} startPosition {startPosition} circleCenter {circleCenter} perfectPosition {perfectPosition} missedPosition {missedPosition}");
    }


    public override void _PhysicsProcess(double delta)
	{
        double songDelta = conductor.GetSongPositionDelta();
        elapsedTime += songDelta;

		if (elapsedTime <= 0) return;


        if (elapsedTime < timeNeededToPerfect) {
            Vector2 intermediatePosition = startPosition + ((perfectPosition - startPosition) / (float)(timeNeededToPerfect / elapsedTime));

            GlobalPosition = intermediatePosition;
        } else if (elapsedTime < timeNeededToMissed)
		{
			float pointInBetween = (float)(timeNeededToMissed - timeNeededToPerfect / elapsedTime - timeNeededToPerfect);

            Vector2 intermediatePosition = startPosition + ((missedPosition - startPosition) / pointInBetween);

            sprite.Modulate = sprite.Modulate.Lerp(new Color(sprite.Modulate.R, sprite.Modulate.G, sprite.Modulate.B, 0), pointInBetween);

            GlobalPosition = intermediatePosition;
        } else
		{
            QueueFree();
		}


		
    }
}
