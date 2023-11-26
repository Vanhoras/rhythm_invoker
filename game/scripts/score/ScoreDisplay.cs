using Godot;

public partial class ScoreDisplay : Node
{
	[Export]
	private Label ScoreLabel;

	public void DisplayScore(long score)
	{
		ScoreLabel.Text = score.ToString();
	}
}
