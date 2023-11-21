using Godot;
using System;

public partial class Conductor : AudioStreamPlayer
{
	[Signal]
    public delegate void OnBeatEventHandler(int currentMeasure, int currentBeat);

    [Signal]
    public delegate void OnMeasureEventHandler(int currentMeasure);

    [Export]
	private Song _song;

	public Song Song {  
		get { return _song; } 
		set { 
			_song = value;
            _currentMeasure = 0;
			_currentBeat = 1;
            _songPosition = 0;
            Stream = Song.AudioStream;
			Play();
		}
	}

    private int _currentMeasure = 0;
	private int _currentBeat = 1;
	private int _currentBeatTotal = 0;

	public int CurrentMeasure { get { return _currentMeasure; } }
	public int CurrentBeat { get { return _currentBeat; } }

	private double _songPosition;

	public override void _Ready()
	{
		base._Ready();
        Stream = Song.AudioStream;
        Play();
    }

	public override void _PhysicsProcess(double delta)
	{
        CalculateSongPosition();

		int positionInBeats = (int)Math.Floor(_songPosition / Song.SPB);
		
		if (positionInBeats > _currentBeatTotal)
		{
			OnBeatHandler();
		}
    }

	private void OnBeatHandler()
	{
		_currentBeat++;
        _currentBeatTotal++;


        if (_currentBeat > _song.BeatPerMeasure)
		{
			_currentBeat = 1;
			_currentMeasure++;
			EmitSignal(SignalName.OnMeasure, _currentMeasure);
		}

		EmitSignal(SignalName.OnBeat, _currentMeasure, _currentBeat);
    }

	private double CalculateSongPosition()
	{
        double songPosition = GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
        songPosition -= AudioServer.GetOutputLatency();

        if (_songPosition > songPosition)
        {
            songPosition = _songPosition;
        }

        _songPosition = songPosition;

		return songPosition;
    }
}
