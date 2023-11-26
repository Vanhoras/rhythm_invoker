using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class FireballManager : Node
{
	[Export]
	private Node fireballBaseNode;

    [Export]
    private PackedScene fireballPrefab;

    [Export]
    private HitNotification hitNotification;

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
                GD.Print("OK");
                fireballs[closestIndex].QueueFree();
                fireballs.Remove(closestIndex);
                hitNotification.ShowHitNotification(HitType.OK);
            } else
            {
                GD.Print("PERFECT");
                fireballs[closestIndex].QueueFree();
                fireballs.Remove(closestIndex);
                hitNotification.ShowHitNotification(HitType.PERFECT);
            }
        }
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
        GD.Print($"OnFireballMissed {missedIndex}");
    }
}
