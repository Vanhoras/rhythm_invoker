using Godot;
using System;

public partial class Conductor : AudioStreamPlayer
{
	[Signal]
    public delegate void OnBeatEventHandler();

    [Signal]
    public delegate void OnHalfBeatEventHandler();

    [Signal]
    public delegate void OnMeasureEventHandler();

    [Export]
	private Song _song;

	public Song Song {  
		get { return _song; } 
		set { 
			PlaySong(value);
		}
	}

    private int _currentMeasure = 0;
	private int _currentBeat = 1;
	private int _currentBeatTotal = 0;

	public int CurrentMeasure { get { return _currentMeasure; } }
	public int CurrentBeat { get { return _currentBeat; } }
    public int CurrentBeatTotal { get { return _currentBeatTotal; } }

    private double _songPosition;
    private double _songPositionDelta;

	private bool _timerBeforePlaying;
	private double _timer;

    private bool _playing;
    public new bool Playing { get { return _playing; } }

    private bool invokedHalfBeat;

    public override void _Ready()
	{
		base._Ready();
        PlaySong(Song);

        this.Finished += SongFinished;
    }

    public override void _PhysicsProcess(double delta)
	{
        if (_timerBeforePlaying)
        {
            _timer += delta;

            if (_currentBeatTotal < Song.BeatsBeforeStart -1)
            {
                
                int positionInBeats = (int)Math.Floor(_timer / Song.SPB);

                if (!invokedHalfBeat && ((_timer % Song.SPB) > Song.SPB / 2))
                {
                    EmitSignal(SignalName.OnHalfBeat);
                    invokedHalfBeat = true;
                }

                if (positionInBeats > _currentBeatTotal)
                {
                    OnBeatHandler();
                }
            } else
            {
                if (_timer >= (Song.BeatsBeforeStart * Song.SPB) - (AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency()))
                {
                    _timerBeforePlaying = false;
                    _timer = 0;
                    _playing = true;
                    Play();
                }
            }

        } else if (_playing)
		{
            CalculateSongPosition();

            int positionInBeats = (int)Math.Floor(_songPosition / Song.SPB);

            if (!invokedHalfBeat && ((_songPosition % Song.SPB) > Song.SPB / 2))
            {
                EmitSignal(SignalName.OnHalfBeat);
                invokedHalfBeat = true;
            }

            if (positionInBeats > _currentBeatTotal - Song.BeatsBeforeStart)
            {
                OnBeatHandler();
            }
        }
    }

	public void PlaySong(Song song)
	{
        
        _song = song;
        Stream = Song.AudioStream;

        StopSong();
        
        if (Song.BeatsBeforeStart > 0)
        {
            _timerBeforePlaying = true;
            
        } else
        {
            Play();
            _playing = true;
        }
    }

    public void StopSong()
    {
        Stop();
        _playing = false;
        _currentMeasure = 0;
        _currentBeat = 1;
        _currentBeatTotal = 0;
        _songPosition = 0;
        _songPositionDelta = 0;
        _timer = 0;
    }

    private void SongFinished()
    {
        StopSong();
        GD.Print("Finished");
    }

    public double GetDistanceFromClosestBeat()
    {
        return 0;
    }

    public double GetSongPosition()
    {
        return _songPosition;
    }

    public double GetSongPositionDelta()
    {
        return _songPositionDelta;
    }

    private void OnBeatHandler()
	{
		_currentBeat++;
        _currentBeatTotal++;
        invokedHalfBeat = false;


        if (_currentBeat > _song.BeatPerMeasure)
		{
			_currentBeat = 1;
			_currentMeasure++;
			EmitSignal(SignalName.OnMeasure);
		}

		EmitSignal(SignalName.OnBeat);
        // GD.Print($"Beat _currentBeatTotal {_currentBeatTotal} ");
    }

	private double CalculateSongPosition()
	{
        double songPosition = GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
        songPosition -= AudioServer.GetOutputLatency();

        if (_songPosition > songPosition)
        {
            songPosition = _songPosition;
        }

        _songPositionDelta = songPosition - _songPosition;

        _songPosition = songPosition;

		return songPosition;
    }
}
