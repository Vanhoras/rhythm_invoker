using Godot;

public partial class HighScoreManager : Node
{
	private long _highScore;
	public long HighScore { get => _highScore; }

    private long _lastScore;
    public long LastScore { get => _lastScore; }

    public override void _Ready()
	{
	}

	public void SetScore(long score)
	{
        _lastScore = score;

		if (score > _highScore)
		{
			_highScore = score;
		}
    }
}
