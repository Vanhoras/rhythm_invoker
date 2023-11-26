using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class FireballManager : Node
{
    [Signal]
    public delegate void OnPerfectEventHandler();

    [Export]
	private Node fireballBaseNode;

    [Export]
    private ScoreManager scoreManager;

    [Export]
    public Circle circle;

    [Export]
    public Conductor conductor;

    [Export]
    private HitNotification hitNotification;

    [Export]
    private Node2D[] spawnpoints;


    [Export]
    private PackedScene fireballPrefab;

    [Export]
    private float lengthToPerfect;

    [Export]
    private float lengthToMissed;

    [Export]
    private float perfectZoneTimeInBeats;
    private float perfectZoneTime;

    [Export]
    private float okZoneTimeInBeats;
    private float okZoneTime;

    private Vector2 circleCenter;

    private Godot.Collections.Dictionary<int, Fireball> fireballs = new();

    private int fireballsSpawned = 0;

    public override void _Ready()
	{
        circleCenter = circle.GlobalPosition;

        conductor.OnBeat += OnBeat;
        conductor.OnHalfBeat += OnHalfBeat;

        perfectZoneTime = (float)(perfectZoneTimeInBeats * conductor.Song.SPB);
        okZoneTime = (float)(okZoneTimeInBeats * conductor.Song.SPB);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("button"))
        {
            CheckNoteHit();
        }
    }

    private void CheckNoteHit()
    {
        float closestTimeDistance = float.MaxValue;
        float distanceFromMissed;
        int closestIndex = -1;

        foreach (KeyValuePair<int, Fireball> entry in fireballs)
        {
            distanceFromMissed = entry.Value.GetTimeDistanceFromMissed();
            if (distanceFromMissed < closestTimeDistance)
            {
                closestTimeDistance = distanceFromMissed;
                closestIndex = entry.Key;
            }
        }

        if (closestIndex != -1)
        {
            float distanceFromPerfect = fireballs[closestIndex].GetTimeDistanceFromPerfect();

            if (distanceFromPerfect > okZoneTime)
            {
                fireballs[closestIndex].Miss();
            } else if (distanceFromPerfect > perfectZoneTime)
            {
                fireballs[closestIndex].Hit();
                fireballs.Remove(closestIndex);

                hitNotification.ShowHitNotification(HitType.OK);
                scoreManager.HitNote(HitType.OK);
                EmitSignal(SignalName.OnPerfect);
            } else
            {
                fireballs[closestIndex].Hit();
                fireballs.Remove(closestIndex);

                hitNotification.ShowHitNotification(HitType.PERFECT);
                scoreManager.HitNote(HitType.PERFECT);
                EmitSignal(SignalName.OnPerfect);
            }
        }
    }

    private void OnHalfBeat()
    {
        GD.Print("OnHalfBeat");
        int currentBeatTotal = conductor.CurrentBeatTotal;

        Array<Note> notes = conductor.Song.Notes;

        for (int i = 0; i < notes.Count; i++)
        {
            if (notes[i].SpawnOnBeat == currentBeatTotal && notes[i].HalfBeat)
            {
                SpawnFireball(notes[i].SpawnIndex);
            }
        }
    }

    private void OnBeat()
    {
        int currentBeatTotal = conductor.CurrentBeatTotal;

        Array<Note> notes = conductor.Song.Notes;

        for (int i = 0; i < notes.Count; i++) { 
            if (notes[i].SpawnOnBeat == currentBeatTotal && !notes[i].HalfBeat)
            {
                SpawnFireball(notes[i].SpawnIndex);
            }
        }
    }

    public void SpawnFireball(int SpawnIndex)
    {
        fireballsSpawned++;

        Fireball fireball = (Fireball)fireballPrefab.Instantiate();
        fireball.GlobalPosition = spawnpoints[SpawnIndex - 1].GlobalPosition;
        fireball.LookAt(circleCenter);
        fireball.RotationDegrees = fireball.RotationDegrees - 48;
        fireball.Initialize(fireballsSpawned, conductor, circleCenter, lengthToPerfect, lengthToMissed);

        fireballBaseNode.AddChild(fireball);

        fireballs.Add(fireballsSpawned, fireball);
        fireball.OnFireballMissed += OnFireballMissed;
    }

    private void OnFireballMissed(int missedIndex)
    {
        fireballs.Remove(missedIndex);

        hitNotification.ShowHitNotification(HitType.MISSED);
        scoreManager.HitNote(HitType.MISSED);
    }
}
