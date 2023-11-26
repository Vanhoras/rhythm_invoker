using Godot;

[GlobalClass]
public partial class Note : Resource
{
    [Export]
    private int _spawnIndex;

    public int SpawnIndex { get { return _spawnIndex; } }

    [Export]
    private int _spawnOnBeat;

    public int SpawnOnBeat { get { return _spawnOnBeat; } }

    [Export]
    private bool _halfBeat;

    public bool HalfBeat { get { return _halfBeat; } }

    public void Initialize(int spawnIndex, int spawnOnBeat, bool halfBeat)
    {
        this._spawnIndex = spawnIndex;
        this._spawnOnBeat = spawnOnBeat;
        this._halfBeat = halfBeat;
    }
}
