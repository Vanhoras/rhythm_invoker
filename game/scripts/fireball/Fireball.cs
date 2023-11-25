using Godot;

public partial class Fireball : Node2D
{
    [Export]
    private Sprite2D sprite;

    [Export]
    private int moveTimeInBeats;

    [Export]
    private double beatsTillMissed;

    private Conductor conductor;

    private float lengthToPerfect;
	private float lengthToMissed;

	private Vector2 startPosition;

	private Vector2 circleCenter;
	private Vector2 perfectPosition;
	private Vector2 missedPosition;

	private double timeNeededToPerfect;
    private double timeNeededToMissed;
    private double elapsedTime = 0;

	public void Initialize(Conductor conductor, Vector2 circleCenter, float lengthToPerfect, float lengthToMissed)
	{
        this.conductor = conductor;
        this.circleCenter = circleCenter;
        this.lengthToPerfect = lengthToPerfect;
        this.lengthToMissed = lengthToMissed;

        startPosition = GlobalPosition;

        Vector2 directionVector = (startPosition - circleCenter).Normalized();
        perfectPosition = circleCenter + (directionVector * lengthToPerfect);
        missedPosition = circleCenter + (directionVector * lengthToMissed);


        timeNeededToPerfect = moveTimeInBeats * conductor.Song.SPB;
        timeNeededToMissed = timeNeededToPerfect + beatsTillMissed * conductor.Song.SPB;

        // GD.Print($"Fireball startPosition {startPosition} circleCenter {circleCenter} lengthToPerfect {lengthToPerfect} lengthToMissed {lengthToMissed} perfectPosition {perfectPosition} missedPosition {missedPosition} timeNeededToPerfect {timeNeededToPerfect} timeNeededToMissed {timeNeededToMissed}");
    }

    public override void _PhysicsProcess(double delta)
	{
        double songDelta;
        if (conductor.Playing)
        {
            songDelta = conductor.GetSongPositionDelta();
        } else
        {
            songDelta = delta;
        }

        
        elapsedTime += songDelta;

		if (elapsedTime <= 0) return;


        if (elapsedTime < timeNeededToPerfect) {
            Vector2 intermediatePosition = startPosition + ((perfectPosition - startPosition) / (float)(timeNeededToPerfect / elapsedTime));

            GlobalPosition = intermediatePosition;
        } else if (elapsedTime < timeNeededToMissed)
		{
            float baseTime = (float)(timeNeededToMissed - timeNeededToPerfect);
            float effectiveElapsedTime = (float)(elapsedTime - timeNeededToPerfect);

            Vector2 intermediatePosition = perfectPosition + ((missedPosition - perfectPosition) / (float)(baseTime / effectiveElapsedTime));

            Color newColor = sprite.Modulate;

            newColor.A = (float)Mathf.Lerp(1, 0, effectiveElapsedTime / baseTime);

            sprite.Modulate = newColor;

            GlobalPosition = intermediatePosition;

            GD.Print($"globalPosition {GlobalPosition} pointInBetween {(float)(baseTime / effectiveElapsedTime)} timeNeededToMissed - timeNeededToPerfect {timeNeededToMissed - timeNeededToPerfect} elapsedTime - timeNeededToPerfect {elapsedTime - timeNeededToPerfect} ");
        } else
		{
            QueueFree();
		}
    }
}
