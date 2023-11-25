using Godot;
using Godot.Collections;

public partial class Spawner : Node
{
	[Export]
	private Node fireballBaseNode;

    [Export]
    private PackedScene fireballPrefab;

    [Export]
    private Node2D[] spawnpoints;

    [Export]
    public Conductor conductor;

    [Export]
    public Circle circle;

    [Export]
    private float lengthToPerfect;

    [Export]
    private float lengthToMissed;

    private Vector2 circleCenter;

    public override void _Ready()
	{
        circleCenter = circle.GlobalPosition;

        conductor.OnBeat += OnBeat;
    }

    private void OnBeat()
    {
        int currentBeatTotal = conductor.CurrentBeatTotal;

        Array<Note> notes = conductor.Song.Notes;

        for (int i = 0; i < notes.Count; i++) { 
            if (notes[i].SpawnOnBeat == currentBeatTotal)
            {
                SpawnFireball(notes[i].SpawnIndex);
            }
        }
    }

    public void SpawnFireball(int SpawnIndex)
    {
        Fireball fireball = (Fireball)fireballPrefab.Instantiate();
        fireball.GlobalPosition = spawnpoints[SpawnIndex - 1].GlobalPosition;
        fireball.LookAt(circleCenter);
        fireball.RotationDegrees = fireball.RotationDegrees - 48;
        fireball.Initialize(conductor, circleCenter, lengthToPerfect, lengthToMissed);

        fireballBaseNode.AddChild(fireball);
    }
}
