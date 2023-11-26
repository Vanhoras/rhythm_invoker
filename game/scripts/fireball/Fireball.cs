using Godot;
using System;

public partial class Fireball : Node2D
{
    [Signal]
    public delegate void OnFireballMissedEventHandler(int ID);

    [Export]
    private AnimatedSprite2D sprite;

    [Export]
    private int moveTimeInBeats;

    [Export]
    private double beatsTillMissed;

    [Export]
    private double beatsUntilDisintegrateAfterHit;
    private double timeUntilDisintegrateAfterHit;

    private int _id;

    public int ID { get { return _id; } }

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

    private bool hit;
    private double elapsedTimeSinceHit;


    public void Initialize(int id, Conductor conductor, Vector2 circleCenter, float lengthToPerfect, float lengthToMissed)
	{
        this._id = id;
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

        timeUntilDisintegrateAfterHit = beatsUntilDisintegrateAfterHit * conductor.Song.SPB;

        sprite.Frame = 0;

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

        if (hit)
        {
            AnimateAfterHit(songDelta);
            return;
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

            // GD.Print($"globalPosition {GlobalPosition} pointInBetween {(float)(baseTime / effectiveElapsedTime)} timeNeededToMissed - timeNeededToPerfect {timeNeededToMissed - timeNeededToPerfect} elapsedTime - timeNeededToPerfect {elapsedTime - timeNeededToPerfect} ");
        } else
		{
            Miss();
        }
    }

    public float GetTimeDistanceFromPerfect()
    {
        return (float) Mathf.Abs(timeNeededToPerfect - elapsedTime);
    }

    public float GetTimeDistanceFromMissed()
    {
        return (float)Mathf.Abs(timeNeededToMissed - elapsedTime);
    }

    public void Miss()
    {
        EmitSignal(SignalName.OnFireballMissed, ID);
        QueueFree();
    }

    public void Hit()
    {
        hit = true;
        elapsedTimeSinceHit = 0;
        sprite.Frame = 1;

        Color newColor = sprite.Modulate;

        newColor.A = 0.7f;

        sprite.Modulate = newColor;
    }

    private void AnimateAfterHit(double delta)
    {
        elapsedTimeSinceHit += delta;

        if (elapsedTimeSinceHit >= timeUntilDisintegrateAfterHit / 2)
        {
            sprite.Frame = 2;
        } 
        
        if (elapsedTimeSinceHit >= timeUntilDisintegrateAfterHit)
        {
            QueueFree();
        }
    }
}
