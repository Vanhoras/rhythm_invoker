using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Song : Resource
{
	[Export]
	private AudioStream _audioStream;

	public AudioStream AudioStream { get { return _audioStream; } }

	[Export]
	private int _bpm = 120;

	public int BPM
	{
		get { return _bpm; }
	}

	public double SPB { 
		get { 
			return 60 / (double)_bpm;
        } 
	}

    [Export]
    private int _beatPerMeasure = 4;

    public int BeatPerMeasure
    {
        get { return _beatPerMeasure; }
    }

    [Export]
    private int _beatsBeforeStart;

    public int BeatsBeforeStart
    {
        get { return _beatsBeforeStart; }
    }

    [Export]
    private Array<Note> _notes;

    public Array<Note> Notes { get { return _notes; } }
}
